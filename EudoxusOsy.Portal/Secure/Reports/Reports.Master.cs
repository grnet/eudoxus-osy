using System;
using System.Web;
using System.Web.Security;
using System.Linq;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;
using System.Web.Script.Serialization;
using EudoxusOsy.Portal.Controls;
using Imis.Web.Controls;
using System.Threading;

namespace EudoxusOsy.Portal.Secure.Reports
{
    public partial class Reports : BaseMasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var urls = new
            {
                /* Helpdesk Urls */
                ViewApplicationDetailsUrl = ResolveClientUrl("~/Secure/Reports/EditorPopups/ViewApplicationDetails.aspx")
            };

            Page.ClientScript.RegisterStartupScript(
                GetType(),
                "reportUrlLinks",
                string.Format("var reportUrls = {0};", new JavaScriptSerializer().Serialize(urls)),
                true);

            if (Roles.IsUserInRole(RoleNames.Reports) || Roles.IsUserInRole(RoleNames.SuperReports))
            {
                Reporter reporter = Context.LoadReporter() ?? new ReporterRepository().FindByUsername(Page.User.Identity.Name);
            }
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
            LoginStatus1.LogoutPageUrl = Server.MapPath("~/Default.aspx");
        }

        protected void LoginStatus1_LoggedOut(object sender, EventArgs e)
        {
            AuthenticationService.ClearRoleCookie();
        }
    }
}