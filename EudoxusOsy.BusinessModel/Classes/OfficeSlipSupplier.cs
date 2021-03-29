namespace EudoxusOsy.BusinessModel
{
    public class OfficeSlipSupplier
    {
        public int ID { get; set; }
        public int GroupID { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public decimal InvoiceValue { get; set; }
        public decimal Vat { get; set; }
        public decimal FinalAmount { get; set; }
        public decimal DeductionIncomeTax { get; set; }
        public decimal Deduction { get; set; }
        public decimal PayableAmount { get; set; }
    }
}