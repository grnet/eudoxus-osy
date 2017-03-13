using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public partial class AuditReceipt
    {
        public enAuditReceiptStatus? Status
        {
            get { return (enAuditReceiptStatus?)StatusInt; }
            set
            {
                if (StatusInt != (int)value)
                    StatusInt = (int)value;
            }
        }

        public enAuditReceiptReason? Reason
        {
            get { return (enAuditReceiptReason?)ReasonInt; }
            set
            {
                if (ReasonInt != (int)value)
                    ReasonInt = (int)value;
            }
        }
    }
}
