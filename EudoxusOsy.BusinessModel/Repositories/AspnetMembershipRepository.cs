using System;
using System.Collections.Generic;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public class AspnetMembershipRepository : DomainRepository<DBEntities, aspnet_Membership, int>
    {
        #region [ Base .ctors ]

        public AspnetMembershipRepository() : base() { }

        public AspnetMembershipRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public aspnet_Membership FindByUserID(Guid userID)
        {
            return BaseQuery
                    .Where(x => x.UserId == userID)
                    .FirstOrDefault();
        }
    }
}
