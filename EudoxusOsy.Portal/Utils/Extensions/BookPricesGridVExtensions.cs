using System;
using EudoxusOsy.BusinessModel;
using Imis.Domain;

namespace EudoxusOsy.Portal
{
    public static class BookPricesGridVExtensions
    {
        public static void ApproveFuturePrice(this BookPricesGridV bookPrice, IUnitOfWork uow)
        {
            if (bookPrice.ChangeYear.Value > PhaseHelper.MaxYear())
            {
                var bookPriceDb = new BookPriceRepository(uow).FindByBookIDAndYear(bookPrice.BookID,
                    bookPrice.ChangeYear.Value);

                if (bookPriceDb == null)
                {
                    BookPrice newBookPrice = new BookPrice()
                    {
                        Year = bookPrice.ChangeYear.Value,
                        BookID = bookPrice.BookID,
                        IsChecked = bookPrice.PriceChecked.HasValue? bookPrice.PriceChecked.Value: false,
                        Status = enBookPriceStatus.Active,
                        Price = bookPrice.Price.HasValue? bookPrice.Price.Value: 0,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "sysadmin"
                    };

                    uow.MarkAsNew(newBookPrice);
                }
                else
                {
                    bookPriceDb.Price = bookPrice.Price.HasValue? bookPrice.Price.Value: 0;
                    bookPriceDb.UpdatedAt = DateTime.Now;
                    bookPriceDb.UpdatedBy = "sysadmin";
                }

                BookPriceChange bookPriceChange = new BookPriceChangeRepository(uow).Load(bookPrice.BookPriceID.Value);
                bookPriceChange.Approved = true;

                BookPricesGridV otherPriceChange = new BookPricesGridVRepository(uow).AdditionalApprovalPending(bookPrice.ChangeYear.Value, bookPrice.BookID);


                if (otherPriceChange == null)
                {
                    var book = new BookRepository(uow).FindByID(bookPrice.BookID);
                    book.PendingCommitteePriceVerification = false;
                }           

                uow.Commit();
            }            
        }

    }
}