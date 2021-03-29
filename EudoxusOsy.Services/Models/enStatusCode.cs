namespace EudoxusOsy.Services
{
    public enum enStatusCode
    {
        OK = 1,
        Errors,
        UnexpectedError,

        SupplierCreated,
        SupplierUpdated,
        SupplierInsertionFailed,

        MinistryPaymentsUserCreated,
        MinistryPaymentsUserUpdated,

        KPSRegistrationInsertionSucceeded,
        KPSRegistrationInsertionFailed,

        KpsBooksInsertionSucceeded,
        KpsBooksInsertionFailed,

        CoAuthorsInsertionSucceeded,
        CoAuthorsInsertionFailed,
    }
}
