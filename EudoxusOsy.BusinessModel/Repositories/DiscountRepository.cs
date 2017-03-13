using System;
using System.Collections.Generic;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public class DiscountRepository : DomainRepository<DBEntities, Discount, int>
    {
        #region [ Base .ctors ]

        public DiscountRepository() : base() { }

        public DiscountRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public Discount FindByID(int id)
        {
            return BaseQuery
                    .FirstOrDefault(x => x.ID == id);
        }

        public Discount FindGeneralForPhase(int phaseID)
        {
            return BaseQuery
                    .FirstOrDefault(x => x.PhaseID == phaseID 
                    && !x.BookID.HasValue);
        }
    }
}
