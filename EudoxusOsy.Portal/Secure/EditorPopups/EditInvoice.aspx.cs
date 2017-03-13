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

namespace EudoxusOsy.Portal.Secure.EditorPopups
{
    public partial class EditInvoice : BaseEntityPortalPage<Invoice>
    {
        #region [ Entity Fill ]

        protected override void Fill()
        {
            int invoiceID;
            if (int.TryParse(Request.QueryString["id"], out invoiceID) && invoiceID > 0)
            {
                Entity = new InvoiceRepository(UnitOfWork).Load(invoiceID);
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "hidePopup", "window.parent.popUp.hide();", true);
            }
        }

        #endregion

        #region [ Page Inits ]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ucInvoiceInput.Entity = Entity;
                ucInvoiceInput.Bind();
            }
        }

        #endregion

        #region [ Button Handlers ]

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ASPxEdit.ValidateEditorsInContainer(Page, "vgInvoice"))
                return;

            ucInvoiceInput.Fill(Entity);
            
            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }

        #endregion
    }
}