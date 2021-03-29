using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public interface IBookRepository : IBaseRepository<Book, int>
    {
        List<Book> FindByBookKpsID(int kpsID, params Expression<Func<Book, object>>[] includeExpressions);
    }
}
