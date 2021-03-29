using System;
using System.Web;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.Security;
using Imis.Domain;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;

namespace EudoxusOsy.Portal
{
    public partial class Default : BaseEntityPortalPage<Reporter>
    {
        protected override void Fill()
        {
            Entity = new ReporterRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            login1.FailureText = "Λάθος όνομα χρήστη ή κωδικός πρόσβασης.";

            if (!Page.IsPostBack && Page.User.Identity.IsAuthenticated)
            {
                if (Entity == null)
                {
                    lblErrors.Text = "Παρουσιάστηκε κάποιο πρόβλημα στην εφαρμογή. Παρακαλούμε κλείστε τον browser που χρησιμοποιείται και ανοίξτε τον ξανά για να διορθωθεί.";
                    return;
                }

                string redirectUrl = string.Empty;

                var roles = Roles.GetRolesForUser(User.Identity.Name);

                if (roles.Contains(RoleNames.Supplier))
                {
                    if (Entity.VerificationStatus == enVerificationStatus.Verified)
                    {
                        redirectUrl = ResolveClientUrl("~/Secure/Suppliers/Default.aspx");
                    }
                    else
                    {
                        redirectUrl = ResolveClientUrl("~/Common/AccessDenied.aspx");
                    }
                }
                else if (roles.Contains(RoleNames.MinistryWelfare))
                {
                    redirectUrl = ResolveClientUrl("~/Secure/Welfare/Default.aspx");
                }
                else if (roles.Contains(RoleNames.MinistryPayments) || 
                        roles.Contains(RoleNames.MinistryAuditor) || 
                        roles.Contains(RoleNames.SuperMinistry))
                {
                    redirectUrl = ResolveClientUrl("~/Secure/Ministry/Default.aspx");
                }
                else if (roles.Contains(RoleNames.SuperHelpdesk)
                    || roles.Contains(RoleNames.Helpdesk))
                {
                    redirectUrl = ResolveClientUrl("~/Secure/Helpdesk/Default.aspx");
                }
                else if (roles.Contains(RoleNames.Reports) ||
                         roles.Contains(RoleNames.SuperReports))
                {
                    redirectUrl = ResolveClientUrl("~/Secure/Reports/Default.aspx");
                }
                else if (roles.Contains(RoleNames.SystemAdministrator))
                {
                    redirectUrl = ResolveClientUrl("~/Admin/Default.aspx");
                }
                else
                {
                    redirectUrl = ResolveClientUrl("~/Common/AccessDenied.aspx");
                }

                if (!string.IsNullOrEmpty(redirectUrl))
                {
                    Response.Clear();
                    Response.Write(string.Format("<script type='text/javascript'>window.location.href = '{0}';</script>", redirectUrl));
                    Response.End();
                    return;
                }
            }

            LinkButton loginButton = login1.FindControl("LoginButton") as LinkButton;
            TextBox txtUserName = login1.FindControl("UserName") as TextBox;
            TextBox txtPassword = login1.FindControl("Password") as TextBox;

            txtUserName.Attributes["onkeypress"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(loginButton, string.Empty));
            txtPassword.Attributes["onkeypress"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(loginButton, string.Empty));

            Form.DefaultButton = loginButton.UniqueID;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            TextBox txtUserName = login1.FindControl("UserName") as TextBox;
            TextBox txtPassword = login1.FindControl("Password") as TextBox;

            txtUserName.Focus();
            txtPassword.Text = string.Empty;
        }

        protected void login1_LoggingIn(object sender, LoginCancelEventArgs e)
        {
            MembershipUser user = Membership.GetUser(login1.UserName.ToNull());

            if (user != null)
            {
                if (user.IsLockedOut)
                {
                    login1.FailureText = "Ο χρήστης είναι κλειδωμένος. Αν δεν θυμάστε τον κωδικό πρόσβασης μπορείτε να ζητήσετε υπενθύμιση κωδικού, αλλιώς μπορείτε να επικοινωνήσετε με το Γραφείο Αρωγής Χρηστών.";
                }
                else if (!user.IsApproved)
                {
                    login1.FailureText = "Ο χρήστης είναι κλειδωμένος. Παρακαλούμε απευθυνθείτε στον Κεντρικό Χρήστη που δημιούργησε το λογαριασμό.";
                }
            }
        }

        protected void login1_LoggedIn(object sender, EventArgs e)
        {
            var rep = new ReporterRepository(UnitOfWork).FindByUsername(login1.UserName.ToNull());
            if (rep != null)
                AuthenticationService.LoginReporter(rep);
        }
    }
}
