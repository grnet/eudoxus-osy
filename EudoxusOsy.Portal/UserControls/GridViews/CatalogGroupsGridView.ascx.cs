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

namespace EudoxusOsy.Portal.UserControls.GridViews
{
    public partial class CatalogGroupsGridView : BaseGridViewUserControl
    {
        #region [ Properties ]

        public override ASPxGridView Grid
        {
            get { return gvCatalogGroups; }
        }

        public bool UseDefaultColors { get; set; }

        public ASPxGridViewExporter Exporter { get { return gveCatalogGroups; } }

        public event EventHandler<ASPxGridViewExportRenderingEventArgs> ExporterRenderBrick;

        #endregion

        #region [ GridView Events ]

        protected void gveCatalogGroups_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            if (ExporterRenderBrick != null)
                ExporterRenderBrick(sender, e);
        }

        #endregion

        #region [ GridView Methods ]

        //protected int GetCatalogCount(CatalogGroup group)
        //{
        //    if (group == null)
        //        return 0;

        //    return group.Catalogs
        //            .Where(x => x.StatusInt == (int)enCatalogStatus.Active && (x.StateInt == (int)enCatalogState.Normal || x.StateInt == (int)enCatalogState.FromMove))
        //            .Count();
        //}

        //protected decimal GetTotalAmount(CatalogGroup group)
        //{
        //    if (group == null)
        //        return 0;

        //    return group.Catalogs
        //            .Where(x => x.StatusInt == (int)enCatalogStatus.Active 
        //                    && (x.StateInt == (int)enCatalogState.Normal || x.StateInt == (int)enCatalogState.FromMove))
        //            .Sum(x => (decimal)x.Amount);
        //}

        #endregion
    }
}
