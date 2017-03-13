using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public partial class WelfareRecordEntry
    {
        public enWelfareRecordEntryState State
        {
            get { return (enWelfareRecordEntryState)StateInt; }
            set
            {
                if (StateInt != (int)value)
                    StateInt = (int)value;
            }
        }
    }
}
