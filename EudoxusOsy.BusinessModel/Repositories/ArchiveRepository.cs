using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EudoxusOsy.BusinessModel
{
    public class ArchiveRepository : DomainRepository<DBEntities, Archive, int>, IArchiveRepository
    {
        #region [ Base .ctors ]

        public ArchiveRepository() : base() { }

        public ArchiveRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion


        public List<Archive> GetActive()
        {
            return BaseQuery.Where(x => x.IsActive).OrderBy(x => x.Year).ToList();
        }
    }    
}
