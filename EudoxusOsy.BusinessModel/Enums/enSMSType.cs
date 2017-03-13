namespace EudoxusOsy.BusinessModel
{
    public enum enSMSType
    {
        /// <summary>
        /// SMS με κωδικό πιστοποίησης
        /// </summary>
        VerificationCode = 1,

        /// <summary>
        /// Custom Message
        /// </summary>
        CustomMessage = 2,

        /// <summary>
        /// Αποστολή κουπονιού Δικαιούχου
        /// </summary>
        BeneficiaryVoucherCode = 3,
    }
}
