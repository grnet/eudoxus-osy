using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;

namespace EudoxusOsy.Portal.Secure
{
    public partial class RefreshRoles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var username = User.Identity.Name.ToLower();
            var changedOfficeUsers = CacheManager.ChangedProviderUsers;

            AuthenticationService.ClearRoleCookie();

            changedOfficeUsers.Remove(username);

            string returnURL = Request.QueryString["returnURL"];

            if (!string.IsNullOrEmpty(returnURL))
            {
                Response.Redirect(returnURL);
            }
            else
            {
                Response.Redirect("~/Default.aspx");
            }
        }
    }
}