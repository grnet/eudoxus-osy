namespace EudoxusOsy.BusinessModel
{
    public class SupplierExportInfo
    {
        public int SupplierKpsID { get; set; }
        public string Name { get; set; }
        public string AFM { get; set; }
        public string ContactName { get; set; }
        public string TradeName { get; set; }
        public string SupplierType { get; set; }
        public string SupplierStatus { get; set; }
        public string DOY { get; set; }
        public string PaymentDOY { get; set; }
        public string IBAN { get; set; }
        public string Telephone { get; set;}
        public string Email { get; set; }
        public string Url { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string NoLogisticBooks { get; set; }
    }
}