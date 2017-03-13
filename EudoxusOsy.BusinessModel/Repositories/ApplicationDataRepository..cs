using System;
using System.Collections.Generic;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public class ApplicationDataRepository : DomainRepository<DBEntities, ApplicationData, int>
    {
        #region [ Base .ctors ]

        public ApplicationDataRepository() : base() { }

        public ApplicationDataRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion
   }
}
