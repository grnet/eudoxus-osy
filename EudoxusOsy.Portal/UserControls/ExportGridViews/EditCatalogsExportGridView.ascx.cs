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
    public partial class EditCatalogsExportGridView : BaseGridViewUserControl
    {
        #region [ Properties ]

        public override ASPxGridView Grid
        {
            get { return gvEditCatalogs; }
        }

        public ASPxGridViewExporter Exporter { get { return gveEditCatalogs; } }

        public event EventHandler<ASPxGridViewExportRenderingEventArgs> ExporterRenderBrick;

        #endregion

        #region [ GridView Events ]

        protected void gveEditCatalogs_OnRenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                var ec = gvEditCatalogs.GetRow(e.VisibleIndex) as EditCatalogsGridV;

                if (ec != null)
                {
                    switch (e.Column.Name)
                    {
                        case "IsLocked":
                            e.TextValue = e.Text = ec.HasUnexpectedPriceChange || ec.HasPendingPriceVerification? "ΝΑΙ":"ΟΧΙ";
                            break;                          
                        default:
                            break;
                    }
                }
            }

            if (ExporterRenderBrick != null)
                ExporterRenderBrick(sender, e);
        }

        public void Export(IEnumerable<EditCatalogsGridV> source, string fileName)
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