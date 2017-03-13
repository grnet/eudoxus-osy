using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.ComponentModel;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;

namespace EudoxusOsy.Portal.Common
{
    public partial class AlterPassword : BaseEntityPortalPage<Reporter>
    {
        #region [ Properties ]

        [DefaultValue(true)]
        private bool _requestOldPassword;
        public bool RequestOldPassword
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                    _requestOldPassword = Convert.ToBoolean(Request.QueryString["id"]);

                return _requestOldPassword;
            }
            set
            {
                _requestOldPassword = value;
            }
        }

        #endregion

        protected MembershipUser CurrentUser;

        protected override void Fill()
        {
            Entity = new ReporterRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);

            if (Entity != null)
            {
                CurrentUser = Membership.GetUser(Entity.Username);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ucChangePassword.RequestOldPassword = RequestOldPassword;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            string oldPassword = ucChangePassword.OldPassword;
            string newPassword = ucChangePassword.NewPassword;

            if (!RequestOldPassword)
            {
                oldPassword = CurrentUser.ResetPassword();
            }

            if (CurrentUser.ChangePassword(oldPassword, newPassword))
            {
                AuthenticationService.InvalidateCookie(CurrentUser.UserName, true);

                ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
            }
            else
            {
                lblErrors.Visible = true;
            }
        }
    }
}
