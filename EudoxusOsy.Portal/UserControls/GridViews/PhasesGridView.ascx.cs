using DevExpress.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Linq;
using System.Web;

namespace EudoxusOsy.Portal.UserControls.GridViews
{
    public partial class PhasesGridView : BaseGridViewUserControl
    {
        #region [ Properties ]

        public override ASPxGridView Grid
        {
            get { return gvPhases; }
        }

        public bool UseDefaultColors { get; set; }

        public ASPxGridViewExporter Exporter { get { return gvePhases; } }

        public event EventHandler<ASPxGridViewExportRenderingEventArgs> ExporterRenderBrick;

        #endregion

        protected void Page_Load(object source, EventArgs e)
        {
            Grid.AllColumns.First(x => x.Name == "PhaseAmount").Visible = HttpContext.Current.User.IsInRole(RoleNames.Helpdesk)
                || HttpContext.Current.User.IsInRole(RoleNames.MinistryPayments)
                || HttpContext.Current.User.IsInRole(RoleNames.SuperMinistry)
                || HttpContext.Current.User.IsInRole(RoleNames.SuperHelpdesk)
                || HttpContext.Current.User.IsInRole(RoleNames.SystemAdministrator);

            Grid.AllColumns.First(x => x.Name == "TotalDebt").Visible = HttpContext.Current.User.IsInRole(RoleNames.Helpdesk)
                || HttpContext.Current.User.IsInRole(RoleNames.MinistryPayments)
                || HttpContext.Current.User.IsInRole(RoleNames.SuperMinistry)
                || HttpContext.Current.User.IsInRole(RoleNames.SuperHelpdesk)
                || HttpContext.Current.User.IsInRole(RoleNames.SystemAdministrator);
        }

        #region [ GridView Events ]

        protected void gvePhases_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            if (ExporterRenderBrick != null)
                ExporterRenderBrick(sender, e);
        }

        #endregion
    }
}
