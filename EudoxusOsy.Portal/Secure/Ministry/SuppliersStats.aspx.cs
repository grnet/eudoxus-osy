using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.Security;

namespace EudoxusOsy.Portal.Secure.Ministry
{
    public partial class SuppliersStats : BaseEntityPortalPage
    {
        #region [ Page inits ]

        protected void cmbPhases_Init(object sender, EventArgs e)
        {
            cmbPhases.FillPhases(true);
            if (cmbPhases.SelectedIndex == -1)
            {
                var currentPhase = new PhaseRepository(UnitOfWork).GetCurrentPhase();
                cmbPhases.Items.FindByValue(currentPhase.ID.ToString()).Selected = true;
                bsView.PhaseID = currentPhase.ID;
                bsView.Bind();
            }
        }

        #endregion

        #region [ Button Handlers ]


        protected void cmbPhases_ValueChanged(object sender, EventArgs e)
        {
            bsView.PhaseID = Convert.ToInt32(cmbPhases.SelectedItem.Value);
            bsView.Bind();
            gvSuppliersStats.DataBind();
        }

        #endregion

        #region [ DataSource Events ]
        protected void odsSuppliers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            var filters = ucSearchFilters.GetSearchFilters();

            e.InputParameters["supplierKpsID"] = filters.SupplierKpsID;
            e.InputParameters["afm"] = filters.SupplierAFM;
            e.InputParameters["supplierType"] = (int?)filters.SupplierType;
            e.InputParameters["name"] = filters.SupplierName;
            e.InputParameters["phaseID"] = Convert.ToInt32(cmbPhases.SelectedItem.Value);
        }

        #endregion

        #region [ GridView Events ]

        protected void gvSuppliersStats_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();

            if (action == "refresh")
            {
                gvSuppliersStats.DataBind();
                return;
            }

            gvSuppliersStats.DataBind();
        }

        #endregion

        #region [ GridView Methods ]

        #endregion


    }
}