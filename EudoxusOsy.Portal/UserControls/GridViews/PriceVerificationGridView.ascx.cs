using DevExpress.Web;
using EudoxusOsy.Portal.Controls;
using System;
using System.Collections.Generic;
using EudoxusOsy.BusinessModel;

namespace EudoxusOsy.Portal.UserControls.GridViews
{
    public partial class PriceVerificationGridView : BaseGridViewUserControl
    {

        #region [ Properties ]

        public enum enPriceVerificationMode
        {
            Ministry = 0,
            Unexpected = 1
        }

        public enPriceVerificationMode Mode { get; set; }

        public override ASPxGridView Grid
        {
            get { return gvPriceVerification; }
        }

        public ASPxGridViewExporter Exporter { get { return gvePriceVerification; } }

        public event EventHandler<ASPxGridViewExportRenderingEventArgs> ExporterRenderBrick;
        #endregion

        protected void Page_Load(object source, EventArgs e)
        {
            if (Mode == enPriceVerificationMode.Unexpected)
            {
                gvPriceVerification.Columns["PendingCommitteePriceVerification"].Visible = false;
            }
            else if (Mode == enPriceVerificationMode.Ministry)
            {
                gvPriceVerification.Columns["UnexpectedPriceVerified"].Visible = false;
            }
        }

        #region [ GridView Events ]
        protected void gvePriceVerification_OnRenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {

            if (e.RowType == GridViewRowType.Data)
            {
                var ec = gvPriceVerification.GetRow(e.VisibleIndex) as BookPricesGridV;

                if (ec != null)
                {
                    switch (e.Column.Name)
                    {
                        case "PendingCommitteePriceVerification":
                            e.TextValue = e.Text = ec.HasPendingPriceVerification? "ΝΑΙ":"ΟΧΙ";
                            break;                          
                        case "UnexpectedPriceVerified":
                            e.TextValue = e.Text = ec.HasUnexpectedPriceChange? "ΝΑΙ":"ΟΧΙ";
                            break;                          
                        default:
                            break;
                    }
                }
            }

            if (ExporterRenderBrick != null)
                ExporterRenderBrick(sender, e);
        }

        public void Export(IEnumerable<BookPricesGridV> source, string fileName)
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