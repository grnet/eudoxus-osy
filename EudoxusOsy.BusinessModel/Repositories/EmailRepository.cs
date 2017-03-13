using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public class EmailRepository : BaseRepository<Email>
    {
        #region [ Constructors ]

        public EmailRepository() : base() { }

        public EmailRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion
    }
}
