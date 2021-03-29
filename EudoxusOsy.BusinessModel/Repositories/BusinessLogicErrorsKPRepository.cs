using System;
using System.Collections.Generic;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public class BusinessLogicErrorsKPRepository : DomainRepository<DBEntities, BusinessLogicErrorsKP, int>
    {
        #region [ Base .ctors ]

        public BusinessLogicErrorsKPRepository() : base() { }

        public BusinessLogicErrorsKPRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion
   }
}
