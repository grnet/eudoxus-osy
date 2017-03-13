using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;

namespace EudoxusOsy.BusinessModel
{
    public class IncomingTransfersSearchFilters : BaseSearchFilters<BankTransfer>
    {
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal? Amount { get; set; }

        public override Imis.Domain.EF.Search.Criteria<BankTransfer> GetExpression()
        {
            var expression = Imis.Domain.EF.Search.Criteria<BankTransfer>.Empty;

            if (!string.IsNullOrEmpty(InvoiceNumber))
                expression = expression.Where(x => x.InvoiceNumber, InvoiceNumber);

            if (InvoiceDate != null && InvoiceDate != DateTime.MinValue)
                expression = expression.Where(x => x.InvoiceDate, InvoiceDate);

            if (Amount.HasValue)
                expression = expression.Where(x => x.InvoiceValue, Amount);

            return string.IsNullOrEmpty(expression.CommandText) ? null : expression;
        }
    }
}