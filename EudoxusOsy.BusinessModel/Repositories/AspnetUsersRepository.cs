using System;
using System.Collections.Generic;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public class AspnetUsersRepository : DomainRepository<DBEntities, aspnet_Users, int>
    {
        #region [ Base .ctors ]

        public AspnetUsersRepository() : base() { }

        public AspnetUsersRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public aspnet_Users FindByUsername(string username)
        {
            return BaseQuery
                    .Where(x => x.UserName == username)
                    .FirstOrDefault();
        }
    }
}
