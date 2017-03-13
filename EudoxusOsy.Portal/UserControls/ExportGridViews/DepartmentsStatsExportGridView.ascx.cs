using DevExpress.Web;
using EudoxusOsy.BusinessModel.Classes;
using EudoxusOsy.Portal.Controls;
using System;
using System.Collections.Generic;

namespace EudoxusOsy.Portal.UserControls.ExportGridViews
{
    public partial class DepartmentsStatsExportGridView : BaseGridViewUserControl
    {
        #region [ Properties ]

        public override ASPxGridView Grid
        {
            get { return gvDepartments; }
        }

        public bool UseDefaultColors { get; set; }

        public ASPxGridViewExporter Exporter { get { return gveDepartments; } }

        public event EventHandler<ASPxGridViewExportRenderingEventArgs> ExporterRenderBrick;

        #endregion

        #region [ GridView Events ]

        protected void gveDepartments_RenderBrick(object sender, DevExpress.Web.ASPxGridViewExportRenderingEventArgs e)
        {
            if (ExporterRenderBrick != null)
                ExporterRenderBrick(sender, e);
        }

        public void Page_Load(object sender, EventArgs e)
        {
        }

        #endregion

        #region [ GridView Methods ]
        public void Export(IEnumerable<DepartmentStatsInfo> source, string fileName)
        {
            Grid.Visible = true;
            Grid.DataSource = source;
            Grid.DataBind();

            Exporter.FileName = fileName;
            Exporter.WriteXlsxToResponse(true);
        }

        #endregion
    }
}