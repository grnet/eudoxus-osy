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
    public partial class BankSearchFiltersControl : BaseSearchFiltersControl<BankSearchFilters>
    {
        #region [ Control Inits ]

        protected void ddlIsBank_Init(object sender, EventArgs e)
        {
            ddlIsBank.FillTrueFalse();
        }

        protected void ddlIsActive_Init(object sender, EventArgs e)
        {
            ddlIsActive.FillTrueFalse();
        }

        #endregion

        #region [ Search Filters ]

        public override BankSearchFilters GetSearchFilters()
        {
            var filters = new BankSearchFilters();
            
            filters.Name = txtName.GetText();
            filters.IsBank = ddlIsBank.GetSelectedBoolean();
            filters.IsActive = ddlIsActive.GetSelectedBoolean();

            return filters;
        }

        public override void SetSearchFilters(BankSearchFilters filters)
        {
            txtName.Text = filters.Name;

            if (filters.IsBank.HasValue)
                ddlIsBank.SelectedItem = ddlIsBank.Items.FindByValue((filters.IsBank.ToInt()));

            if (filters.IsActive.HasValue)
                ddlIsActive.SelectedItem = ddlIsActive.Items.FindByValue((filters.IsActive.ToInt()));
        }

        #endregion
    }
}