using System;
using System.Linq;

namespace EudoxusOsy.BusinessModel
{
    public partial class Book
    {
        public BookPriceChange LastPriceChange
        {
            get
            {
                if (BookPriceChanges == null || BookPriceChanges.Count == 0)
                {
                    return null;
                }

                return BookPriceChanges.OrderByDescending(x => x.CreatedAt).FirstOrDefault();
            }
        }

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
