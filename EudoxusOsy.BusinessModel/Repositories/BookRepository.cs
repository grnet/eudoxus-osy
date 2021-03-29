using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public class BookRepository : DomainRepository<DBEntities, Book, int>, IBookRepository
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

        public List<Book> FindByBookKpsID(int bookKpsID, params Expression<Func<Book, object>>[] includeExpressions)
        {
            var query = BaseQuery.Include(x => x.BookPrices);

            if (includeExpressions.Length > 0)
            {
                foreach (var item in includeExpressions)
                    query = query.Include(item);
            }

            return query.Where(x => x.BookKpsID == bookKpsID).ToList();
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
