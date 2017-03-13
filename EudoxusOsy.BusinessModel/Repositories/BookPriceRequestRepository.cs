using System;
using System.Collections.Generic;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public class BookPriceRequestRepository : DomainRepository<DBEntities, BookPriceRequest, int>
    {
        #region [ Base .ctors ]

        public BookPriceRequestRepository() : base() { }

        public BookPriceRequestRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public BookPriceRequest FindByID(int id)
        {
            return BaseQuery
                    .FirstOrDefault(x => x.ID == id);
        }

        public List<BookPriceRequest> FindByBookID(int bookID)
        {
            return BaseQuery
                    .Where(x => x.BookID == bookID).ToList();
        }

        public List<BookPriceRequest> FindByBookKpsID(int bookKpsID)
        {
            return BaseQuery
                    .Where(x => x.BookKpsID == bookKpsID).ToList();
        }

        public List<BookPriceRequest> FindBySupplierID(int supplierID)
        {
            return BaseQuery
                    .Where(x => x.SupplierID == supplierID).ToList();
        }

    }
}
