using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public partial class Receipt
    {
        public enReceiptState State
        {
            get { return (enReceiptState)StateInt; }
            set
            {
                if (StateInt != (int)value)
                    StateInt = (int)value;
            }
        }

        public enReceiptStatus Status
        {
            get { return (enReceiptStatus)StatusInt; }
            set
            {
                if (StatusInt != (int)value)
                    StatusInt = (int)value;
            }
        }
    }
}
