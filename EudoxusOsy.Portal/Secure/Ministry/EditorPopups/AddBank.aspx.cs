using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Imis.Domain;
using EudoxusOsy.Portal.Controls;
using System.Web.Security;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Utils;
using EudoxusOsy.Portal.Utils;
using DevExpress.Web;

namespace EudoxusOsy.Portal.Secure.Ministry.EditorPopups
{
    public partial class AddBank : BaseEntityPortalPage<Bank>
    {
        #region [ Entity Fill ]

        protected override void Fill()
        {
            Entity = new Bank()
            {
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = Page.User.Identity.Name
            };
        }

        #endregion

        #region [ Button Handlers ]

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ASPxEdit.ValidateEditorsInContainer(Page, "vgBank"))
                return;

            ucBankInput.Fill(Entity);

            UnitOfWork.MarkAsNew(Entity);
            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }

        #endregion
    }
}