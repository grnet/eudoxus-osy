using EudoxusOsy.BusinessModel;
using EudoxusOsy.Utils;
using Imis.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.Portal
{
    public class UpdateBooksHelper
    {
        /// <summary>
        /// 1. Call the GetModifiedBooks KPS Service
        /// 2. Update the Book data on the Book table and the BookPriceChange table (if needed)
        /// 3. Call the UpdateBookStatus KPS Service for each Book that was processed
        /// 4. Report the changes to the project team via email
        /// </summary>
        public static async void UpdateModifiedBooksFromKPS()
        {
            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                LogHelper.LogMessage<UpdateBooksHelper>("updateModifiedBooks started");

                // 1. Call the GetModifiedBooks KPS Service

                var currenCatalogsPhase = new PhaseRepository(uow).GetCurrentCatalogsPhase();
                List<BookDTO> UpdatedPriceBooks = new List<BookDTO>();
                StringBuilder sb = new StringBuilder();
                PhaseRepository phaseRepository = new PhaseRepository(uow);

                #region [ TestData ]
                //TEST
                //booksResponse.numResults = 1;
                //booksResponse.books = new List<BookDTO>();
                //BookDTO dto1 = new BookDTO();
                //dto1.idKPS = 6934;
                //dto1.title = "TEST";
                //dto1.subtitle = "TEST sub";
                //dto1.authors = "testakis";
                //dto1.publisher = "testakios";
                //dto1.pages = 5;
                //dto1.isbn = "isbn";
                //dto1.supplierCode = 1;
                //dto1.active = true;
                //dto1.suggestedPrice = 50;
                //dto1.decisionNumber = "1";
                //dto1.priceComments = "my comm";
                //dto1.checkedPrice = true;

                //booksResponse.books.Add(dto1);
                //-----------

                #endregion
                GetBooksResponse booksResponse = BookServicesClients.GetModifiedBooks();
                UpdatedPriceBooks = booksResponse.books;

                var result = await DoProcessModifiedBooksWithDirectKPSUpdate(phaseRepository, uow, booksResponse);

                // 4. Report the changes to the project team via email
                var reportEmail = EmailFactory.GetServiceBookChangesReport("Ενημέρωση τιμής από ΚΠΣ", GenerateBookChangesReport(UpdatedPriceBooks));
                uow.MarkAsNew(reportEmail);
                uow.Commit();
                EmailQueueWorker.Current.AddEmailDispatchToQueue(reportEmail.ID);
            }
        }

        public static async Task<string> DoProcessModifiedBooksWithDirectKPSUpdate(IPhaseRepository phaseRepository, IUnitOfWork uow, IGetBookResponse booksResponse)
        {
            //var progressIndicator = new Progress<int>(ReportProgress);
            StringBuilder sb = new StringBuilder();
            sb.Append("get modified books started \r\n");
            // 1. Call the GetModifiedBooks KPS Service
            var currentCatalogsPhase = phaseRepository.GetCurrentCatalogsPhase();

            if (booksResponse != null && booksResponse.books != null && booksResponse.books.Count > 0)
            {
                int i = 0;
                string stringResult = string.Empty;
                while (i * 5000 < booksResponse.numResults)
                {
                    stringResult += await ProcessModifiedBooksResponseWithDirectUpdate(sb, booksResponse, currentCatalogsPhase, uow, (i + 1) * 5000);
                    i++;
                }
                return stringResult;
            }
            else
            {
                sb.Append("No Modified books found in KPS");
            }

            return sb.ToString();
        }

        //public static void UpdateBooksInKPS(List<UpdateBookStatusRequest> booksToUpdate, StringBuilder sb)
        //{
        //    if (Config.EnableKPSUpdate)
        //    {
        //        foreach (var bookToUpdateRequest in booksToUpdate)
        //        {
        //            BookServicesClients.UpdateBookStatus(bookToUpdateRequest);
        //            sb.AppendFormat("book update in KPS: {0} \r\n", bookToUpdateRequest.bsaId);
        //        }
        //    }
        //}


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

        /// <summary>
        /// 1. Call the GetNewBooks KPS Service
        /// 2. Insert the Book data on the Book table and create a BookPriceChange entry (not a new BookPrice yet)
        /// 3. Call the UpdateBookStatus KPS Service for each Book that was processed
        /// 4. Report the newly inserted books to the project team via email
        /// </summary>
        public static async void GetNewBooksFromKPS()
        {
            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                //TODO: Update the book prices for both phases, the current and the current-1
                var currentCatalogsPhase = new PhaseRepository(uow).GetCurrentCatalogsPhase();
                #region [Test Data]
                //TEST
                //booksResponse.numResults = 1;
                //booksResponse.books = new List<BookDTO>();
                //BookDTO dto1 = new BookDTO();
                //dto1.idKPS = 555111;
                //dto1.title = "TEST";
                //dto1.subtitle = "TEST sub";
                //dto1.authors = "testakis";
                //dto1.publisher = "testakios";
                //dto1.pages = 5;
                //dto1.isbn = "isbn";
                //dto1.supplierCode = 1;
                //dto1.active = true;
                //dto1.suggestedPrice = 50;
                //dto1.decisionNumber = "1";
                //dto1.priceComments = "my comm";
                //dto1.checkedPrice = true;

                //booksResponse.books.Add(dto1);
                //-----------

                #endregion

                StringBuilder sb = new StringBuilder();

                LogHelper.LogMessage<UpdateBooksHelper>("updateNewBooks started");
                List<Book> books = new List<Book>();
                List<Book> existingBooks = new List<Book>();

                var newBooksResponse = BookServicesClients.GetNewBooks();

                var result = await Task.Run(() => { ProcessNewBooksResponse(uow, currentCatalogsPhase, sb, books, existingBooks, newBooksResponse); return sb.ToString(); }).ConfigureAwait(false);

                //email the differences
                var reportEmail = EmailFactory.GetServiceBookChangesReport("Νέα βιβλία από ΚΠΣ", GenerateNewBooksReport(books) + GenerateExistingAsNewBooksReport(existingBooks));
                uow.MarkAsNew(reportEmail);
                uow.Commit();
                EmailQueueWorker.Current.AddEmailDispatchToQueue(reportEmail.ID);
            }
        }

        public static void ProcessNewBooksResponse(IUnitOfWork uow, Phase currentCatalogsPhase, StringBuilder sb, List<Book> books, List<Book> existingBooks, IGetBookResponse newBooksResponse)
        {
            /**
                1. Call the KPS GetNewBooksService
            */

            BookRepository bookRepository = new BookRepository(uow);
            var newBookIDs = newBooksResponse.books.Select(x => x.id);

            /**
                2.Insert the bookData to the table
            */
            books = new List<Book>();
            Criteria<Book> bookCriteria = new Criteria<Book>();

            if (newBooksResponse != null && newBooksResponse.books != null && newBooksResponse.books.Count > 0)
            {
                sb.AppendFormat("booksResponse: {0} \r\n", newBooksResponse);
                sb.AppendFormat("booksResponse results count : {0} \r\n", newBooksResponse.numResults);

                /**
                    If the book already exists in OSY, we update its KPS status as modified, so that the GetModified books
                    algorithm will take care of the price changes
                */
                newBooksResponse.books.ForEach(x =>
                {
                    Book book = new BookRepository(uow).FindByBookKpsID(x.id).FirstOrDefault();
                    sb.AppendFormat("looking for book : {0} {1} \r\n", (int)x.id, x.title);

                    var error = UpdateBooksHelper.CheckBookBusinessLogic(book, x);
                    if (error.HasValue)
                    {
                        BusinessLogicErrorsKP logicError = new BusinessLogicErrorsKP();
                        logicError.ErrorEntityType = enErrorEntityType.Book;
                        logicError.ErrorCode = error.Value;
                        logicError.EntityID = x.id;
                        logicError.Description = error.GetLabel();
                        logicError.CreatedAt = DateTime.Now;
                        uow.MarkAsNew(logicError);
                        sb.AppendFormat("Business Logic error for book : {0} {1} \r\n", (int)x.id, x.title);
                    }
                    else
                    {
                        if (book == null)
                        {
                            Book newBook = x.ToOsyBook();
                            uow.MarkAsNew(newBook);
                            var bookPriceToInsert = UpdateBooksHelper.CreateBookPrice(currentCatalogsPhase, x, newBook, uow);
                            sb.AppendFormat("book created in OSY : Kpsid: {0} {1} \r\n", (int)x.id, x.title);

                            books.Add(newBook);
                        }
                        else
                        {
                            book.UpdateBookFromDto(x);
                            sb.AppendFormat("book: {0} {1} ALREADY EXISTS, Data updated, but not the price \r\n", (int)x.id, x.title);
                        }
                    }
                });
                uow.Commit();
            }

            /**
                3.Call the UpdateBookStatus Service for each book
            */
            if (Config.EnableKPSUpdate)
            {
                foreach (var newBook in books)
                {
                    UpdateBookStatusRequest updateRequest = new UpdateBookStatusRequest();
                    updateRequest.bsaId = newBook.ID;
                    updateRequest.id = newBook.BookKpsID;
                    updateRequest.bsaStatus = "Full";
                    BookServicesClients.UpdateBookStatus(updateRequest);
                    sb.AppendFormat("book update in KPS: osyID: {0}, kpsID: {1} \r\n", newBook.ID, newBook.BookKpsID);
                }

                foreach (var existingBook in existingBooks)
                {
                    UpdateBookStatusRequest updateRequest = new UpdateBookStatusRequest();
                    updateRequest.bsaId = existingBook.ID;
                    updateRequest.id = existingBook.BookKpsID;
                    updateRequest.bsaStatus = "Modified";
                    BookServicesClients.UpdateBookStatus(updateRequest);
                    sb.AppendFormat("book update in KPS: osyID: {0}, kpsID: {1} \r\n", existingBook.ID, existingBook.BookKpsID);
                }
            }
        }

        public static string GenerateNewBooksReport(List<Book> books)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Νέα βιβλία από ΚΠΣ\r\n");
            sb.Append("\t KpsID \t Τίτλος\r\n");

            foreach (Book book in books)
            {
                sb.AppendFormat("\t {0} \t {1}\r\n", book.BookKpsID, book.Title);
            }

            return sb.ToString();
        }

        public static string GenerateExistingAsNewBooksReport(List<Book> books)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Υπάρχοντα βιβλία που ήρθαν σαν νέα από το ΚΠΣ\r\n");
            sb.Append("\t KpsID \t Τίτλος\r\n");

            foreach (Book book in books)
            {
                sb.AppendFormat("\t {0} \t {1}\r\n", book.BookKpsID, book.Title);
            }

            return sb.ToString();
        }


        public static BookPrice CreateBookPrice(Phase currentPhase, BookDTO dto, Book book, IUnitOfWork uow)
        {
            var newBookPrice = new BookPrice();
            if ((dto.price.HasValue || dto.suggestedPrice > 0) && book.BookType == enBookType.Regular)
            {
                newBookPrice.Price = dto.price ?? dto.suggestedPrice;
                newBookPrice.Book = book;
                newBookPrice.Year = currentPhase.Year;
                newBookPrice.IsChecked = dto.checkedPrice;
                newBookPrice.CreatedAt = DateTime.Now;
                newBookPrice.CreatedBy = "sysadmin";
                newBookPrice.Status = enBookPriceStatus.Active;
                uow.MarkAsNew(newBookPrice);
            }
            return newBookPrice;
        }

        public static enErrorCode? CheckBookBusinessLogic(Book book, BookDTO dto, BookPrice bookPrice = null)
        {
            return BookHelper.CheckBookServiceBusinessLogic(book, dto, bookPrice);
        }

        public async static Task<string> ProcessModifiedBooksResponseWithDirectUpdate(StringBuilder sb, IGetBookResponse booksResponse, Phase currentCatalogsPhase, IUnitOfWork uow, int? limit = null, IRepositoryFactory repFactory = null)
        {
            sb.AppendFormat("booksResponse results count : {0} \r\n", booksResponse.numResults);

            var limitedBooks = booksResponse.books;
            if (limit.HasValue)
            {
                limitedBooks = limitedBooks.Skip(limit.Value - 5000).Take(5000).ToList();
            }

            var modifiedBookKpsIDs = limitedBooks.Select(x => x.id);

            /**
             Get the books and the book prices to reduce stress of the DB
            */
            List<Book> books = GetBooks(uow, repFactory, modifiedBookKpsIDs);

            IList<int> modifiedBooksWithCatalogsAfterPhase13IDs = GetModifiedBooksIDsWithCatalogsAfterPhase13(uow, repFactory, books);

            int i = 0;
            //2. Update the Book data on the Book table and the BookPriceChange table (if needed)
            foreach (BookDTO dto in limitedBooks)
            {
                i++;
                var bookPriceHasChanged = false;
                sb.AppendFormat("looking for book : {0} {1} \r\n", (int)dto.id, dto.title);
                var book = books.FirstOrDefault(x => x.BookKpsID == dto.id);

                if (book != null)
                {
                    sb.AppendFormat("processing book : {0} \r\n", book.ID);
                }
                else
                {
                    sb.AppendFormat("ERROR, book not found (kps id): {0} \r\n", dto.id);
                    continue;
                }

                if (book.ID != 0)
                {
                    /**
                        Update the book data with the received values (all but the price)
                    */
                    ProcessModifiedBookItem(sb, currentCatalogsPhase, uow, modifiedBooksWithCatalogsAfterPhase13IDs, dto, ref bookPriceHasChanged, ref book);
                    UpdateInKpsFull(sb, dto, book);
                }
                else
                {
                    sb.AppendFormat("Book not found : {0} \r\n", book.ID);
                }
            }

            uow.Commit();
            //return new Task<string>(()=> sb.ToString());
            return await Task.Run(() => sb.ToString());
        }

        private static void ProcessModifiedBookItem(StringBuilder sb, Phase currentCatalogsPhase, IUnitOfWork uow, IList<int> modifiedBooksWithCatalogsAfterPhase13IDs, BookDTO dto, ref bool bookPriceHasChanged, ref Book book)
        {
            book = book.UpdateBookFromDto(dto);

            /** 
                Find the current price of the book
                check if that price has changed
            */
            // TODO: We should update both Book prices for current and for current - 1 period. (when the phase year changes)
            var bookPrice = book.BookPrices.FirstOrDefault(x => x.Status == enBookPriceStatus.Active && x.Year == currentCatalogsPhase.Year);

            /**
                check the provided data for business logic errors
            */
            var hasCatalogsfromPhase13AndBeyond = modifiedBooksWithCatalogsAfterPhase13IDs.Contains(book.ID);

            var error = CheckBookBusinessLogic(book, dto, bookPrice);
            if (error.HasValue)
            {
                BusinessLogicErrorsKP logicError = new BusinessLogicErrorsKP();
                logicError.ErrorEntityType = enErrorEntityType.Book;
                logicError.ErrorCode = error.Value;
                logicError.EntityID = dto.id;
                logicError.Description = error.GetLabel();
                logicError.CreatedAt = DateTime.Now;
                uow.MarkAsNew(logicError);
            }
            else
            {
                var newPrice = dto.price.HasValue ? dto.price : dto.suggestedPrice;

                /** 
                    if there is no bookPrice for the book then create it
                */
                if (bookPrice == null)
                {
                    bookPrice = CreateBookPrice(currentCatalogsPhase, dto, book, uow);
                }

                if (bookPrice != null && (bookPrice.IsNew || bookPrice.Price != newPrice || (bookPrice.IsChecked && !dto.checkedPrice)))
                {
                    /**
                        If the book does not have catalogs from Phase13 and beyond and it is not "price locked" in any way, then don't mark it,
                        do not create a bookPriceChange entry, just change its price.
                    */
                    if (!hasCatalogsfromPhase13AndBeyond && book.HasPendingPriceVerification == false && book.HasUnexpectedPriceChange == false)
                    {
                        sb.AppendFormat(" Book: {0} has no catalogs for phases >= 13. Its price will be automatically updated... \r\n", book.ID);
                        bookPrice.Price = (decimal)newPrice;
                        bookPrice.IsChecked = dto.checkedPrice;
                        //TODO: Update also the bookPrice of the current period and the current - 1 period if the year has changed!!!

                        //Log the book price change in the database
                        BookPriceChange bookPriceChange = book.BookPriceChangeFromDto(dto);
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
                if (bookPriceHasChanged || (book.PendingCommitteePriceVerification == true && dto.checkedPrice))
                {
                    BookPriceChange bookPriceChange = book.BookPriceChangeFromDto(dto);
                    uow.MarkAsNew(bookPriceChange);

                    // if the book is not marked as pending price verification by committee, then do mark it as unexpected Price Change
                    if (!book.PendingCommitteePriceVerification.HasValue || book.PendingCommitteePriceVerification == false)
                    {
                        //Toggle Unexpected Price change 
                        BookHelper.ToggleUnexpectedPriceChange(uow, book.ID, true);
                        bookPriceChange.IsUnexpected = true;
                    }
                }
                uow.Commit();
            }
        }

        private static void UpdateInKpsFull(StringBuilder sb, BookDTO dto, Book book)
        {
            // 3. Call the UpdateBookStatus KPS Service for each Book that was processed
            UpdateBookStatusRequest bookStatusRequest = new UpdateBookStatusRequest();
            bookStatusRequest.id = dto.id;
            bookStatusRequest.bsaId = book.ID;
            bookStatusRequest.bsaStatus = "Full";

            BookServicesClients.UpdateBookStatus(bookStatusRequest);
            sb.AppendFormat("book update in KPS: osyID: {0}, kpsID: {1} \r\n", book.ID, book.BookKpsID);
        }

        private static IList<int> GetModifiedBooksIDsWithCatalogsAfterPhase13(IUnitOfWork uow, IRepositoryFactory repFactory, List<Book> books)
        {
            ICatalogRepository catalogRepository = repFactory != null ? repFactory.GetRepositoryInstance<Catalog, CatalogRepository>(uow) : new CatalogRepository(uow);
            var modifiedBooksWithCatalogsAfterPhase13IDs = catalogRepository.GetBooksWithCatalogsInPhase13AndBeyondFromBooks(books.Select(x => x.ID));
            return modifiedBooksWithCatalogsAfterPhase13IDs;
        }

        private static List<Book> GetBooks(IUnitOfWork uow, IRepositoryFactory repFactory, IEnumerable<int> modifiedBookKpsIDs)
        {
            IBookRepository bookRepository = repFactory != null ? repFactory.GetRepositoryInstance<Book, BookRepository>(uow) : new BookRepository(uow);

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