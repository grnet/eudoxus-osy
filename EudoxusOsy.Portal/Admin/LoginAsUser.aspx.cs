using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using EudoxusOsy.BusinessModel;

namespace EudoxusOsy.Portal.Admin
{
    public partial class LoginAsUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lbtnLogin_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Session.Clear();
                Guid guid = Guid.NewGuid();
                HttpCookie cookie = new HttpCookie("lm", guid.ToString());
                Response.Cookies.Add(cookie);
                Session.Add("lm", guid.ToString());

                AuthenticationService.LoginReporter(txtUsername.Text);
                Response.Redirect("~/Default.aspx");
            }
        }

        protected void cvUsername_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = new ReporterRepository().FindByUsername(txtUsername.Text) != null;
        }
    }
}
