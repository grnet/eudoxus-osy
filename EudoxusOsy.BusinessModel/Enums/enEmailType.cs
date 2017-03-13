namespace EudoxusOsy.BusinessModel
{
    public enum enEmailType
    {
        /// <summary>
        /// Custom Message
        /// </summary>
        CustomMessage = 1,

        /// <summary>
        /// Αλλαγές σε IBAN
        /// </summary>
        IbanChanges = 2,

        /// <summary>
        /// Daily AuditReceipts process Report
        ///</summary>
        DailyAuditReceiptsProcessReport = 3,

        /// <summary>
        /// Service book changes from KPS process Report
        ///</summary>
        ServiceBookChangesReport = 4
    }
}
