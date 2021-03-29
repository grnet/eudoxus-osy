namespace EudoxusOsy.BusinessModel
{
    /// <summary>
    /// enumeration for error code in BusinessLogicErrorsKPS
    /// </summary>
    public enum enErrorCode
    {
        /// <summary>
        /// If the PriceChecked field that comes from KPS Service is FALSE but there is a Price value
        /// </summary>
        PriceCheckedFalseButMinistryPrice = 1,

        /// <summary>
        /// The Book has a price that is checked in OSY database but the PriceChecked value that comes from the KPS service is "False"
        /// </summary>
        BookIsCheckedInOsyButNotCheckedPriceFromKpsService = 2,

        /// <summary>
        /// The Book has pending committee price verification but the price received was not checked
        /// </summary>
        BookHasPendingPriceVerificationButUnCheckedPriceChangeReceived = 3
    }
}
