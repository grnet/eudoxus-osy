using DevExpress.Web;
using EudoxusOsy.Portal.Controls;
using System;
using System.Collections.Generic;
using EudoxusOsy.BusinessModel;

namespace EudoxusOsy.Portal.UserControls.GridViews
{
    public partial class BookPriceChangesGridView : BaseGridViewUserControl
    {
        #region [ Properties ]

        public override ASPxGridView Grid
        {
            get { return gvBookPriceChanges; }
        }

        public ASPxGridViewExporter Exporter { get { return gveBookPriceChanges; } }

        public event EventHandler<ASPxGridViewExportRenderingEventArgs> ExporterRenderBrick;

        #endregion

        protected void Page_Load(object source, EventArgs e)
        {
        }

        #region [ GridView Events ]
        
        protected void gveBookPriceChanges_OnRenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                var ec = gvBookPriceChanges.GetRow(e.VisibleIndex) as BookPriceChange;

                if (ec != null)
                {
                    switch (e.Column.Name)
                    {
                        case "BookKpsID":
                            e.TextValue = e.Text = ec.Book.BookKpsID.ToString();
                            break;                          
                        case "Title":
                            e.TextValue = e.Text = ec.Book.Title;
                            break;     
                        case "PriceChecked":
                            e.TextValue = e.Text = ec.PriceChecked ? "ΝΑΙ" : "ΟΧΙ";
                            break;     
                        case "Approved":
                            e.TextValue = e.Text = ec.IsApproved ? "ΝΑΙ" : "ΟΧΙ";
                            break;     
                        default:
                            break;
                    }
                }
            }


            if (ExporterRenderBrick != null)
                ExporterRenderBrick(sender, e);
        }

        public void Export(IEnumerable<BookPriceChange> source, string fileName)
        {
            Grid.Visible = true;

            if (string.IsNullOrEmpty(Grid.DataSourceID))
            {
                Grid.DataSource = source;
            }            
            Grid.DataBind();

            Exporter.FileName = fileName;
            Exporter.ExportWithDefaults();
        }
        #endregion
    }
}