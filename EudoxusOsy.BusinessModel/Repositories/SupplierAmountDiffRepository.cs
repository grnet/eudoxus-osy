using System;
using System.Collections.Generic;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public class SupplierAmountDiffRepository : DomainRepository<DBEntities, TempAmountDiff, int>
    {
        #region [ Base .ctors ]

        public SupplierAmountDiffRepository() : base() { }

        public SupplierAmountDiffRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public void RefreshAmountDiffs(int phaseID)
        {
            var ctx = GetCurrentObjectContext();
            ctx.GetSupplierMoneyDiffs(phaseID);
        }
   }
}
