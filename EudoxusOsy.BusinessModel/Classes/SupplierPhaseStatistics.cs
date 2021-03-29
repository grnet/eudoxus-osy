namespace EudoxusOsy.BusinessModel
{
    public class SupplierPhaseStatistics
    {
        public int SupplierID { get; set; }
        public Phase Phase { get; set; }
        public decimal OwedAmount { get; set; }
        public decimal AllocatedAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public decimal PaidAmount { get; set; }
    }
}