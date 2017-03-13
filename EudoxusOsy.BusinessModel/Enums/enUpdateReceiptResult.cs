using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EudoxusOsy.BusinessModel
{
    public enum enUpdateReceiptResult
    {
        Success = 0,

        /// <summary>
        /// Invalid Reason
        /// </summary> 
        InvalidReason = 1,
        /// <summary>
        /// Zero Rows
        /// </summary> 
        ZeroRows = 2,
        /// <summary>
        /// Registration KPS is zero
        /// </summary> 
        RegistrationKpsZero = 3,
        /// <summary>
        /// repeat of reason
        /// </summary> 
        RepeatOfReason = 4,
        /// <summary>
        /// Procedure failed
        /// </summary> 
        ProcedureFailed = 5,
        /// <summary>
        /// Reserved
        /// </summary> 
        Reserved = 6,
        /// <summary>
        /// Missing Secretariat
        /// </summary> 
        SecretariatMissing = 7,
        /// <summary>
        /// Missing Depaartment
        /// </summary> 
        DepartmentMissing = 8
    }
}