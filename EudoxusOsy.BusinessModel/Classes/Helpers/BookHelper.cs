using Imis.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EudoxusOsy.BusinessModel
{
    public static class BookHelper
    {
        public static void TogglePriceVerification(IUnitOfWork uow, int bookID, bool toggle, bool updatePrice = false)
        {
            var book = new BookRepository(uow).Load(bookID);
            book.PendingCommitteePriceVerification = toggle;

            List<Catalog> catalogs = new CatalogRepository(uow).FindByBookID(bookID).Where(x => (x.PhaseID >= 13)).ToList();
            catalogs.ForEach(c => c.HasPendingPriceVerification = toggle);

            if (updatePrice)
            {
                var currentPhaseID = EudoxusOsyCacheManager<Phase>.Current.GetItems().Max(x => x.ID);
                var currentPhase = new PhaseRepository(uow).Load(currentPhaseID);

                //TODO: Do not change the existing bookPrice if there are catalogs using it
                //TODO: Build Reversal catalogs with price difference.
                BookPrice bookPrice = new BookPriceRepository(uow).FindByBookIDAndYear(bookID, currentPhase.Year);
                if (book.LastPriceChange != null)
                {
                    bookPrice.Price = book.LastPriceChange.Price.Value;
                }
            }

            uow.Commit();
        }

        public static void TogglePriceVerification(IUnitOfWork uow, Book book, bool toggle, bool updatePrice = false)
        {
            book.PendingCommitteePriceVerification = toggle;

            List<Catalog> catalogs = new CatalogRepository(uow).FindByBookID(book.ID).Where(x => (x.PhaseID >= 13)).ToList();
            catalogs.ForEach(c => c.HasPendingPriceVerification = toggle);

            if (updatePrice)
            {
                var currentPhaseID = EudoxusOsyCacheManager<Phase>.Current.GetItems().Max(x => x.ID);
                var currentPhase = new PhaseRepository(uow).Load(currentPhaseID);

                //TODO: Do not change the existing bookPrice if created Catalogs exist
                //TODO: Build Reversal catalogs with price difference.
                BookPrice bookPrice = new BookPriceRepository(uow).FindByBookIDAndYear(book.ID, currentPhase.Year);
                if (book.LastPriceChange != null)
                {
                    bookPrice.Price = book.LastPriceChange.Price.Value;
                }
            }

            uow.Commit();
        }

        public static void ToggleUnexpectedPriceChange(IUnitOfWork uow, int bookID, bool toggle, bool updatePrice = false)
        {
            var book = new BookRepository(uow).Load(bookID);

            var currenPhase = new PhaseRepository(uow).GetCurrentPhase();
            List<Catalog> catalogs = new CatalogRepository(uow).FindByBookID(bookID).Where(x => (x.PhaseID >= 13)).ToList();

            if (toggle)
            {
                book.HasUnexpectedPriceChangePhaseID = currenPhase.ID;

            }
            else
            {
                book.HasUnexpectedPriceChangePhaseID = null;
            }
            catalogs.ForEach(c => c.HasUnexpectedPriceChange = toggle);

            if (updatePrice)
            {
                var currentPhaseID = EudoxusOsyCacheManager<Phase>.Current.GetItems().Max(x => x.ID);
                var currentPhase = new PhaseRepository(uow).Load(currentPhaseID);

                //Do not change the existing bookPrice if created Catalogs exist
                //Build Reversal catalogs with price difference.
                BookPrice bookPrice = new BookPriceRepository(uow).FindByBookIDAndYear(bookID, currentPhase.Year);
                if (book.LastPriceChange != null)
                {
                    bookPrice.Price = book.LastPriceChange.Price.Value;
                }
            }

            uow.Commit();
        }

        public static void ToggleUnexpectedPriceChange(IUnitOfWork uow, Book book, bool toggle, bool updatePrice = false)
        {
            var currenPhase = new PhaseRepository(uow).GetCurrentPhase();
            List<Catalog> catalogs = new CatalogRepository(uow).FindByBookID(book.ID).Where(x => (x.PhaseID >= 13)).ToList();

            if (toggle)
            {
                book.HasUnexpectedPriceChangePhaseID = currenPhase.ID;

            }
            else
            {
                book.HasUnexpectedPriceChangePhaseID = null;
            }
            catalogs.ForEach(c => c.HasUnexpectedPriceChange = toggle);

            if (updatePrice)
            {
                var currentPhaseID = EudoxusOsyCacheManager<Phase>.Current.GetItems().Max(x => x.ID);
                var currentPhase = new PhaseRepository(uow).Load(currentPhaseID);

                //TODO: Do not change the existing bookPrice if created Catalogs exist
                //TODO: Build Reversal catalogs with price difference.
                BookPrice bookPrice = new BookPriceRepository(uow).FindByBookIDAndYear(book.ID, currentPhase.Year);
                if (book.LastPriceChange != null)
                {
                    bookPrice.Price = book.LastPriceChange.Price.Value;
                }
            }

            uow.Commit();
        }

        public static void DoUpdateBooksAndCatalogs(string action, int id, Phase currentPhase, decimal? newPrice, IUnitOfWork uow)
        {
            /**
                when unlocked do the following:
                1. toggle the pending price check flag from books and catalogs, IF THE PRICE WAS JUST APPROVED NOTHING ELSE NEEDS TO HAPPEN
                2. find the unconnected catalogs/ or those which are in new groups of the book for phaseID >= 13 and change the price
                3a. Set status = 0 of the bookprice of the current year (invalid) and create a new active  bookPrice for the current year
                3b. find the connected catalogs that are in processed groups, create the reversal catalogs with the price difference 
            */

            if (newPrice.HasValue && action == "unlock")
            {
                //mark the active BookPrice as invalid
                var oldBookPrice = new BookPriceRepository(uow).FindByBookIDAndYear(id, currentPhase.Year);
                oldBookPrice.Status = enBookPriceStatus.Invalid;

                //Create a new bookPrice
                var newBookPrice = new BookPrice();
                newBookPrice.Year = currentPhase.Year;
                newBookPrice.Price = newPrice.Value;
                newBookPrice.BookID = id;
                newBookPrice.Status = enBookPriceStatus.Active;
                newBookPrice.CreatedAt = DateTime.Now;
                newBookPrice.CreatedBy = "sysadmin";
                uow.MarkAsNew(newBookPrice);
                uow.Commit();

                CatalogGroupHelper.RecalculateCatalogsForBook(id, currentPhase.ID, newPrice.Value, newBookPrice.ID, uow);
                CatalogGroupHelper.CreatePriceDifferenceCatalogsForBook(id, currentPhase.ID, newPrice.Value, newBookPrice.ID, uow);
            }
            TogglePriceVerification(uow, id, action == "lock", action == "unlock");
        }

        public static void DoUpdateBooksAndCatalogsUnexpected(string action, int id, Phase currentPhase, decimal? newPrice, IUnitOfWork uow)
        {
            /**
                when unlocked do the following:
                1. toggle the pending price check flag from books and catalogs, IF THE PRICE WAS JUST APPROVED NOTHING ELSE NEEDS TO HAPPEN
                2. find the unconnected catalogs/ or those which are in new groups of the book for phaseID >= 13 and change the price
                3a. Set status = 0 of the bookprice of the current year (invalid) and create a new active  bookPrice for the current year
                3b. find the connected catalogs that are in processed groups, create the reversal catalogs with the price difference 
            */

            if (newPrice.HasValue && action == "unlock")
            {
                //mark the active BookPrice as invalid
                var oldBookPrice = new BookPriceRepository(uow).FindByBookIDAndYear(id, currentPhase.Year);
                oldBookPrice.Status = enBookPriceStatus.Invalid;

                //Create a new bookPrice
                var newBookPrice = new BookPrice();
                newBookPrice.Year = currentPhase.Year;
                newBookPrice.Price = newPrice.Value;
                newBookPrice.BookID = id;
                newBookPrice.Status = enBookPriceStatus.Active;
                newBookPrice.CreatedAt = DateTime.Now;
                newBookPrice.CreatedBy = "sysadmin";
                uow.MarkAsNew(newBookPrice);
                uow.Commit();

                CatalogGroupHelper.RecalculateCatalogsForBook(id, currentPhase.ID, newPrice.Value, newBookPrice.ID, uow);
                CatalogGroupHelper.CreatePriceDifferenceCatalogsForBook(id, currentPhase.ID, newPrice.Value, newBookPrice.ID, uow);
            }
            ToggleUnexpectedPriceChange(uow, id, action == "lock", action == "unlock");
        }


    }
}
