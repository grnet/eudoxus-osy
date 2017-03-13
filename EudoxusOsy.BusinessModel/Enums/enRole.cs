namespace EudoxusOsy.BusinessModel
{
    public enum enRole
    {
        /// <summary>
        /// Διαχειριστής
        /// </summary>
        SystemAdministrator = 1,

        /// <summary>
        /// Εκδότης
        /// </summary>
        Supplier = 2,

        /// <summary>
        /// Υπουργείο (Αιτήσεις Κοστολόγησης)
        /// </summary>
        MinistryWelfare = 3,

        /// <summary>
        /// Υπουργείο (Πληρωμές)
        /// </summary>
        MinistryPayments = 4
    }
}
