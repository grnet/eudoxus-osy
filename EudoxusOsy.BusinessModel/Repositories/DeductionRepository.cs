using System;
using System.Collections.Generic;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public class DeductionRepository : DomainRepository<DBEntities, Deduction, int>
    {
        #region [ Base .ctors ]

        public DeductionRepository() : base() { }

        public DeductionRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public Deduction FindByID(int id)
        {
            return BaseQuery
                    .FirstOrDefault(x => x.ID == id);
        }

        public List<Deduction> FindActive()
        {
            return BaseQuery
                    .Where(x => x.IsActive).ToList();
        }

    }
}
