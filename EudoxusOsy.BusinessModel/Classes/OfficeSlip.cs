namespace EudoxusOsy.BusinessModel
{
    public class OfficeSlip
    {
        public string SupplierName { get; set; }
        public int GroupID { get; set; }
        public string AFM { get; set; }
        public string PaymentOffice { get; set; }
        public decimal Amount { get; set; }
        public string AmountString { get; set; }
    }
}