using System;
using System.Collections.Generic;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public class FileRepository : DomainRepository<DBEntities, File, int>
    {
        #region [ Base .ctors ]

        public FileRepository() : base() { }

        public FileRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public List<File> GetMatchingFilesByFilename(string expression)
        {
            return BaseQuery.Where(x => x.FileName.Contains(expression)).ToList();
        }
   }
}
