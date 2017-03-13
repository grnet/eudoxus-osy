using EudoxusOsy.BusinessModel;
using EudoxusOsy.Utils;
using Imis.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public static void UpdateModifiedBooksFromKPS()
        {
            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                LogHelper.LogMessage<UpdateBooksHelper>("updateModifiedBooks started");

                // 1. Call the GetModifiedBooks KPS Service
                GetBooksResponse booksResponse = BookServicesClients.GetModifiedBooks();
                var currentPhase = new PhaseRepository(uow).GetCurrentPhase();
                List<BookDTO> UpdatedPriceBooks = new List<BookDTO>();

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

                if (booksResponse != null && booksResponse.books != null && booksResponse.books.Count > 0)
                {
                    BookRepository bookRepository = new BookRepository(uow);

                    // 2. Update the Book data on the Book table and the BookPriceChange table (if needed)
                    foreach (BookDTO dto in booksResponse.books)
                    {
                        Book book = bookRepository.FindByBookKpsID((int)dto.id).FirstOrDefault();

                        //Error management ? 
                        if (book != null)
                        {
                            book = book.UpdateBookFromDto(dto);
                            uow.Commit();

                            var bookPrice = new BookPriceRepository(uow).FindByBookIDAndYear(book.ID, currentPhase.Year);
                            var bookPriceHasChanged = false;
                            if (bookPrice != null && bookPrice.Price != (dto.price.HasValue ? dto.price : dto.suggestedPrice))
                            {
                                bookPriceHasChanged = true;
                            }

                            if (bookPriceHasChanged)
                            {
                                var bookPriceChange = book.BookPriceChangeFromDto(dto);
                                uow.MarkAsNew(bookPriceChange);

                                // if the book is not marked as pending price verification by committee, then do mark it as unexpected Price Change
                                if (!book.PendingCommitteePriceVerification.HasValue || book.PendingCommitteePriceVerification == false)
                                {
                                    //Toggle Unexpected Price change 
                                    BookHelper.ToggleUnexpectedPriceChange(uow, book.ID, true);
                                }

                                UpdatedPriceBooks.Add(dto);
                                uow.Commit();
                            }

                            // 3. Call the UpdateBookStatus KPS Service for each Book that was processed
                            UpdateBookStatusRequest bookStatusRequest = new UpdateBookStatusRequest();
                            bookStatusRequest.id = dto.id;
                            bookStatusRequest.bsaId = book.ID;
                            bookStatusRequest.bsaStatus = "Full";
                            //Refactoring for TEST cases!!!
                            BookServicesClients.UpdateBookStatus(bookStatusRequest);
                        }
                        else
                        {
                            LogHelper.LogMessage(string.Format("Book: {0} NOT FOUND", dto.id), typeof(UpdateBooksHelper));
                        }
                    }

                    // 4. Report the changes to the project team via email
                    var reportEmail = EmailFactory.GetServiceBookChangesReport("Ενημέρωση τιμής από ΚΠΣ", GenerateBookChangesReport(UpdatedPriceBooks));
                    uow.MarkAsNew(reportEmail);
                    uow.Commit();
                    EmailQueueWorker.Current.AddEmailDispatchToQueue(reportEmail.ID);
                }
            }
        }

        public static string GenerateBookChangesReport(List<BookDTO> booksDtos)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Αλλαγές τιμών από ΚΠΣ για τα παρακάτω βιβλία\r\n");
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
        public static void GetNewBooksFromKPS()
        {
            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                GetBooksResponse booksResponse = new GetBooksResponse();

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

                LogHelper.LogMessage<UpdateBooksHelper>("updateNewBooks started");

                /**
                    1. Call the KPS GetNewBooksService
                */
                var newBooksResponse = BookServicesClients.GetNewBooks();

                /**
                    2.Insert the bookData to the table
                */
                List<Book> books = new List<Book>();
                List<Book> existingBooks = new List<Book>();
                if (newBooksResponse != null && newBooksResponse.numResults > 0)
                {
                    newBooksResponse.books.ForEach(x =>
                    {
                        Book book = new BookRepository(uow).FindByBookKpsID((int)x.id).FirstOrDefault();
                        if (book == null)
                        {
                            Book newBook = x.ToOsyBook();

                            uow.MarkAsNew(newBook);
                            books.Add(newBook);
                        }
                        else
                        {
                            book.UpdateBookFromDto(x);
                            existingBooks.Add(book);
                        }
                    });
                    uow.Commit();
                }

                /**
                    3.Call the UpdateBookStatus Service for each book
                */

                foreach (var newBook in books)
                {
                    UpdateBookStatusRequest updateRequest = new UpdateBookStatusRequest();
                    updateRequest.bsaId = newBook.ID;
                    updateRequest.id = newBook.BookKpsID;
                    updateRequest.bsaStatus = "Full";
                    BookServicesClients.UpdateBookStatus(updateRequest);
                }

                foreach (var existingBook in existingBooks)
                {
                    UpdateBookStatusRequest updateRequest = new UpdateBookStatusRequest();
                    updateRequest.bsaId = existingBook.ID;
                    updateRequest.id = existingBook.BookKpsID;
                    updateRequest.bsaStatus = "Modified";
                    BookServicesClients.UpdateBookStatus(updateRequest);
                }

                //email the differences
                var reportEmail = EmailFactory.GetServiceBookChangesReport("Νέα βιβλία από ΚΠΣ", GenerateNewBooksReport(books));
                uow.MarkAsNew(reportEmail);
                uow.Commit();
                EmailQueueWorker.Current.AddEmailDispatchToQueue(reportEmail.ID);
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
    }
}