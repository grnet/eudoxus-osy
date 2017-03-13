namespace EudoxusOsy.BusinessModel
{
    public enum enSMSStatus
    {
        /// <summary>
        /// Δημιουργήθηκε μόνο
        /// </summary>
        CreatedOnly = 0,

        /// <summary>
        /// Πέτυχε η αποστολή
        /// </summary>
        Sent = 1,

        /// <summary>
        /// Απέτυχε η αποστολή
        /// </summary>
        Failed = 2
    }
}
