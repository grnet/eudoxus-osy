namespace EudoxusOsy.BusinessModel
{
    public class OSlipInvoice
    {
        public string SupplierName { get; set; }
        public int GroupID { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string AFM { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAmount1123 { get; set; }
        public decimal TotalAmount9113 { get; set; }
        public string AmountString { get; set; }
        public string VatAmountString { get; set; }
        public string TotalAmount1123String { get; set; }
        public string TotalAmount9113String { get; set; }
    }
}