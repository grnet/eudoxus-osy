using System;
using System.Collections.Generic;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public class BookSupplierRepository : DomainRepository<DBEntities, BookSupplier, int>
    {
        #region [ Base .ctors ]

        public BookSupplierRepository() : base() { }

        public BookSupplierRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        protected virtual ObjectQuery<BookSupplier> BaseBookSupplierQuery { get { return BaseQuery.Where(string.Format("it.IsActive = {0}", true)); } }

        #endregion

        public BookSupplier FindBySupplierIDAndYear(int supplierID, int year)
        {
            return BaseBookSupplierQuery
                    .FirstOrDefault(x => x.SupplierID == supplierID && x.Year == year);
        }

        public List<BookSupplier> FindByBookKpsID(int bookKpsID)
        {
            return BaseBookSupplierQuery
                    .Where(x => x.BookID == bookKpsID).ToList();
        }

        public List<BookSupplier> FindBySupplierID(int supplierID)
        {
            return BaseBookSupplierQuery
                    .Where(x => x.SupplierID == supplierID).ToList();
        }

        public List<BookSupplier> FindBySupplierIDAndBookIDAndYear(int supplierID, int bookID, int year)
        {
            return BaseBookSupplierQuery
                    .Where(x => x.SupplierID == supplierID && x.BookID == bookID && x.Year == year).ToList();
        }


    }
}
