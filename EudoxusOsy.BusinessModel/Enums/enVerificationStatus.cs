using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public enum enVerificationStatus
    {
        /// <summary>
        /// Μη Πιστοποιημένος Χρήστης
        /// </summary> 
        NotVerified = 0,

        /// <summary>
        /// Πιστοποιημένος Χρήστης
        /// </summary>
        Verified = 1,

        /// <summary>
        /// Χρήστης που δεν μπορεί να πιστοποιηθεί
        /// </summary>
        CannotBeVerified = 2
    }
}
