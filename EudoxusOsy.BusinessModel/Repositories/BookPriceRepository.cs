using System;
using System.Collections.Generic;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public class BookPriceRepository : DomainRepository<DBEntities, BookPrice, int>
    {
        #region [ Base .ctors ]

        public BookPriceRepository() : base() { }

        public BookPriceRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public BookPrice FindByID(int id)
        {
            return BaseQuery
                    .FirstOrDefault(x => x.ID == id);
        }

        public List<BookPrice> FindByBookID(int bookID)
        {
            return BaseQuery
                    .Where(x => x.BookID == bookID).ToList();
        }

        public BookPrice FindByBookIDAndYear(int bookID, int year)
        {
            return BaseQuery
                    .Where(x => x.BookID == bookID && x.Year == year && x.StatusInt == (int)enBookPriceStatus.Active).FirstOrDefault();
        }

    }
}
