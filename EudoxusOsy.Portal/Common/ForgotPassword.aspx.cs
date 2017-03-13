using DevExpress.Web;
using Imis.Domain;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Linq;
using System.Web.Security;

namespace EudoxusOsy.Portal.Common
{
    public partial class ForgotPassword : BaseEntityPortalPage<object>
    {
        //#region [ Page Inits ]

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    Response.Redirect("~/Default.aspx");
        //    lblInfo.Text = "";
        //}

        //#endregion

        //#region [ Button Handlers ]

        //protected void btnSendNewPassword_Click(object sender, EventArgs e)
        //{
        //    if (!ASPxEdit.ValidateEditorsInContainer(Page, "vgForgotPassword"))
        //        return;

        //    try
        //    {
        //        var users = Membership.FindUsersByEmail(txtEmail.GetText()).OfType<MembershipUser>();

        //        if (users.Count() == 0)
        //        {
        //            lblInfo.Text = "Το Email που εισάγατε δεν αντιστοιχεί σε κάποιο χρήστη του πληροφοριακού συστήματος.<br/><br/>Βεβαιωθείτε ότι το πληκτρολογείτε σωστά, διαφορετικά επικοινωνήστε με το Γραφείο Αρωγής Χρηστών στο τηλέφωνο <b>215-215-7869</b> για να διαπιστώσετε τι συμβαίνει.";
        //            return;
        //        }

        //        if (users.Count() > 1)
        //        {
        //            lblInfo.Text = "Βρέθηκαν παραπάνω από ένας χρήστες με το email που εισάγατε. Παρακαλούμε επικοινωνήστε με το Γραφείο Αρωγής Χρηστών για να διαπιστώσετε τι συμβαίνει.";
        //            return;
        //        }

        //        var user = users.Single();
        //        if (user.IsLockedOut)
        //        {
        //            user.UnlockUser();
        //        }

        //        string oldPassword = user.ResetPassword();
        //        string newPassword = Guid.NewGuid().ToString().Substring(0, 8);

        //        user.ChangePassword(oldPassword, newPassword);
        //        Membership.UpdateUser(user);

        //        using (IUnitOfWork uow = UnitOfWorkFactory.Create())
        //        {
        //            var reporter = new ReporterRepository(uow).FindByEmail(user.Email);

        //            //reporter.MustChangePassword = true;

        //            AuthenticationService.InvalidateCookie(user.UserName, false);

        //            var email = EmailFactory.GetForgotPassword(reporter, user.UserName, newPassword);
        //            uow.MarkAsNew(email);
        //            uow.Commit();

        //            EmailQueueWorker.Current.AddEmailDispatchToQueue(email.ID);
        //        }

        //        lblInfo.Text = "Ο νέος κωδικός πρόσβασης στάλθηκε με επιτυχία στο email που δηλώσατε.";
        //    }
        //    catch (Exception)
        //    {
        //        lblInfo.Text = "Υπήρξε κάποιο σφάλμα κατά την υπενθύμιση του Κωδικού Πρόσβασης.<br/><br/>Παρακαλούμε προσπαθήστε ξανά, διαφορετικά επικοινωνήστε με το Γραφείο Αρωγής Χρηστών για να διαπιστώσετε τι συμβαίνει.";
        //    }
        //}

        //#endregion
    }
}
