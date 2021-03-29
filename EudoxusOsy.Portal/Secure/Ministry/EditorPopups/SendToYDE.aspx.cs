using EudoxusOsy.BusinessModel;
using EudoxusOsy.BusinessModel.Flow;
using EudoxusOsy.Portal.Controls;
using System;
using EudoxusOsy.Portal.Utils;

namespace EudoxusOsy.Portal.Secure.Ministry.EditorPopups
{
    public partial class SendToYDE : BaseSecureEntityPortalPage<CatalogGroup>
    {
        #region [ Entity Fill ]

        protected override void Fill()
        {
            int groupID;
            if (int.TryParse(Request.QueryString["id"], out groupID) && groupID > 0)
            {
                Entity = new CatalogGroupRepository(UnitOfWork).Load(groupID, x => x.Catalogs);
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

        protected void Page_Load(object sender, EventArgs E)
        {
            if (!IsAuthorized)
            {
                ClientScript.RegisterStartupScript(GetType(), "hidePopup", "window.parent.popUp.hide();", true);

            }
        }

        #endregion

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            PaymentOrdersUserManagement poum = new PaymentOrdersUserManagement(UnitOfWork);
            poum.MoveToState(enCatalogGroupTriggers.SendToYDE, Entity, User.Identity.Name, txtSentComments.GetText(), txtSentDate.GetDate());

            ClientScript.RegisterStartupScript(GetType(), "closePopup", "window.parent.cmdRefresh();window.parent.popUp.hide();", true);
        }
    }
}