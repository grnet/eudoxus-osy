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
    public partial class AddTransfer : BaseEntityPortalPage<BankTransfer>
    {
        #region [ Entity Fill ]

        protected override void Fill()
        {   
            int supplierID;
            if (int.TryParse(Request.QueryString["id"], out supplierID) && supplierID > 0)
            {
                Entity = new BankTransfer()
                {   
                    SupplierID = supplierID,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    CreatedBy = Page.User.Identity.Name
                };
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "hidePopup", "window.parent.popUp.hide();", true);
            }
        }

        #endregion

        #region [ Button Handlers ]

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ASPxEdit.ValidateEditorsInContainer(Page, "vgTransfer"))
                return;
            
            ucTransferInput.Fill(Entity);

            UnitOfWork.MarkAsNew(Entity);
            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }

        #endregion
    }
}