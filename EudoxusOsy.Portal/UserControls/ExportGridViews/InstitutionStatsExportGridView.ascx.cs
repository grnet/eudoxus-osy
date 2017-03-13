using DevExpress.Web;
using EudoxusOsy.BusinessModel.Classes;
using EudoxusOsy.Portal.Controls;
using System;
using System.Collections.Generic;

namespace EudoxusOsy.Portal.UserControls.ExportGridViews
{
    public partial class InstitutionStatsExportGridView : BaseGridViewUserControl
    {

        #region [ Properties ]

        public override ASPxGridView Grid
        {
            get { return gvInstitutions; }
        }

        public bool UseDefaultColors { get; set; }

        public ASPxGridViewExporter Exporter { get { return gveInstitutions; } }

        public event EventHandler<ASPxGridViewExportRenderingEventArgs> ExporterRenderBrick;

        #endregion

        #region [ GridView Events ]

        protected void gveInstitutions_RenderBrick(object sender, DevExpress.Web.ASPxGridViewExportRenderingEventArgs e)
        {
            if (ExporterRenderBrick != null)
                ExporterRenderBrick(sender, e);
        }

        public void Page_Load(object sender, EventArgs e)
        {
        }

        #endregion

        #region [ GridView Methods ]
        public void Export(IEnumerable<InstitutionStatsInfo> source, string fileName)
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