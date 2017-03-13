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
    public partial class TransferSearchFiltersControl : BaseSearchFiltersControl<TransferSearchFilters>
    {
        #region [ Control Inits ]

        #endregion

        #region [ Search Filters ]

        public override TransferSearchFilters GetSearchFilters()
        {
            var filters = new TransferSearchFilters();
            filters.InvoiceNumber = txtTransferNumber.Text;
            filters.BankID = ddlBank.GetSelectedInteger();
            filters.SupplierKpsID = txtSupplierKpsID.GetInteger();
            filters.SupplierName = txtSupplierName.Text;
            filters.PhaseID = ddlPhase.GetSelectedInteger();
            filters.InvoiceDate = dateInvoiceDate.GetDate();

            return filters;
        }

        public override void SetSearchFilters(TransferSearchFilters filters)
        {
            if (!string.IsNullOrEmpty(filters.InvoiceNumber))
            {
                txtTransferNumber.Text = filters.InvoiceNumber;
            }

            if(filters.BankID.HasValue)
            {
                ddlBank.SelectedItem = ddlBank.Items.FindByValue(filters.BankID.Value);
            }

            if(filters.SupplierKpsID.HasValue)
            {
                txtSupplierKpsID.Text = filters.SupplierKpsID.ToString();
            }

            if(!string.IsNullOrEmpty(filters.SupplierName))
            {
                txtSupplierName.Text = filters.SupplierName;
            }

            if(filters.PhaseID.HasValue)
            {
                ddlPhase.SelectedItem = ddlPhase.Items.FindByValue(filters.PhaseID.Value);
            }
        }

        #endregion

        protected void ddlBank_Init(object sender, EventArgs e)
        {
            ddlBank.FillBanks(true);
        }

        protected void ddlPhase_Init(object sender, EventArgs e)
        {
            ddlPhase.FillPhases(true);
        }
    }
}