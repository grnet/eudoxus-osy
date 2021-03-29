using DevExpress.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Web.UI;

namespace EudoxusOsy.Portal.Secure.EditorPopups
{
    public partial class EditInvoice : BaseSecureEntityPortalPage<Invoice>
    {
        protected CatalogGroup CurrentGroup;

        #region [ Entity Fill ]

        protected override void Fill()
        {
            int invoiceID;
            if (int.TryParse(Request.QueryString["id"], out invoiceID) && invoiceID > 0)
            {
                Entity = new InvoiceRepository(UnitOfWork).Load(invoiceID);
                if (Entity.GroupID > 0)
                {
                    CurrentGroup = new CatalogGroupRepository(UnitOfWork).Load(Entity.GroupID, x => x.Supplier);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "hidePopup", "window.parent.popUp.hide();", true);
            }
        }

        protected override bool Authorize()
        {
            return CurrentGroup.Supplier.ReporterID == User.Identity.ReporterID
                || EudoxusOsyRoleProvider.IsAuthorizedEditorUser();
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

            if (CatalogGroupHelper.CanAddInvoice(CurrentGroup, User) && IsAuthorized)
            {

                ucInvoiceInput.Fill(Entity);

                UnitOfWork.Commit();

                ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
            }
        }

        #endregion
    }
}