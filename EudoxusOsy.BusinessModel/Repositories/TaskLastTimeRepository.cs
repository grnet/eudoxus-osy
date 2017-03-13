using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public class TaskLastRunTimeRepository : BaseRepository<TaskLastRunTime>
    {

        #region [ Constructors ]

        public TaskLastRunTimeRepository() : base() { }

        public TaskLastRunTimeRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public TaskLastRunTime FindByName(string name)
        {
            return BaseQuery.FirstOrDefault(x => x.Name == name);
        }
    }
}

