using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public partial class BookPricesV 
    {
        public bool HasPendingPriceVerification
        {
            get { return PendingCommitteePriceVerification != null ? PendingCommitteePriceVerification.Value : false; }
        }

        public bool HasUnexpectedPriceChange
        {
            get { return HasUnexpectedPriceChangePhaseID != null && HasUnexpectedPriceChangePhaseID.Value > 0; }
        }
    }
}
