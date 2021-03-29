using System.Collections.Generic;

namespace EudoxusOsy.BusinessModel
{
    public class SupplierInfo
    {
        public int ID { get; set; }
        public int KpsID { get; set; }
        public decimal? TotalDebtFromSupplierCatalogs { get; set; }
        public enSupplierType SupplierType { get; set; }
        public bool? HasLogisticsBooks { get; set; }
        public decimal AssignedMoney { get; set; }
        public decimal PaidMoney { get; set; }
        public decimal OwedMoney { get; set; }
        public double Percent { get; set; }
        public decimal Difference { get; set; }
        public decimal RemainingMoney { get; set; }
        public IList<CatalogGroup> Groups { get; set; }
        public int? IBANID { get; set; }
    }
}