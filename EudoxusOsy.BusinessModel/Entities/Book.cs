using System.Linq;

namespace EudoxusOsy.BusinessModel
{
    public partial class Book : IBook
    {       
        public bool HasPendingPriceVerification
        {
            get { return PendingCommitteePriceVerification == null ? false : PendingCommitteePriceVerification.Value; }
        }

        public bool HasUnexpectedPriceChange
        {
            get { return HasUnexpectedPriceChangePhaseID != null && HasUnexpectedPriceChangePhaseID.Value > 0; }
        }

    }
}
