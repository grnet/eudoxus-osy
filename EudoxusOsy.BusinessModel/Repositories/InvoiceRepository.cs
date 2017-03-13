using System;
using System.Collections.Generic;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public class InvoiceRepository : DomainRepository<DBEntities, Invoice, int>
    {
        #region [ Base .ctors ]

        public InvoiceRepository() : base() { }

        public InvoiceRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public Invoice FindByID(int id)
        {
            return BaseQuery
                    .FirstOrDefault(x => x.ID == id);
        }

        public List<Invoice> FindByGroupID(int groupID)
        {
            return BaseQuery
                    .Where(x => x.GroupID == groupID).ToList();
        }

    }
}
