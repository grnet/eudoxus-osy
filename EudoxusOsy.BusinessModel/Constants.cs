using System.Configuration;

namespace EudoxusOsy.BusinessModel
{
    public static class EudoxusOsyConstants
    {
        public const int OSY_STARTING_YEAR = 2010;
        public const int FOREIGN_PFO_ID = -1;
        public const string SELECTED_PHASE_KEY = "_selectedPhaseID";
        public const string CHANGED_PROVIDER_USERS_KEY = "_changedProviderUsersKey";
        public static readonly string SQLXML_BULK_CONNECTIONSTRING = ConfigurationManager.ConnectionStrings["SqlXmlBulk"].ConnectionString;
    }

    public static class RoleNames
    {
        public const string SystemAdministrator = "SystemAdministrator";

        public const string SuperHelpdesk = "SuperHelpdesk";
        public const string Helpdesk = "Helpdesk";

        public const string SuperReports = "SuperReports";
        public const string Reports = "Reports";

        public const string Supplier = "Supplier";
        public const string MinistryWelfare = "MinistryWelfare";
        public const string MinistryPayments = "MinistryPayments";
        public const string MinistryAuditor = "MinistryAuditor";
        public const string SuperMinistry = "SuperMinistry";
    }

    public static class TaskNames
    {
        public const string GenerateReportFiles = "GenerateReportFiles";
        public const string UpdateReceiptsFromAuditReceipts = "UpdateReceiptsFromAuditReceipts";
        public const string CacheStats = "CacheStats";
        public const string CacheStatsPP = "CacheStatsPP";
        public const string CompareXmlReceipts = "CompareXmlReceipts";
        public const string UpdateBooks = "UpdateBooks";
    }

    public static class DeductionNames
    {
        public const string Mtpy = "MTPY";
        public const string Stamp = "STAMP";
        public const string Oga = "OGA";
        public const string IncomeTaxAmount = "INCOMETAXAMOUNT";
        public const string TotalDeductionsAmount = "TOTALDEDUCTIONS";
        public const string TotalVatAmount = "TOTALVATAMOUNT";
    }

    public static class ApplicationDataNames
    {
        public const string CurrentAuditReceiptXml = "CurrentAuditReceiptXml";
        public const string ShouldRunXmlComparisonAndSendReports = "ShouldRunXmlComparisonAndSendReports";
        public const string ShouldRunComplementReceipts = "ShouldRunComplementReceipts";
    }
}