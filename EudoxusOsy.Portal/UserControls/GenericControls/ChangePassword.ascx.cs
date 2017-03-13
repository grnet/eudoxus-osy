using System;
using System.ComponentModel;
using DevExpress.Web;

namespace EudoxusOsy.Portal.UserControls.GenericControls
{
    public partial class ChangePassword : System.Web.UI.UserControl
    {
        #region [ Properties ]

        [DefaultValue(true)]
        public bool RequestOldPassword
        {
            get { return trOldPassword.Visible; }
            set { trOldPassword.Visible = value; }
        }

        public string OldPassword
        {
            get { return txtOldPassword.GetText(); }
        }

        public string NewPassword
        {
            get { return txtNewPassword.GetText(); }
        }

        #endregion

        #region [ Validation ]

        public string ValidationGroup
        {
            get
            {
                return txtNewPassword.ValidationSettings.ValidationGroup;
            }
            set
            {
                foreach (var control in this.RecursiveOfType<ASPxEdit>())
                    control.ValidationSettings.ValidationGroup = value;
            }
        }

        #endregion
    }
}