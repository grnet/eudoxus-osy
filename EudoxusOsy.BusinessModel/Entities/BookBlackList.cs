using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public partial class BookBlackList
    {
        public enBookBlackListStatus Status
        {
            get { return (enBookBlackListStatus)StatusInt; }
            set
            {
                if (StatusInt != (int)value)
                    StatusInt = (int)value;
            }
        }
    }
}
