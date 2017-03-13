using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using EudoxusOsy.Portal.Controls;
using System.Web.Script.Serialization;
using EudoxusOsy.BusinessModel;
using System.Threading;

namespace EudoxusOsy.Portal.Admin
{
    public partial class Admin : BaseMasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var urls = new
            {
                /* Helpdesk Urls */
                ChangePasswordUrl = ResolveClientUrl("~/Common/AlterPassword.aspx")
            };

            Page.ClientScript.RegisterStartupScript(
                GetType(),
                "adminUrlLinks",
                string.Format("var adminUrls = {0};", new JavaScriptSerializer().Serialize(urls)),
                true);
        }

        protected bool ShowNode(SiteMapNode node)
        {
            if (node.Roles.Count == 0)
                return true;
            foreach (string r in Roles.GetRolesForUser(Thread.CurrentPrincipal.Identity.Name))
            {
                if (node.Roles.Cast<string>().Contains(r, StringComparer.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        protected void LoginStatus1_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            LoginStatus1.LogoutPageUrl = Server.MapPath("~/Admin/Default.aspx");
        }

        protected void LoginStatus1_LoggedOut(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            AuthenticationService.ClearRoleCookie();
            Response.Redirect(FormsAuthentication.LoginUrl, true);
        }
    }
}