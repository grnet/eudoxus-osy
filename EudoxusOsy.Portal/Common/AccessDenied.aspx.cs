using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using EudoxusOsy.BusinessModel;

namespace EudoxusOsy.Portal.Common
{
    public partial class AccessDenied : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LoginStatus1_OnLoggingOut(object sender, LoginCancelEventArgs e)
        {
            var c = Response.Cookies[Roles.CookieName + "_"];
            if (c != null)
                c.Expires = DateTime.Now.AddDays(-1);
        }

        protected void LoginStatus1_LoggedOut(object sender, EventArgs e)
        {
            AuthenticationService.ClearRoleCookie();
        }
    }
}
