using Imis.Domain;
using System.Collections.Generic;
using System.Linq;

namespace EudoxusOsy.BusinessModel
{
    public partial class CatalogGroup : IUserChangeTracking
    {
        public enCatalogGroupState State
        {
            get { return (enCatalogGroupState)StateInt; }
            set
            {
                if (StateInt != (int)value)
                    StateInt = (int)value;
            }
        }

        public List<BookInCatalogInfo> GetBooksInCatalogGroup(int groupID, IUnitOfWork uow)
        {

            var catalogs = new CatalogRepository(uow).FindByGroupIDWithBooks(groupID);

            List<BookInCatalogInfo> bookInfos = new List<BookInCatalogInfo>();

            foreach (var catalog in catalogs)
            {
                var bookInfo = new BookInCatalogInfo()
                {
                    ID = catalog.BookID,
                    BookKpsID = catalog.Book.BookKpsID,
                    BookCount = catalog.BookCount,
                    Title = catalog.Book.Title,
                    Author = catalog.Book.Author,
                    Department = EudoxusOsyCacheManager<Department>.Current.Get(catalog.DepartmentID).Name,
                    TotalAmount = catalog.Amount.HasValue ? catalog.Amount : 0m,
                    Price = catalog.BookPrice != null ? catalog.BookPrice.Price : 0m,
                    PaymentPrice = catalog.BookCount > 0 && catalog.Amount != null ? (decimal)catalog.Amount / catalog.BookCount : 0m,
                    ISBN = catalog.Book.ISBN,
                    Publisher = catalog.Book.Publisher
                };
                bookInfos.Add(bookInfo);
            }

            return bookInfos;
        }

        public decimal? TotalAmount
        {
            get
            {
                if (Catalogs == null)
                    return null;

                return Catalogs
                        .Where(x => x.StateInt == (int)enCatalogState.Normal || x.StateInt == (int)enCatalogState.FromMove)
                        .Where(x => x.StatusInt == (int)enCatalogStatus.Active)
                        .Sum(x => x.Amount);
            }
        }

        public decimal? InvoiceSum
        {
            get
            {
                if (Invoices == null)
                    return null;

                return Invoices
                        .Where(x => x.IsActive)
                        .Sum(x => x.InvoiceValue);
            }
        }

        public bool? ContainsInActiveBooks
        {
            get
            {
                if (Catalogs == null)
                    return null;

                return Catalogs
                        .Any(x => !x.IsBookActive);
            }
        }

        public bool? ContainsPendingPriceVerificationCatalogs
        {
            get
            {
                if (Catalogs == null)
                    return null;

                return Catalogs
                        .Where(x => x.PhaseID >= 13)
                        .Any(x => x.HasPendingPriceVerification);
            }
        }

        public bool? ContainsCatalogsWithUnexpectedPriceChanges
        {
            get
            {
                if (Catalogs == null)
                    return null;

                return Catalogs
                        .Where(x => x.PhaseID >= 13)
                        .Any(x => x.HasUnexpectedPriceChange);
            }
        }
    }
}
