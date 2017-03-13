using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EudoxusOsy.Portal.UserControls.ExportGridViews
{
    public partial class OfficeSlipExportGridView : BaseGridViewUserControl
    {
        #region [ Properties ]

        public override ASPxGridView Grid
        {
            get { return gvOfficeSlip; }
        }

        public bool UseDefaultColors { get; set; }

        public ASPxGridViewExporter Exporter { get { return gveOfficeSlip; } }

        public event EventHandler<ASPxGridViewExportRenderingEventArgs> ExporterRenderBrick;

        #endregion

        #region [ GridView Events ]

        protected void gveOfficeSlip_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            if (ExporterRenderBrick != null)
                ExporterRenderBrick(sender, e);
        }

        #endregion

        #region [ GridView Methods ]
        public void Export(IEnumerable<PaymentOrder> source, string fileName)
        {
            Grid.Visible = true;
            Grid.DataSource = source;
            Grid.DataBind();

            Exporter.FileName = fileName;
            Exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
        }
        #endregion
    }
}