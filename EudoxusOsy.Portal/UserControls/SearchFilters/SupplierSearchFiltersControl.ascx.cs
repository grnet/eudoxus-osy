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
    public partial class SupplierSearchFiltersControl : BaseSearchFiltersControl<SupplierSearchFilters>
    {
        public enum enSupplierSearchFiltersControlMode
        {
            Normal = 1,
            Stats = 2
        }

        public enSupplierSearchFiltersControlMode Mode { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(Mode == enSupplierSearchFiltersControlMode.Stats)
            {
                ddlSupplierStatus.Visible = false;
                tdStatus.InnerText = "Ενεργοί";
            }
        }

        #region [ Control Inits ]

        protected void ddlSupplierType_Init(object sender, EventArgs e)
        {
            ddlSupplierType.FillFromEnum<enSupplierType>();
        }

        protected void ddlSupplierStatus_Init(object sender, EventArgs e)
        {
            ddlSupplierStatus.FillFromEnum<enSupplierStatus>("-- αδιάφορο --", true, true);
        }

        #endregion

        #region [ Search Filters ]

        public override SupplierSearchFilters GetSearchFilters()
        {
            var filters = new SupplierSearchFilters();
            
            filters.SupplierKpsID = txtSupplierKpsID.GetInteger();
            filters.SupplierName = txtSupplierName.GetText();
            filters.SupplierAFM = txtSupplierAFM.GetText();            
            filters.SupplierType = ddlSupplierType.GetSelectedEnum<enSupplierType>();
            filters.SupplierStatus = ddlSupplierStatus.GetSelectedEnum<enSupplierStatus>();

            return filters;
        }

        public override void SetSearchFilters(SupplierSearchFilters filters)
        {
            if (filters.SupplierKpsID.HasValue)
                txtSupplierKpsID.Text = filters.SupplierKpsID.ToString();

            txtSupplierName.Text = filters.SupplierName;
            txtSupplierAFM.Text = filters.SupplierAFM;
            
            if (filters.SupplierType.HasValue)
                ddlSupplierType.SelectedItem = ddlSupplierType.Items.FindByValue(filters.SupplierType.GetValue());

            if (filters.SupplierStatus.HasValue)
                ddlSupplierStatus.SelectedItem = ddlSupplierStatus.Items.FindByValue(filters.SupplierStatus.GetValue());
        }

        #endregion
    }
}