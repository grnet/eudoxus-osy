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
    public partial class FileSelfPublisherGridView : BaseGridViewUserControl
    {
        #region [ Properties ]

        public override ASPxGridView Grid
        {
            get { return gvInvoices; }
        }

        public ASPxGridViewExporter Exporter { get { return gveFileSelfPublisher; } }

        public event EventHandler<ASPxGridViewExportRenderingEventArgs> ExporterRenderBrick;

        #endregion

        #region [ GridView Events ]

        protected void gveFileSelfPublisher_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            if (ExporterRenderBrick != null)
                ExporterRenderBrick(sender, e);
        }

        protected string CreateLink(FileSelfPublisherInfo fileSelfPublisherInfo)
        {
            return string.Format("<a href='../../Secure/FileDownload.ashx?fid={0}'>{1}</a>", fileSelfPublisherInfo.ID, fileSelfPublisherInfo.FileName);
        }

        #endregion
    }
}