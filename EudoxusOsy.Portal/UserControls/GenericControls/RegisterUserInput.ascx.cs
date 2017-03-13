using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.Portal.Controls;
using EudoxusOsy.BusinessModel;
using System.Web.Security;
using DevExpress.Web;
using System.ComponentModel;
using EudoxusOsy.Portal.Utils;
using Imis.Domain;

namespace EudoxusOsy.Portal.UserControls.GenericControls
{
    public partial class RegisterUserInput : BaseEntityUserControl<Reporter>
    {
        #region [ Properties ]

        public string UserName { get { return txtUserName.GetText(); } }
        public string Password { get { return txtPassword.GetText(); } }
        public string Email { get { return txtEmail.GetText(); } }        

        #endregion

        #region [ Control Inits ]

        protected void txtUserName_Callback(object source, CallbackEventArgsBase e)
        {

            if (Membership.GetUser(e.Parameter) != null)
            {
                txtUserName.IsValid = false;
                txtUserName.ErrorText = "Το Όνομα Χρήστη χρησιμοποιείται";
            }
        }

        protected void txtEmail_Callback(object source, CallbackEventArgsBase e)
        {

            if (!string.IsNullOrEmpty(Membership.GetUserNameByEmail(e.Parameter)))
            {
                txtEmail.IsValid = false;
                txtEmail.ErrorText = "Το 'Email χρησιμοποιείται";
            }
        }

        #endregion

        #region [ Page Inits ]

        protected void Page_Init(object sender, EventArgs e)
        {
            txtUserName.ValidationSettings.RegularExpression.ValidationExpression = RegexHelper.GetUsernameRegExp();
            txtPassword.ValidationSettings.RegularExpression.ValidationExpression = RegexHelper.GetPasswordRegExp();
            txtEmail.ValidationSettings.RegularExpression.ValidationExpression = RegexHelper.GetEmailRegExp();            
        }

        #endregion

        #region [ Extract - Bind ]

        public override Reporter Fill(Reporter entity)
        {
            if (!string.IsNullOrEmpty(entity.Email))
            {
                MembershipUser mu = Membership.GetUser(entity.Username);
                mu.Email = txtEmail.GetText();
                Membership.UpdateUser(mu);
            }

            entity.Email = txtEmail.GetText();

            return entity;
        }

        public override void Bind()
        {
            if (Entity == null)
                return;

            if (string.IsNullOrEmpty(Header))
            {
                Header = "Στοιχεία Λογαριασμού Χρήστη";
            }

            txtUserName.Text = Entity.Username;
            txtEmail.Text = Entity.Email;            
        }

        #endregion

        #region [ Helper Methods ]

        public string CreateUser()
        {
            try
            {
                MembershipCreateStatus status;
                MembershipUser mu;

                mu = Membership.CreateUser(txtUserName.Text.Trim(), txtPassword.Text, txtEmail.Text, null, null, true, out status);

                if (mu == null)
                    throw new MembershipCreateUserException(status);

                _CreatedUser = mu;
                return mu.UserName;
            }
            catch (MembershipCreateUserException)
            {
                throw;
            }
        }

        #endregion

        #region [ Properties ]

        public bool ReadOnly
        {
            set
            {
                foreach (var control in this.RecursiveOfType<ASPxEdit>())
                {
                    control.ReadOnly = value;
                    control.ClientEnabled = !value;
                }
            }
        }

        public bool ShowHintPopup
        {
            set
            {
                if (!value)
                {
                    txtUserName.CssClass = txtUserName.CssClass.Replace("hint", "");
                    txtPassword.CssClass = txtPassword.CssClass.Replace("hint", "");
                    txtEmail.CssClass = txtEmail.CssClass.Replace("hint", "");                    
                }
            }
        }

        public string EmailInfo
        {
            set
            {
                ltrEmailInfo.Text = value;
            }
        }

        public string LabelWidth { get; set; }
        public string Header { get; set; }

        public bool HideConfirmationInfoFields
        {
            set
            {
                trPasswordInfo.Visible =
                trEmailInfo.Visible = !value;
            }
        }

        public bool HideConfirmationFields
        {
            set
            {
                trPasswordInfo.Visible =
                trEmailConfirmation.Visible = !value;
            }
        }

        MembershipUser _CreatedUser = null;
        public string ProviderUserKey
        {
            get
            {
                if (_CreatedUser == null)
                    throw new InvalidOperationException("No MembershipUser was found. Please check CreateUser() or SetUser().");
                return _CreatedUser.ProviderUserKey.ToString();
            }
        }

        #endregion

        #region [ Validation ]

        public string ValidationGroup
        {
            get
            {
                return txtEmail.ValidationSettings.ValidationGroup;
            }
            set
            {
                foreach (var control in this.RecursiveOfType<ASPxEdit>())
                    control.ValidationSettings.ValidationGroup = value;
            }
        }

        protected void txtUserName_Validation(object sender, DevExpress.Web.ValidationEventArgs e)
        {
            e.IsValid = !new PortalServices.Services().UserNameExists(e.Value.ToString());
            e.ErrorText = "Το Όνομα Χρήστη χρησιμοποιείται";
        }

        protected void txtEmail_Validation(object sender, DevExpress.Web.ValidationEventArgs e)
        {
            e.IsValid = !new PortalServices.Services().EmailExists(e.Value.ToString());
            e.ErrorText = "Το Email χρησιμοποιείται";
        }

        #endregion
    }
}