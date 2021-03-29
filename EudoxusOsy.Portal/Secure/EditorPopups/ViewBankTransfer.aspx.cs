using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Imis.Domain;
using EudoxusOsy.Portal.Controls;
using System.Web.Security;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Utils;
using EudoxusOsy.Portal.Utils;
using DevExpress.Web;

namespace EudoxusOsy.Portal.Secure.EditorPopups
{
    public partial class ViewBankTransfer : BaseSecureEntityPortalPage<CatalogGroup>
    {
        #region [ Entity Fill ]

        protected override void Fill()
        {
            int groupID;
            if (int.TryParse(Request.QueryString["id"], out groupID) && groupID > 0)
            {
                Entity = new CatalogGroupRepository(UnitOfWork).Load(groupID, x => x.Bank, x => x.Supplier);
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "hidePopup", "window.parent.popUp.hide();", true);
            }
        }

        protected override bool Authorize()
        {
            return (EudoxusOsyRoleProvider.IsAuthorizedEditorUser()
                || Entity.Supplier.ReporterID == User.Identity.ReporterID);
        }

        #endregion

        #region [ Page Inits ]

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsAuthorized)
            {
                ClientScript.RegisterStartupScript(GetType(), "hidePopup", "window.parent.popUp.hide();", true);
            }
            else if (Entity.BankID.HasValue)
            {
                lblBank.Text = Entity.Bank.Name;
            }
        }

        #endregion
    }
}