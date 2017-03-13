using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;

namespace EudoxusOsy.BusinessModel
{
    public class TransferSearchFilters : BaseSearchFilters<BankTransfer>
    {
        public string InvoiceNumber { get; set; }
        public int? BankID { get; set; }
        public int? PhaseID { get; set; }
        public int? SupplierKpsID { get; set; }
        public string SupplierName { get; set; }
        public DateTime? InvoiceDate { get; set; }

        public override Imis.Domain.EF.Search.Criteria<BankTransfer> GetExpression()
        {
            var expression = Imis.Domain.EF.Search.Criteria<BankTransfer>.Empty;

            if (!string.IsNullOrEmpty(InvoiceNumber))
            {
                expression = expression.Where(x => x.InvoiceNumber, InvoiceNumber, Imis.Domain.EF.Search.enCriteriaOperator.Like);
            }

            if (BankID.HasValue)
            {
                expression = expression.Where(x => x.BankID, BankID.Value);
            }

            if (SupplierKpsID.HasValue)
            {
                expression = expression.Where(x => x.Supplier.SupplierKpsID, SupplierKpsID.Value);
            }

            if(!string.IsNullOrEmpty(SupplierName))
            {
                expression = expression.Where(x => x.Supplier.Name, SupplierName, Imis.Domain.EF.Search.enCriteriaOperator.Like);
            }

            if(PhaseID.HasValue)
            {
                expression = expression.Where(x => x.PhaseID, PhaseID.Value);
            }

            if(InvoiceDate.HasValue)
            {
                expression = expression.Where(x => x.InvoiceDate, InvoiceDate.Value);
            }

            return string.IsNullOrEmpty(expression.CommandText) ? null : expression;
        }
    }
}