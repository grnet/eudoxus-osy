using DevExpress.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Collections.Generic;

namespace EudoxusOsy.Portal.UserControls.ExportGridViews
{
    public partial class PriceVerificationMinistryExportGrid : BaseGridViewUserControl
    {
        #region [ Properties ]

        public override ASPxGridView Grid
        {
            get { return gvPriceVerification; }
        }

        public ASPxGridViewExporter Exporter { get { return gvePriceVerification; } }

        public event EventHandler<ASPxGridViewExportRenderingEventArgs> ExporterRenderBrick;

        #endregion

        #region [ GridView Events ]

        protected void gvePriceVerification_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            if (ExporterRenderBrick != null)
                ExporterRenderBrick(sender, e);
        }

        public void Export(IEnumerable<PriceVerificationExportInfo> source, string fileName)
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