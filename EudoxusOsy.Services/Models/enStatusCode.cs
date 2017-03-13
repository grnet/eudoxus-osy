namespace EudoxusOsy.Services
{
    public enum enStatusCode
    {
        OK = 1,
        Errors,
        UnexpectedError,

        SupplierCreated,
        SupplierUpdated,

        MinistryPaymentsUserCreated,
        MinistryPaymentsUserUpdated,

        KPSRegistrationInsertionSucceeded,
        KPSRegistrationInsertionFailed,

        KpsBooksInsertionSucceeded,
        KpsBooksInsertionFailed,
    }
}
