using System;
using System.Collections.Generic;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public class PhaseRepository : DomainRepository<DBEntities, Phase, int>
    {
        #region [ Base .ctors ]

        public PhaseRepository() : base() { }

        public PhaseRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public Phase GetCurrentPhase()
        {
            return BaseQuery.Where(x => x.IsActive)
                    .OrderByDescending(x => x.ID)
                    .FirstOrDefault();
        }
        
    }
}
