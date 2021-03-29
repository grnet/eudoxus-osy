using Imis.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EudoxusOsy.BusinessModel
{
    public static class BookHelper
    {        
        public static void TogglePriceVerification(IUnitOfWork uow, BookPricesGridV bookPrice, bool lockIt, int phaseId)
        {
            var book = new BookRepository(uow).Load(bookPrice.BookID);

            if (lockIt)
            {
                book.PendingCommitteePriceVerification = lockIt;
            }
            else
            {
                BookPricesGridV otherPriceChange = new BookPricesGridVRepository().AdditionalApprovalPending(bookPrice.ChangeYear.Value, bookPrice.BookID);

                if (otherPriceChange == null)
                {
                    book.PendingCommitteePriceVerification = lockIt;
                }                
            }
            List<Catalog> catalogs = new CatalogRepository(uow).FindByBookID(bookPrice.BookID).Where(x => (x.PhaseID == phaseId)).ToList();
            catalogs.ForEach(c => c.HasPendingPriceVerification = lockIt) ;                        
        }
     
        public static void ToggleUnexpectedPriceChange(IUnitOfWork uow, BookPricesGridV bookPrice, bool lockIt, int phaseId)
        {
            var book = new BookRepository(uow).Load(bookPrice.BookID);
                        
            if (lockIt)
            {
                var currentCatalogsPhase = new PhaseRepository(uow).GetCurrentCatalogsPhase();
                book.HasUnexpectedPriceChangePhaseID = currentCatalogsPhase.ID;
            }
            else
            {
                BookPricesGridV otherPriceChange = new BookPricesGridVRepository().AdditionalApprovalPending(bookPrice.ChangeYear.Value, bookPrice.BookID);

                if (otherPriceChange == null)
                {
                    book.HasUnexpectedPriceChangePhaseID = null;
                }
            }

            List<Catalog> catalogs = new CatalogRepository(uow).FindByBookID(bookPrice.BookID).Where(x => (x.PhaseID == phaseId)).ToList();

            catalogs.ForEach(c => c.HasUnexpectedPriceChange = lockIt);            
        }

        public static void DoUpdateBooksAndCatalogs(string action, BookPricesGridV bookPrice, List<Phase> phases, IUnitOfWork uow)
        {
            /**
                when unlocked do the following:
                1. toggle the pending price check flag from books and catalogs, IF THE PRICE WAS JUST APPROVED NOTHING ELSE NEEDS TO HAPPEN
                2. find the unconnected catalogs/ or those which are in new groups of the book for phaseID and change the price
                3a. Set status = 0 of the bookprice of the current year (invalid) and create a new active  bookPrice for the current year
                3b. find the connected catalogs that are in processed groups, create the reversal catalogs with the price difference 
            */
            MarkPrices(action, bookPrice, phases, uow);

            foreach (var phase in phases)
            {
                TogglePriceVerification(uow, bookPrice, action == "lock", phase.ID);
            }
            
            if (action == "unlock")
            {
                BookPriceChange bookPriceChange = new BookPriceChangeRepository(uow).Load(bookPrice.BookPriceID.Value);
                bookPriceChange.Approved = true;
            }
            uow.Commit();
        }

        public static void DoUpdateBooksAndCatalogsUnexpected(string action, BookPricesGridV bookPrice, List<Phase> phases, IUnitOfWork uow)
        {
            /**
                when unlocked do the following:
                1. toggle the pending price check flag from books and catalogs, IF THE PRICE WAS JUST APPROVED NOTHING ELSE NEEDS TO HAPPEN
                2. find the unconnected catalogs/ or those which are in new groups of the book for phaseID and change the price
                3a. Set status = 0 of the bookprice of the current year (invalid) and create a new active bookPrice for the current year
                3b. find the connected catalogs that are in processed groups, create the reversal catalogs with the price difference 
            */
            MarkPrices(action, bookPrice, phases, uow);

            foreach (var phase in phases)
            {
                ToggleUnexpectedPriceChange(uow, bookPrice, action == "lock", phase.ID);
            }
            
            if (action == "unlock")
            {
                BookPriceChange bookPriceChange = new BookPriceChangeRepository(uow).Load(bookPrice.BookPriceID.Value);
                bookPriceChange.Approved = true;
            }
            uow.Commit();
        }

        private static void MarkPrices(string action, BookPricesGridV bookPrice, List<Phase> phases, IUnitOfWork uow)
        {
            var newPrice = bookPrice.Price.HasValue ? bookPrice.Price : bookPrice.SuggestedPrice;
            var isChecked = bookPrice.PriceChecked.HasValue ? bookPrice.PriceChecked.Value : false;
            
            if (newPrice.HasValue && action == "unlock")
            {
                MarkOldBookPrice(bookPrice.BookID, phases[0].Year, uow);
                CreateBookPrice(bookPrice.BookID, phases, newPrice, isChecked, uow);
            }
        }

        private static void CreateBookPrice(int id, List<Phase> phases, decimal? newPrice, bool isChecked,
            IUnitOfWork uow)
        {
            //Create a new bookPrice
            var newBookPrice = new BookPrice();
            newBookPrice.Year = phases[0].Year;
            newBookPrice.Price = newPrice.Value;
            newBookPrice.BookID = id;
            newBookPrice.Status = enBookPriceStatus.Active;
            newBookPrice.CreatedAt = DateTime.Now;
            newBookPrice.CreatedBy = "sysadmin";
            newBookPrice.IsChecked = isChecked;
            uow.MarkAsNew(newBookPrice);
            uow.Commit();

            foreach (var currentCatalogsPhase in phases)
            {
                CatalogGroupHelper.RecalculateCatalogsForBook(id, currentCatalogsPhase.ID, newPrice.Value, newBookPrice.ID, uow);
                CatalogGroupHelper.CreatePriceDifferenceCatalogsForBook(id, currentCatalogsPhase.ID, newPrice.Value,
                    newBookPrice.ID, uow);
            }            
        }

        private static void MarkOldBookPrice(int id, int year, IUnitOfWork uow)
        {
            //mark the active BookPrice as invalid
            var oldBookPrice = new BookPriceRepository(uow).FindByBookIDAndYear(id, year);
            oldBookPrice.Status = enBookPriceStatus.Invalid;
            oldBookPrice.UpdatedAt = DateTime.Now;
            oldBookPrice.UpdatedBy = "sysadmin";
        }


        public static enErrorCode? CheckBookServiceBusinessLogic(IBook book, BookDTO dto, BookPrice bookPrice = null)
        {
            enErrorCode? error = null;

            if (dto.checkedPrice == false && dto.price.HasValue)
            {
                error = enErrorCode.PriceCheckedFalseButMinistryPrice;
            }
            else if (bookPrice != null && bookPrice.IsChecked && dto.checkedPrice != true)
            {
                error = enErrorCode.BookIsCheckedInOsyButNotCheckedPriceFromKpsService;
            }
            else if (book != null && book.PendingCommitteePriceVerification == true && dto.checkedPrice == false)
            {
                error = enErrorCode.BookHasPendingPriceVerificationButUnCheckedPriceChangeReceived;
            }

            return error;
        }

        public static enBookType MapKindBookToBookType(string kindbook)
        {
            switch (kindbook.ToLower())
            {
                case "selfpublished":
                    return enBookType.SelfPublished;
                case "published":
                case "regular":
                    return enBookType.Regular;
                case "epublished":
                    return enBookType.EPublished;
                case "ebook":
                    return enBookType.eBook;
                case "professornotes":
                    return enBookType.ProfessorNotes;
                default:
                    return enBookType.Other;
            }
        }

    }
}
