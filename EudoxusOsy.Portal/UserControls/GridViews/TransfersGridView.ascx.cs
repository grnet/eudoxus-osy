using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using DevExpress.Web;

namespace EudoxusOsy.Portal.UserControls.GridViews
{
    public partial class TransfersGridView : BaseGridViewUserControl
    {
        #region [ Properties ]

        public override ASPxGridView Grid
        {
            get { return gvTransfers; }
        }

        public ASPxGridViewExporter Exporter { get { return gveTransfers; } }

        public event EventHandler<ASPxGridViewExportRenderingEventArgs> ExporterRenderBrick;

        #endregion

        #region [ GridView Events ]

        protected void gveTransfers_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            if (ExporterRenderBrick != null)
                ExporterRenderBrick(sender, e);
        }

        #endregion

        #region [ GridView Methods ]
        public void Export(IEnumerable<BankTransfer> source, string fileName)
        {
            Grid.Visible = true;
            Grid.DataSource = source;
            var dsID = Grid.DataSourceID;
            Grid.DataSourceID = null;
            Grid.DataSourceForceStandardPaging = false;
            Grid.DataBind();
            Grid.Columns.FindByName("aa").Visible = false;

            Exporter.FileName = fileName;
            Exporter.WriteXlsxToResponse(true);


            Grid.Columns.FindByName("aa").Visible = true;
            Grid.DataSourceID = dsID;
            Grid.DataSource = null;
            Grid.DataSourceForceStandardPaging = true;
        }

        #endregion
    }
}