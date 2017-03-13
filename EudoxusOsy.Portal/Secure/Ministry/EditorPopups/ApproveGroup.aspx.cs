using EudoxusOsy.BusinessModel;
using EudoxusOsy.BusinessModel.Flow;
using EudoxusOsy.Portal.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EudoxusOsy.Portal.Secure.Ministry.EditorPopups
{
    public partial class ApproveGroup : BaseEntityPortalPage<CatalogGroup>
    {
        #region [ Entity Fill ]

        protected override void Fill()
        {
            int groupID;
            if (int.TryParse(Request.QueryString["id"], out groupID) && groupID > 0)
            {
                Entity = new CatalogGroupRepository(UnitOfWork).Load(groupID);
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "hidePopup", "window.parent.popUp.hide();", true);
            }
        }

        #endregion

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var stateMachine = new CatalogGroupStateMachine(Entity);

            if(stateMachine.CanFire(enCatalogGroupTriggers.Approve))
            {
                var triggerParams = new CatalogGroupTriggerParams()
                {
                    Username = User.Identity.Name,
                    Comments = txtApprovalComments.GetText(),
                    UnitOfWork = UnitOfWork
                };
                
                stateMachine.Approve(triggerParams);

                UnitOfWork.Commit();
            }

            ClientScript.RegisterStartupScript(GetType(), "closePopup", "window.parent.cmdRefresh();window.parent.popUp.hide();", true);
        }
    }
}