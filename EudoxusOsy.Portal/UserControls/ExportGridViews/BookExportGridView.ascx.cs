using DevExpress.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Collections.Generic;

namespace EudoxusOsy.Portal.UserControls.ExportGridViews
{
    public enum enBookGridViewMode
    {
        GroupExport = 0,
        BooksPage = 1
    }

    public partial class BookExportGridView : BaseGridViewUserControl
    {
        #region [ Properties ]

        public enBookGridViewMode Mode { get; set; }

        public override ASPxGridView Grid
        {
            get { return gvBooks; }
        }

        public bool UseDefaultColors { get; set; }

        public ASPxGridViewExporter Exporter { get { return gveBooks; } }

        public event EventHandler<ASPxGridViewExportRenderingEventArgs> ExporterRenderBrick;

        #endregion

        #region [ GridView Events ]

        protected void gveBooks_RenderBrick(object sender, DevExpress.Web.ASPxGridViewExportRenderingEventArgs e)
        {
            if (ExporterRenderBrick != null)
                ExporterRenderBrick(sender, e);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (Mode == enBookGridViewMode.BooksPage)
            {
                Grid.Columns.FindByName("BookPrice").Visible = false;
                Grid.Columns.FindByName("PaymentPrice").Visible = false;
                Grid.Columns.FindByName("Department").Visible = false;
                Grid.Columns.FindByName("BookCount").Visible = false;
                Grid.Columns.FindByName("TotalAmount").Visible = false;

                if (Grid.Columns.FindByName("PageCount") != null)
                {
                    Grid.Columns.FindByName("PageCount").Visible = false;
                }

                if (Grid.Columns.FindByName("Subtitle") != null)
                {
                    Grid.Columns.FindByName("Subtitle").Visible = false;
                }
            }
            else if (Mode == enBookGridViewMode.GroupExport)
            {
                if (Grid.Columns.FindByName("PageCount") != null)
                {
                    Grid.Columns.FindByName("PageCount").Visible = false;
                }

                if (Grid.Columns.FindByName("Subtitle") != null)
                {
                    Grid.Columns.FindByName("Subtitle").Visible = false;
                }

                if (Grid.Columns.FindByName("BookType") != null)
                {
                    Grid.Columns.FindByName("BookType").Visible = false;
                }
            }
        }

        #endregion

        #region [ GridView Methods ]
        public void Export(IEnumerable<BookInCatalogInfo> source, string fileName)
        {
            Grid.Visible = true;
            Grid.DataSource = source;
            Grid.DataBind();

            Exporter.FileName = fileName;
            Exporter.WriteXlsxToResponse(true);
        }

        public void Export(IEnumerable<Book> source, string fileName)
        {
            Grid.Visible = true;
            Grid.DataSource = source;
            Grid.DataBind();

            Exporter.FileName = fileName;
            Exporter.ExportWithDefaults();
        }
        #endregion
    }
}