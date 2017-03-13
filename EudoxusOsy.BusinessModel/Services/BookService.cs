using EudoxusOsy.Services.Models;
using Imis.Domain;
using System.Linq;

namespace EudoxusOsy.BusinessModel.Services
{
    public class BookService
    {
        protected IUnitOfWork UnitOfWork { get; private set; }

        public BookService(IUnitOfWork uow)
        {
            UnitOfWork = uow;
        }

        public void MapFromDto(KpsBookDto bookDto)
        {
            Book book = new BookRepository(UnitOfWork).FindByBookKpsID(bookDto.idKPS).FirstOrDefault();

            if (book == null)
            {
                book = new Book();
                UnitOfWork.MarkAsNew(book);
            }

            book.BookKpsID = bookDto.idKPS;

            string kindbook = bookDto.kindbook;

            switch (kindbook.ToLower())
            {
                case "selfpublished":
                    book.BookType = enBookType.SelfPublished;
                    break;
                case "regular":
                    book.BookType = enBookType.Regular;
                    break;
                case "epublished":
                    book.BookType = enBookType.EPublished;
                    break;
                default:
                    book.BookType = enBookType.Regular;
                    break;
            }

            book.Title = bookDto.title;
            book.Subtitle = bookDto.subtitle;
            book.Author = bookDto.authors;
            book.Publisher = bookDto.publisher;
            book.Pages = bookDto.pages;
            book.ISBN = bookDto.isbn;
            book.FirstRegistrationYear = bookDto.publicationYear;
            book.SupplierCode = bookDto.supplierCode;
            book.IsActive = true;

            UnitOfWork.Commit();

        }
    }
}
