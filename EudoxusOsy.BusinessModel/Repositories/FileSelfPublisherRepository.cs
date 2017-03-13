using System;
using System.Collections.Generic;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public class FileSelfPublisherRepository : DomainRepository<DBEntities, FileSelfPublisher, int>
    {
        #region [ Base .ctors ]

        public FileSelfPublisherRepository() : base() { }

        public FileSelfPublisherRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion
   }
}
