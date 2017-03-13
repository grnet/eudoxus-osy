using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace EudoxusOsy.BusinessModel
{
    public class BookRepository : DomainRepository<DBEntities, Book, int>
    {
        #region [ Base .ctors ]

        public BookRepository() : base() { }

        public BookRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public Book FindByID(int id)
        {
            return BaseQuery
                    .FirstOrDefault(x => x.ID == id);
        }

        public List<Book> FindByBookKpsID(int bookKpsID)
        {
            return BaseQuery
                    .Where(x => x.BookKpsID == bookKpsID).ToList();
        }

        public List<Book> FindPendingVerification(int startRowIndex, int maximumRows, string sortExpression, out int recordCount)
        {
            var query = BaseQuery
                .Include(x => x.BookPriceChanges)
                .Include(x => x.BookSuppliers)                
                .Where(x => x.PendingCommitteePriceVerification == true)
                .OrderBy(sortExpression)
                    .Skip(startRowIndex)
                    .Take(maximumRows);

            recordCount = query.Count();

            return query.ToList();
        }

    }
}
