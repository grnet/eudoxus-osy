using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;

namespace EudoxusOsy.Portal.UserControls.ExportGridViews
{
    public partial class CatalogsReportExportGridView : BaseGridViewUserControl
    {
        #region [ Properties ]

        public override ASPxGridView Grid
        {
            get { return gvCatalogsReport; }
        }

        public ASPxGridViewExporter Exporter { get { return gveCatalogsReport; } }

        public event EventHandler<ASPxGridViewExportRenderingEventArgs> ExporterRenderBrick;

        #endregion

        #region [ GridView Events ]

        protected void gveCatalogsReport_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            if (ExporterRenderBrick != null)
                ExporterRenderBrick(sender, e);
        }

        public void Export(IEnumerable<CatalogsReportItems> source, string fileName)
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