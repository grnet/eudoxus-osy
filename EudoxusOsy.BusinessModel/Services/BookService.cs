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
            book.BookType = BookHelper.MapKindBookToBookType(kindbook);

            book.Title = bookDto.title;
            book.Subtitle = bookDto.subtitle;
            book.Author = bookDto.authors;
            book.Publisher = bookDto.publisher;
            book.Pages = bookDto.pages;
            book.ISBN = bookDto.isbn;
            book.FirstRegistrationYear = bookDto.firstPostYear.HasValue ? bookDto.firstPostYear : null;
            book.SupplierCode = bookDto.publisherId;
            book.IsActive = true;

            UnitOfWork.Commit();

        }
    }
}
