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

namespace EudoxusOsy.Portal.Secure.Ministry.EditorPopups
{
    public partial class EditTransfer : BaseSecureEntityPortalPage<BankTransfer>
    {
        #region [ Entity Fill ]

        protected override void Fill()
        {
            int transferID;
            if (int.TryParse(Request.QueryString["id"], out transferID) && transferID > 0)
            {
                Entity = new BankTransferRepository(UnitOfWork).Load(transferID);
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "hidePopup", "window.parent.popUp.hide();", true);
            }
        }

        protected override bool Authorize()
        {
            return EudoxusOsyRoleProvider.IsAuthorizedEditorUser();
        }

        #endregion

        #region [ Page Inits ]

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsAuthorized)
            {
                ClientScript.RegisterStartupScript(GetType(), "hidePopup", "window.parent.popUp.hide();", true);
            }

            if (!Page.IsPostBack)
            {
                ucTransferInput.Entity = Entity;
                ucTransferInput.Bind();
            }
        }

        #endregion

        #region [ Button Handlers ]

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ASPxEdit.ValidateEditorsInContainer(Page, "vgTransfer"))
                return;

            ucTransferInput.Fill(Entity);

            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }

        #endregion
    }
}