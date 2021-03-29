using System;
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
                    //PaymentPrice = catalog.BookPrice != null ? Math.Round(catalog.BookPrice.Price * catalog.Discount.DiscountPercentage ,2 , MidpointRounding.AwayFromZero): 0m,
                    PaymentPrice = catalog.BookPrice != null ? Math.Round((decimal)(catalog.Amount/catalog.BookCount), 2, MidpointRounding.AwayFromZero) : 0m,
                    ISBN = catalog.Book.ISBN,
                    Publisher = catalog.Book.Publisher
                };
                bookInfos.Add(bookInfo);
            }

            return bookInfos;
        }

        public List<BookInCatalogInfo> GetBooksInCatalogGroupByBook(int groupID, IUnitOfWork uow)
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
                    TotalAmount = catalog.Amount.HasValue ? catalog.Amount : 0m,
                    Price = catalog.BookPrice != null ? catalog.BookPrice.Price : 0m,
                    //PaymentPrice = catalog.BookPrice != null ? Math.Round(catalog.BookPrice.Price * catalog.Discount.DiscountPercentage ,2 , MidpointRounding.AwayFromZero): 0m,
                    PaymentPrice = catalog.BookPrice != null ? Math.Round((decimal)(catalog.Amount / catalog.BookCount), 2, MidpointRounding.AwayFromZero) : 0m,
                    ISBN = catalog.Book.ISBN,
                    Publisher = catalog.Book.Publisher,
                    DiscountID = catalog.DiscountID
                };
                bookInfos.Add(bookInfo);
            }

            bookInfos = bookInfos
                .GroupBy(l => new { l.BookKpsID, l.DiscountID })                     
                .Select(cl => new BookInCatalogInfo()
                {
                    ID = cl.First().ID,
                    BookKpsID = cl.First().BookKpsID,
                    BookCount = cl.Sum(c => c.BookCount),
                    Title = cl.First().Title,
                    Author = cl.First().Author,
                    TotalAmount = cl.Sum(c => c.TotalAmount),
                    Price = cl.First().Price,
                    PaymentPrice = cl.First().PaymentPrice,
                    ISBN = cl.First().ISBN,
                    Publisher = cl.First().Publisher
                }).ToList();

            return bookInfos.OrderBy(x => x.Title).ToList();
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

        public bool DoesNotHavePendingPrices
        {
            get
            {
                bool condition = (!ContainsCommitteePendingPrices.HasValue ||
                                 !ContainsCommitteePendingPrices.Value)
                                &&
                                (!ContainsUnexpectedPendingPrices.HasValue ||
                                 !ContainsUnexpectedPendingPrices.Value);
                return condition;
            }
        }

        public bool? ContainsCommitteePendingPrices
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

        public bool? ContainsUnexpectedPendingPrices
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
