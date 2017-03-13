using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.ComponentModel;
using EudoxusOsy.Portal.Controls;
using EudoxusOsy.BusinessModel;
using DevExpress.Web;

namespace EudoxusOsy.Portal.Common
{
    public partial class ChangePassword : BaseEntityPortalPage<Reporter>
    {
        #region [ Entity Fill ]

        protected MembershipUser CurrentUser;

        protected override void Fill()
        {
            Entity = new ReporterRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);

            if (Entity != null)
            {
                CurrentUser = Membership.GetUser(Entity.Username);
            }
        }

        #endregion

        #region [ Page Inits ]

        protected void Page_Load(object sender, EventArgs e)
        {
            ucChangePassword.RequestOldPassword = true;
        }

        #endregion

        #region [ Button Handlers ]

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ASPxEdit.ValidateEditorsInContainer(Page, "vgChangePassword"))
                return;

            lblErrors.Text = string.Empty;

            string oldPassword = ucChangePassword.OldPassword;
            string newPassword = ucChangePassword.NewPassword;

            if (CurrentUser.ChangePassword(oldPassword, newPassword))
            {
                //Entity.MustChangePassword = false;
                UnitOfWork.Commit();

                AuthenticationService.InvalidateCookie(CurrentUser.UserName, true);
                AuthenticationService.LoginReporter(Entity);

                mvChangePassword.SetActiveView(vPasswordChanged);
            }
            else
            {
                lblErrors.Text = "Ο παλιός κωδικός πρόσβασης δεν είναι σωστός. Βεβαιωθείτε ότι εισάγετε σωστά τον κωδικό που σας ήρθε με το email Υπενθύμισης Κωδικού Πρόσβασης.";
            }
        }

        #endregion
    }
}
