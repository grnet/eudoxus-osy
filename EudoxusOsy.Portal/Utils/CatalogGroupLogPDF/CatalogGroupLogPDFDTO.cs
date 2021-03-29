using System.Data;
using EudoxusOsy.BusinessModel;

namespace EudoxusOsy.Portal
{
    public class CatalogGroupLogPDFDTO
    {

        public enCatalogGroupState? State { get; set; }
        public string CatalogGroupID { get; set; }
        public string YearFromToLiteral { get; set; }
        public string AFM { get; set; }
        public string SupplierName { get; set; }
        public string DOY { get; set; }
        public string IBAN { get; set; }
        public string Email { get; set; }
        public string Comments { get; set; }
        public string AcademicInstitution { get; set; }
        public string IncomeTax { get; set; }
        public string IncomeTaxPerc { get; set; }
        public string TotalDeductionsAmount { get; set; }
        public string PaymentAmount { get; set; }
        public string StateLabel { get; set; }
        public string TotalVatAmount { get; set; }
        public string GrandTotalAmount { get; set; }
        public string HideVAT { get; set; }
        public string StampAmount { get; set; }
        public string OgaAmount { get; set; }
        public DataTable BookInCatalogInfo { get; set; }
    }
}