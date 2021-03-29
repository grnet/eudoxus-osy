using EudoxusOsy.Utils;
using Imis.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EudoxusOsy.BusinessModel.Classes.Helpers;

namespace EudoxusOsy.BusinessModel
{
    public class UpdateBooksHelper
    {           
        public static void ProcessNewBooksResponse(IUnitOfWork uow, StringBuilder sb, IGetBookResponse newBooksResponse)
        {
            List<Book> books = new List<Book>();
            List<Book> existingBooks = new List<Book>();

            if (newBooksResponse != null && newBooksResponse.books != null && newBooksResponse.books.Count > 0)
            {
                sb.AppendFormat("booksResponse: {0} \r\n", newBooksResponse);
                sb.AppendFormat("booksResponse results count : {0} \r\n", newBooksResponse.numResults);

                /**
                    If the book already exists in OSY, we update its KPS status as modified, so that the GetModified books
                    algorithm will take care of the price changes
                */
                newBooksResponse.books.ForEach(x => { ProcessFromDtoNew(uow, sb, books, existingBooks, x); });
                uow.Commit();
            }

            int lastBook = 0;

            try
            {
                if (Config.EnableKPSUpdate)
                {
                    foreach (var newBook in books)
                    {
                        lastBook = newBook.BookKpsID;
                        UpdateKPS(newBook.ID, newBook.BookKpsID, "Full", sb);
                    }

                    foreach (var existingBook in existingBooks)
                    {
                        lastBook = existingBook.BookKpsID;
                        UpdateKPS(existingBook.ID, existingBook.BookKpsID, "Modified", sb);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogMessage(ex.Message + " " + (ex.InnerException != null ? ex.InnerException.Message : ""), typeof(UpdateBooksHelper));
                sb.AppendFormat("\nbook: " + lastBook  + " ΣΦΑΛΜΑ κατά την ενημέρωση του ΚΠΣ.\n");
            }
        }
        private static void ProcessFromDtoNew(IUnitOfWork uow, StringBuilder sb, List<Book> books, List<Book> existingBooks, BookDTO bookDto)
        {
            Book book = new BookRepository(uow).FindByBookKpsID(bookDto.id).FirstOrDefault();
            sb.AppendFormat("looking for book : {0} {1} \r\n", (int)bookDto.id, bookDto.title);
            
            var error = CheckBookBusinessLogic(book, bookDto);
            if (error.HasValue)
            {
                string errorTxt = string.Format("BL error -{2}- for book : {0} {1}  \r\n", (int)bookDto.id, bookDto.title, error.GetLabel());

                BusinessLogicErrorsKP logicError = new BusinessLogicErrorsKP();
                logicError.ErrorEntityType = enErrorEntityType.Book;
                logicError.ErrorCode = error.Value;
                logicError.EntityID = bookDto.id;
                logicError.Description = errorTxt;
                logicError.CreatedAt = DateTime.Now;
                uow.MarkAsNew(logicError);
                sb.AppendFormat(errorTxt);
            }
            else
            {
                if (book == null)
                {
                    Book newBook = bookDto.ToOsyBook();
                    uow.MarkAsNew(newBook);

                    foreach (ArchiveDTO archiveDto in bookDto.archives)
                    {                        
                        CreateBookPrice(archiveDto.year, archiveDto.ArchiveDtoToBookDto(), newBook, uow);
                    }                    
                    sb.AppendFormat("book created in OSY : Kpsid: {0} {1} \r\n", (int)bookDto.id, bookDto.title);

                    books.Add(newBook);
                }
                else
                {
                    book.UpdateBookFromDto(bookDto);
                    sb.AppendFormat("book: {0} {1} ALREADY EXISTS, Data updated, but not the price \r\n", (int)bookDto.id, bookDto.title);

                    existingBooks.Add(book);
                }
            }
        }
        //---------------------------------------------------------
        //Modified books processing                
        public static string ProcessModifiedBooksResponse(IUnitOfWork uow, List<Archive> archives)
        {
            StringBuilder sb = new StringBuilder();

            var modifiedBookKpsIDs = archives.Select(x => x.BookKpsID);
            var yearsAffected = archives.Select(x => x.Year).Distinct();

            if (!modifiedBookKpsIDs.Any() || !yearsAffected.Any())
            {
                return sb.Append("Empty DATA!!!").ToString();
            }
            /**
             Get the books and the book prices to reduce stress of the DB
            */
            List<Book> books = GetBooks(uow, null, modifiedBookKpsIDs);
            Dictionary<int, List<int>> bookDictionary = new Dictionary<int, List<int>>();            

            foreach (var year in yearsAffected)
            {                
                bookDictionary.Add(year, GetModifiedBooksIDsWithCatalogsInYear(uow, books, year));
            }             
             
            foreach (Archive archive in archives)
            {
                sb.AppendFormat("looking for book : {0} ", (int)archive.BookKpsID);
                var book = books.FirstOrDefault(x => x.BookKpsID == archive.BookKpsID);

                if (book != null && book.ID != 0)
                {
                    sb.AppendFormat("-> processing book : {0} \r\n", archive.BookKpsID);
                }
                else
                {
                    sb.AppendFormat("-> ERROR, book not found (kps id): {0} \r\n", archive.BookKpsID);
                    continue;
                }

                List<int> modifiedBooksWithCatalogs;
                bookDictionary.TryGetValue(archive.Year, out modifiedBooksWithCatalogs);

                if (ProcessFromDtoModified(uow, archive.ArchiveToBookDto(), book, archive.Year, modifiedBooksWithCatalogs, sb))
                {
                    UpdateKPS(book.ID, book.BookKpsID, "Full", sb);
                }
                sb.AppendFormat("-----------\r\n");
            }            

            return sb.ToString();
        }
        private static bool ProcessFromDtoModified(IUnitOfWork uow, BookDTO bookDto, Book book, int year, IList<int> booksWithCatalogs, StringBuilder sb)
        {
            bool ok = false;
            /** 
                Find the current price of the book
                check if that price has changed
            */
            var bookPrice = book.BookPrices.FirstOrDefault(x => x.Status == enBookPriceStatus.Active && x.Year == year);
            /**
                check the provided data for business logic errors
            */            
            var hasCatalogsInYear = booksWithCatalogs.Contains(book.ID);

            var error = CheckBookBusinessLogic(book, bookDto, bookPrice);
            if (error.HasValue)
            {
                string errorTxt = string.Format("BL error -{2}- for book : {0} {1} (Year: {3}) \r\n",
                    (int) book.BookKpsID, book.Title, error.GetLabel(), year);

                BusinessLogicErrorsKP logicError = new BusinessLogicErrorsKP();
                logicError.ErrorEntityType = enErrorEntityType.Book;
                logicError.ErrorCode = error.Value;
                logicError.EntityID = book.BookKpsID;
                logicError.Description = errorTxt;
                logicError.CreatedAt = DateTime.Now;
                uow.MarkAsNew(logicError);

                sb.AppendFormat(errorTxt);
            }
            else
            {
                bool bookPriceHasChanged = false;
                var newPrice = bookDto.price.HasValue ? bookDto.price : bookDto.suggestedPrice;
                /** 
                    if there is no bookPrice for the book then create it
                */
                if (bookPrice == null)
                {
                    CreateBookPrice(year, bookDto, book, uow);
                }
                else if (year > PhaseHelper.MaxYear())
                {
                    bookPrice.Price = newPrice.Value;                                                            
                    bookPrice.IsChecked = bookDto.checkedPrice;
                    bookPrice.UpdatedAt = DateTime.Now;
                    bookPrice.UpdatedBy = "sysadmin";                    
                    bookPrice.Fek = bookDto.fek;
                }
                else
                {                    
                    //Check if Change price Already Exists                 
                    if (!new BookPriceChangeRepository(uow).AlreadyExists(book.ID, year, bookDto.price,
                        bookDto.suggestedPrice, bookDto.checkedPrice))
                    {
                        if (bookPrice.IsNew || bookPrice.Price != newPrice || (bookPrice.IsChecked && !bookDto.checkedPrice))
                        {
                            /**
                                If the book does not have catalogs from Phase13 and beyond and it is not "price locked" in any way, then don't mark it,
                                do not create a bookPriceChange entry, just change its price.
                            */
                            if (!hasCatalogsInYear && book.HasPendingPriceVerification == false &&
                                book.HasUnexpectedPriceChange == false)
                            {
                                sb.AppendFormat(
                                    " Book: {0} has no catalogs for year " + year +
                                    ". Its price will be automatically updated... \r\n", book.ID);
                                bookPrice.Price = (decimal)newPrice;
                                bookPrice.IsChecked = bookDto.checkedPrice;

                                //Log the book price change in the database                        
                                BookPriceChange bookPriceChange = book.BookPriceChangeFromDto(bookDto, year);
                                bookPriceChange.Approved = true;
                                uow.MarkAsNew(bookPriceChange);
                            }
                            else
                            {
                                bookPriceHasChanged = true;
                                sb.AppendFormat("BookPrice change spotted for Book: {0} \r\n", book.ID);
                            }
                        }

                        /**
                            If the price has been changed, log the bookPriceChange and then, if needed,  mark the book and its catalogs as unexpected change
                        */
                        if (bookPriceHasChanged || (book.PendingCommitteePriceVerification == true && bookDto.checkedPrice))
                        {
                            BookPriceChange bookPriceChange = book.BookPriceChangeFromDto(bookDto, year);
                            uow.MarkAsNew(bookPriceChange);

                            // if the book is not marked as pending price verification by committee, then do mark it as unexpected Price Change
                            if (!book.PendingCommitteePriceVerification.HasValue ||
                                book.PendingCommitteePriceVerification == false)
                            {
                                //Toggle Unexpected Price change                                 
                                var currentCatalogsPhase = new PhaseRepository(uow).GetCurrentCatalogsPhase();
                                book.HasUnexpectedPriceChangePhaseID = currentCatalogsPhase.ID;

                                List<int> phaseIds = PhaseHelper.GetPhasesOfYear(year).Select(x => x.ID).ToList();

                                List<Catalog> catalogs = new CatalogRepository(uow).FindByBookID(bookPrice.BookID).Where(x => phaseIds.Contains(x.PhaseID.Value)).ToList();
                                catalogs.ForEach(c => c.HasUnexpectedPriceChange = true);
                                
                                bookPriceChange.IsUnexpected = true;
                            }
                        }
                    }                                        
                }
                ok = true;
            }
            uow.Commit();

            return ok;
        }
        //---------------------------------------------------------
        public static List<int> GetModifiedBooksIDsWithCatalogsInYear(IUnitOfWork uow, List<Book> books, int year)
        {
            ICatalogRepository catalogRepository = new CatalogRepository(uow);            
            List<int> modifiedBooksWithCatalogs = catalogRepository.GetBooksWithCatalogsInYear(books.Select(x => x.ID), year);
            return modifiedBooksWithCatalogs.Distinct().ToList();
        }
        public static void UpdateKPS(int bookId, int kpsId, string bsaStatus, StringBuilder sb)
        {
            if (Config.EnableKPSUpdate)
            {
                UpdateBookStatusRequest updateRequest = new UpdateBookStatusRequest();
                updateRequest.bsaId = bookId;
                updateRequest.id = kpsId;
                updateRequest.bsaStatus = bsaStatus;
                BookServicesClients.UpdateBookStatus(updateRequest);
                sb.AppendFormat( (bsaStatus =="Full"?"new_book:":"existing_book") + " updated in KPS: osyID: {0}, kpsID: {1} \r\n", bookId, kpsId);
            }
        }
        //------------------------------------------------------------------------------------------------------------
        public static string GenerateBookChangesReport(List<BookDTO> booksDtos)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Καταγράφηκαν αλλαγές από το ΚΠΣ για τα παρακάτω βιβλία\r\n");
            sb.Append("\t KpsID \t Τίτλος\r\n");

            foreach (BookDTO dto in booksDtos)
            {
                sb.AppendFormat("\t {0} \t {1}\r\n", dto.id, dto.title);
            }

            return sb.ToString();
        }
        public static enErrorCode? CheckBookBusinessLogic(Book book, BookDTO dto, BookPrice bookPrice = null)
        {
            return BookHelper.CheckBookServiceBusinessLogic(book, dto, bookPrice);
        }
        public static BookPrice CreateBookPrice(int year, BookDTO dto, Book book, IUnitOfWork uow)
        {
            var newBookPrice = new BookPrice();
            if ((dto.price.HasValue || dto.suggestedPrice > 0) && book.BookType == enBookType.Regular)
            {
                newBookPrice.Price = dto.price ?? dto.suggestedPrice;
                newBookPrice.Book = book;
                newBookPrice.Year = year;
                newBookPrice.IsChecked = dto.checkedPrice;
                newBookPrice.CreatedAt = DateTime.Now;
                newBookPrice.CreatedBy = "sysadmin";
                newBookPrice.Status = enBookPriceStatus.Active;
                newBookPrice.Fek = dto.fek;
                uow.MarkAsNew(newBookPrice);
            }
            return newBookPrice;
        }
        public static List<Book> GetBooks(IUnitOfWork uow, IRepositoryFactory repFactory, IEnumerable<int> modifiedBookKpsIDs)
        {
            IBookRepository bookRepository = repFactory != null ? repFactory.GetRepositoryInstance<Book, IBookRepository>(uow) : new BookRepository(uow);

            Criteria<Book> bookCriteria = new Criteria<Book>();
            int bookCount = 0;
            bookCriteria.Expression = bookCriteria.Expression.InMultiSet(x => x.BookKpsID, modifiedBookKpsIDs);
            bookCriteria.Include(x => x.BookPrices);
            bookCriteria.UsePaging = false;
            var books = bookRepository.FindWithCriteria(bookCriteria, out bookCount);
            return books;
        }
    }
}