using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using EudoxusOsy.Portal.Controls;
using EudoxusOsy.BusinessModel;

namespace EudoxusOsy.Portal.UserControls.SearchFilters
{
    public partial class IncomingTransfersSearchFiltersControl : BaseSearchFiltersControl<IncomingTransfersSearchFilters>
    {
        #region [ Control Inits ]

        #endregion

        #region [ Search Filters ]

        public override IncomingTransfersSearchFilters GetSearchFilters()
        {
            var filters = new IncomingTransfersSearchFilters();
            filters.InvoiceNumber = !string.IsNullOrEmpty(txtInvoiceNumber.Text) ? txtInvoiceNumber.Text : null;
            decimal result;
            if (decimal.TryParse(txtInvoiceValue.Text, out result))
            {
                filters.Amount = result;
            }
            if (dateInvoiceDate.Value != null)
            {
                filters.InvoiceDate = dateInvoiceDate.Date;
            }

            return filters;
        }

        public override void SetSearchFilters(IncomingTransfersSearchFilters filters)
        {
            if (!string.IsNullOrEmpty(filters.InvoiceNumber))
                txtInvoiceNumber.Text = filters.InvoiceNumber.ToString();

            dateInvoiceDate.Value = filters.InvoiceDate;
            txtInvoiceValue.Text = filters.Amount.HasValue ? filters.Amount.Value.ToString("c") : 0m.ToString("c");
        }

        #endregion
    }
}