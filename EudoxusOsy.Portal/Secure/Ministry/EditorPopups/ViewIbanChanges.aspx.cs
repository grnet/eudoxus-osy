using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;
using System.Drawing;
using EudoxusOsy.Portal.Controls;
using System.Xml.Linq;
using System.Xml;

namespace EudoxusOsy.Portal.Secure.Ministry.EditorPopups
{
    public partial class ViewIbanChanges : BaseEntityPortalPage<Supplier>
    {
        #region [ Entity Fill ]

        protected override void Fill()
        {
            int supplierID;
            if (int.TryParse(Request.QueryString["id"], out supplierID) && supplierID > 0)
            {
                Entity = new SupplierRepository(UnitOfWork).Load(supplierID, x => x.SupplierIBANs);
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
            }
        }

        #endregion

        #region [ Page Events ]

        protected void Page_Load(object source, EventArgs e)
        {
            gvSupplierIBANs.DataSource = Entity.SupplierIBANs;
            gvSupplierIBANs.DataBind();
        }

        #endregion
    }
}