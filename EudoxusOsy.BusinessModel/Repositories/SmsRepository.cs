using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public class SmsRepository : BaseRepository<SMS>
    {
        #region [ Constructors ]

        public SmsRepository() : base() { }

        public SmsRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion
    }
}
