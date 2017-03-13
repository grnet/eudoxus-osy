using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System.Drawing;
using Imis.Domain;
using DevExpress.Web;

namespace EudoxusOsy.Portal.Secure.EditorPopups
{
    public partial class ViewCatalogGroupDetails : BaseEntityPortalPage<CatalogGroup>
    {
        #region [ Entity Fill ]
        
        private List<Catalog> _connectedCatalogs = new List<Catalog>();

        protected override void Fill()
        {
            int catalogGroupID;
            if (int.TryParse(Request.QueryString["id"], out catalogGroupID))
            {
                using (UnitOfWork.SingleConnection())
                {
                    Entity = new CatalogGroupRepository(UnitOfWork).Load(catalogGroupID);

                    if (Entity != null)
                    {
                        _connectedCatalogs = new CatalogRepository(UnitOfWork).FindConnectedCatalogsByGroupID(Entity.ID);
                    }
                }
            }

            if (Entity == null)
            {
                ClientScript.RegisterStartupScript(GetType(), "hidePopup", "window.parent.popUp.hide();", true);
            }
        }

        #endregion

        #region [ Page Inits ]

        protected void Page_Init(object sender, EventArgs e)
        {
            gvConnectedCatalogs.DataSource = _connectedCatalogs;
            gvConnectedCatalogs.DataBind();
        }

        #endregion
    }
}