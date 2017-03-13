using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.Portal.Controls;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Utils;

namespace EudoxusOsy.Portal.Secure.Ministry
{
    public partial class BookKpsServices : BaseEntityPortalPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpdateBookStatus_Click(object sender, EventArgs e)
        {
            var id = txtUpdateBookStatusID.GetInteger();
            var book = new BookRepository(UnitOfWork).Load(id.Value);
            UpdateBookStatusRequest req = new UpdateBookStatusRequest();
            req.bsaId = id.Value;
            req.id = book.BookKpsID;
            req.bsaStatus = "FULL";
            BookServicesClients.UpdateBookStatus(req);
        }

        protected void btnGetModifiedBooks_Click(object sender, EventArgs e)
        {
            try
            {
                txtResults.Text = "";
                txtResults.Text += "get modified books started \r\n";
                // 1. Call the GetModifiedBooks KPS Service
                GetBooksResponse booksResponse = BookServicesClients.GetModifiedBooks();
                var currentPhase = new PhaseRepository(UnitOfWork).GetCurrentPhase();

                txtResults.Text += string.Format("booksResponse: {0} \r\n", booksResponse);
                txtResults.Text += string.Format("booksResponse results count : {0} \r\n", booksResponse.numResults);

                if (booksResponse != null && booksResponse.books != null && booksResponse.books.Count > 0)
                {
                    BookRepository bookRepository = new BookRepository(UnitOfWork);

                    txtResults.Text += string.Format("Book Count: {0} \r\n", booksResponse.books.Count);
                    txtResults.Text += string.Format("books: {0} \r\n", booksResponse.books);

                     //2. Update the Book data on the Book table and the BookPriceChange table (if needed)
                    foreach (BookDTO dto in booksResponse.books)
                    {
                        var bookPriceHasChanged = false;
                        Book book = new Book();
                        txtResults.Text += string.Format("looking for book : {0} {1} \r\n", (int)dto.id, dto.title);
                        book = bookRepository.FindByBookKpsID((int)dto.id).FirstOrDefault();

                        if (book != null)
                        {
                            txtResults.Text += string.Format("processing book : {0} \r\n", book.ID);
                        }
                        else
                        {
                            txtResults.Text += string.Format("ERROR, book not found (kps id): {0} \r\n", dto.id);
                        }

                        //Error management ? 
                        if (book.ID != 0)
                        {
                            book = book.UpdateBookFromDto(dto);
                            UnitOfWork.Commit();
                        }

                        //Check if price has changed
                        //find the current price of the book
                        //check if that price has changed
                        var bookPrice = new BookPriceRepository(UnitOfWork).FindByBookIDAndYear(book.ID, currentPhase.Year);
                        if (bookPrice != null &&  bookPrice.Price != (dto.price.HasValue? dto.price: dto.suggestedPrice))
                        {
                            bookPriceHasChanged = true;
                            txtResults.Text += string.Format("BookPrice change spotted for Book: {0} \r\n", book.ID);
                        }
                        else if(bookPrice == null)
                        {
                            txtResults.Text += string.Format("could not find BookPrice for Book: {0} \r\n", book.ID);
                        }

                        if (bookPriceHasChanged)
                        {
                            BookPriceChange bookPriceChange = book.BookPriceChangeFromDto(dto);
                            UnitOfWork.MarkAsNew(bookPriceChange);

                            // if the book is not marked as pending price verification by committee, then do mark it as unexpected Price Change
                            if(!book.PendingCommitteePriceVerification.HasValue || book.PendingCommitteePriceVerification == false)
                            {
                                //Toggle Unexpected Price change 
                                BookHelper.ToggleUnexpectedPriceChange(UnitOfWork, book.ID, true);
                            }

                            UnitOfWork.Commit();
                        }

                        // 3. Call the UpdateBookStatus KPS Service for each Book that was processed
                        if (book.ID != 0)
                        {
                            UpdateBookStatusRequest bookStatusRequest = new UpdateBookStatusRequest();
                            bookStatusRequest.id = dto.id;
                            bookStatusRequest.bsaId = book.ID;
                            bookStatusRequest.bsaStatus = "Full";
                            //Refactoring for TEST cases!!!
                            BookServicesClients.UpdateBookStatus(bookStatusRequest);
                            txtResults.Text += string.Format("book update in KPS: {0} \r\n", book.ID);
                        }
                        else
                        {
                            txtResults.Text += string.Format("Won't update book in KPS: {0} \r\n", book.ID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                txtResults.Text += string.Format("exception: {0} \r\n", ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                Notify(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }

            Notify("Η ενημέρωση των βιβλίων ολοκληρώθηκε επιτυχώς");
        }

        protected void btnGetNewBooks_Click(object sender, EventArgs e)
        {
            try
            {
                txtResults.Text = "";
                LogHelper.LogMessage<UpdateBooksHelper>("updateNewBooks started");

                /**
                    1. Call the KPS GetNewBooksService
                */
                var newBooksResponse = BookServicesClients.GetNewBooks();

                //TODO: handle the inactive - active status of a book if needed
                /**
                    2.Insert the bookData to the table
                */
                List<Book> books = new List<Book>();
                List<Book> existingBooks = new List<Book>();
                if (newBooksResponse != null && newBooksResponse.books != null && newBooksResponse.books.Count > 0)
                {
                    txtResults.Text += string.Format("booksResponse: {0} \r\n", newBooksResponse);
                    txtResults.Text += string.Format("booksResponse results count : {0} \r\n", newBooksResponse.numResults);

                    BookRepository bookRepository = new BookRepository(UnitOfWork);
                    newBooksResponse.books.ForEach(x =>
                    {
                        Book book = new Book();
                        txtResults.Text += string.Format("looking for book : {0} {1} \r\n", (int)x.id, x.title);
                        book = bookRepository.FindByBookKpsID((int)x.id).FirstOrDefault();

                        if (book == null)
                        {
                            Book newBook = x.ToOsyBook();

                            UnitOfWork.MarkAsNew(newBook);
                            books.Add(newBook);
                        }
                        else
                        {
                            book.UpdateBookFromDto(x);
                            existingBooks.Add(book);
                            txtResults.Text += string.Format("book: {0} {1} ALREADY EXISTS \r\n", (int)x.id, x.title);
                        }
                    });
                    UnitOfWork.Commit();
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
                    txtResults.Text += string.Format("book update in KPS (Full): {0} \r\n", newBook.ID);
                }

                foreach (var existingBook in existingBooks)
                {
                    UpdateBookStatusRequest updateRequest = new UpdateBookStatusRequest();
                    updateRequest.bsaId = existingBook.ID;
                    updateRequest.id = existingBook.BookKpsID;
                    updateRequest.bsaStatus = "Modified";
                    BookServicesClients.UpdateBookStatus(updateRequest);
                    txtResults.Text += string.Format("existing book update in KPS (Modified): {0} \r\n", existingBook.ID);
                }
            }
            catch (Exception ex)
            {
                Notify(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }

            Notify("Η εισαγωγή των βιβλίων ολοκληρώθηκε επιτυχώς");
        }

        protected void btnInitKpsNewBooks_Click(object sender, EventArgs e)
        {
            BookRepository bookRepository = new BookRepository(UnitOfWork);
            var newBooksResponse = BookServicesClients.GetNewBooks();
            List<Book> books = new List<Book>();
            List<Book> existingBooks = new List<Book>();
            var currentPhase = new PhaseRepository(UnitOfWork).GetCurrentPhase();

            txtResults.Text = "";
            txtResults.Text += "get new books Started \r\n";
            LogHelper.LogMessage<UpdateBooksHelper>("updateNewBooks started");

            if (newBooksResponse != null && newBooksResponse.books != null && newBooksResponse.books.Count > 0)
            {
                newBooksResponse.books.ForEach(x =>
                {
                    var bookPriceHasChanged = false;
                    var book = bookRepository.FindByBookKpsID((int)x.id).FirstOrDefault();

                    if (book == null)
                    {
                        Book newBook = x.ToOsyBook();

                        UnitOfWork.MarkAsNew(newBook);
                        books.Add(newBook);
                    }
                    else
                    {
                        txtResults.Text += string.Format("'new' book: {0} {1} ALREADY EXISTS \r\n", x.id, x.title);
                        book.UpdateBookFromDto(x);

                        //Check if price has changed
                        //find the current price of the book
                        //check if that price has changed
                        var bookPrice = new BookPriceRepository(UnitOfWork).FindByBookIDAndYear(book.ID, currentPhase.Year);
                        if (bookPrice != null && bookPrice.Price != (x.price.HasValue ? x.price : x.suggestedPrice))
                        {
                            bookPriceHasChanged = true;
                            txtResults.Text += string.Format("BookPrice change spotted for Book: {0} \r\n", book.ID);
                        }
                        else if (bookPrice == null)
                        {
                            txtResults.Text += string.Format("could not find BookPrice for Book: {0} \r\n", book.ID);
                        }

                        if (bookPriceHasChanged)
                        {
                            BookPriceChange bookPriceChange = book.BookPriceChangeFromDto(x);
                            UnitOfWork.MarkAsNew(bookPriceChange);

                            // if the book is not marked as pending price verification by committee, then do mark it as unexpected Price Change
                            if (!book.PendingCommitteePriceVerification.HasValue || book.PendingCommitteePriceVerification == false)
                            {
                                //Toggle Unexpected Price change 
                                BookHelper.ToggleUnexpectedPriceChange(UnitOfWork, book.ID, true);
                            }
                        }

                        books.Add(book);
                        txtResults.Text += string.Format("book: {0} {1} ALREADY EXISTS \r\n", x.id, x.title);
                    }
                });
                UnitOfWork.Commit();

                foreach (var newBook in books)
                {
                    UpdateBookStatusRequest updateRequest = new UpdateBookStatusRequest();
                    updateRequest.bsaId = newBook.ID;
                    updateRequest.id = newBook.BookKpsID;
                    updateRequest.bsaStatus = "Full";
                    BookServicesClients.UpdateBookStatus(updateRequest);
                    txtResults.Text += string.Format("book update in KPS: {0} \r\n", newBook.ID);
                }
            }
        }
    }
}
