using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;

namespace EudoxusOsy.Portal.UserControls.GridViews
{
    public partial class SupplierPhaseStatisticsGridView : BaseGridViewUserControl
    {
        #region [ Properties ]

        public override ASPxGridView Grid
        {
            get { return gvSupplierPhaseStatistics; }
        }

        public bool UseDefaultColors { get; set; }

        public ASPxGridViewExporter Exporter { get { return gveSupplierPhaseStatistics; } }

        public event EventHandler<ASPxGridViewExportRenderingEventArgs> ExporterRenderBrick;

        #endregion

        protected void Page_Load(object source, EventArgs e)
        {
            //Grid.AllColumns.First(x => x.Name == "PhaseAmount").Visible = HttpContext.Current.User.IsInRole(RoleNames.Helpdesk)
            //    || HttpContext.Current.User.IsInRole(RoleNames.MinistryPayments)
            //    || HttpContext.Current.User.IsInRole(RoleNames.SuperHelpdesk)
            //    || HttpContext.Current.User.IsInRole(RoleNames.SystemAdministrator);
        }

        #region [ GridView Events ]

        protected void gveSupplierPhaseStatistics_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            if (ExporterRenderBrick != null)
                ExporterRenderBrick(sender, e);
        }

        #endregion
    }
}
