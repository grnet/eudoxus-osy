using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.BusinessModel.Interfaces;
using Imis.Domain.EF;

namespace EudoxusOsy.BusinessModel
{
    public class BookPricesVRepository : DomainRepository<DBEntities, BookPricesV, int>, IBookPricesVRepository
    {
        #region [ Base .ctors ]

        public BookPricesVRepository() : base() { }

        public BookPricesVRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion        
    }
}
