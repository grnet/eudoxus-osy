using System;
using System.Collections.Generic;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public class BankRepository : DomainRepository<DBEntities, Bank, int>
    {
        #region [ Base .ctors ]

        public BankRepository() : base() { }

        public BankRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion
   }
}
