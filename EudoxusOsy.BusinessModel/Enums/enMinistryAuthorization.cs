using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EudoxusOsy.BusinessModel
{
    public enum enMinistryAuthorization
    {
        None = 0,

        /// <summary>
        /// Read-Only
        /// </summary> 
        ReadOnly = 1,

        /// <summary>
        /// Διαχείρισης
        /// </summary>
        Admin = 2
    }
}