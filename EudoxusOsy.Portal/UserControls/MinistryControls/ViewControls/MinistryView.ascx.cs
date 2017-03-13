using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.Portal.Controls;
using EudoxusOsy.BusinessModel;

namespace EudoxusOsy.Portal.UserControls.MinistryControls.ViewControls
{
    public partial class MinistryView : BaseEntityUserControl<Reporter>
    {
        public override void Bind()
        {
            if (Entity == null)
                return;

            lblContactName.Text = Entity.ContactName;
            lblContactPhone.Text = Entity.ContactPhone;            
            lblContactEmail.Text = Entity.ContactEmail;
            lblAuthorizationType.Text = Entity.AuthorizationType.GetLabel();
        }
    }
}