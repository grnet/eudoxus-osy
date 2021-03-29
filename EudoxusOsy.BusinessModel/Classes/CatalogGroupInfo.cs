using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public class CatalogGroupInfo
    {
        public int ID { get; set; }
        public int SupplierID { get; set; }
        public int InstitutionID { get; set; }
        public int GroupStateInt { get; set; }
        public bool ContainsInActiveBooks { get; set; }
        public bool HasPendingPriceVerification { get; set; }
        public bool HasUnexpectedPriceChange { get; set; }
        public bool IsLocked { get; set; }
        public int CatalogCount { get; set; }
        public decimal? TotalAmount { get; set; }
        public int InvoiceCount { get; set; }
        public decimal? InvoiceSum { get; set; }
        public Deduction Deduction { get; set; }
        public decimal? Vat { get; set; }
        public bool IsTransfered { get; set; }
        public int? TransferedBankID { get; set; }
        public DateTime? OfficeSlipDate { get; set; }
        public enDeductionVatType? DeductionVatType { get; set; }

        public bool IsCatalogGroupInvoicePDFAvailable { get; set; }

        public bool CanBePaid
        {
            get
            {
                return !IsLocked
                   && !ContainsInActiveBooks
                   && !HasPendingPriceVerification
                   && !HasUnexpectedPriceChange;
            }
        }

        public bool CanSupplierEditGroup
        {
            get
            {
                return !IsLocked && GroupStateInt == (int)enCatalogGroupState.New;
            }
        }
    }
}
