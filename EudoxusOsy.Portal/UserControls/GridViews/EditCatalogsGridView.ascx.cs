using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System.Data.SqlTypes;

namespace EudoxusOsy.Portal.UserControls.GridViews
{
    public partial class EditCatalogsGridView : BaseGridViewUserControl
    {
        #region [ Properties ]

        public override ASPxGridView Grid
        {
            get { return gvEditCatalogs; }
        }

        public bool UseDefaultColors { get; set; }

        public bool HideInstitution
        {
            set 
            {
                if (value)
                {
                    gvEditCatalogs.Columns[3].Visible = false;
                }
            }
        }

        public ASPxGridViewExporter Exporter { get { return gveEditCatalogs; } }

        public event EventHandler<ASPxGridViewExportRenderingEventArgs> ExporterRenderBrick;

        #endregion

        #region [ GridView Events ]

        protected void gveEditCatalogs_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            if (ExporterRenderBrick != null)
                ExporterRenderBrick(sender, e);
        }

        #endregion

        protected void gvEditCatalogs_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            int startIndex = gvEditCatalogs.PageIndex * gvEditCatalogs.SettingsPager.PageSize;
            int end = Math.Min(gvEditCatalogs.VisibleRowCount, startIndex + gvEditCatalogs.SettingsPager.PageSize);
            object[] isDeleteAllowed = new object[end - startIndex], ids = new object[end - startIndex];
            for (int n = startIndex; n < end; n++)
            {
                isDeleteAllowed[n - startIndex] = gvEditCatalogs.GetRowValues(n, "IsDeleteAllowed");
                ids[n - startIndex] = gvEditCatalogs.GetRowValues(n, "ID");
            }
            e.Properties["cpIsDeleteAllowed"] = isDeleteAllowed;
            e.Properties["cpIds"] = ids;
        }

        protected string GetGroupState(Catalog item)
        {
            if (item.CatalogGroup != null)
            {
                return item.CatalogGroup.State.GetLabel();
            }

            return string.Empty;
        }
    }
}
