using DevExpress.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;

namespace EudoxusOsy.Portal.Secure.EditorPopups
{
    public partial class AddInvoice : BaseSecureEntityPortalPage<Invoice>
    {
        #region [ Entity Fill ]

        protected CatalogGroup CurrentGroup;

        protected override void Fill()
        {
            int groupID;
            if (int.TryParse(Request.QueryString["id"], out groupID) && groupID > 0)
            {
                Entity = new Invoice()
                {
                    GroupID = groupID,
                    IsActive = true
                };

                CurrentGroup = new CatalogGroupRepository(UnitOfWork).Load(groupID, x => x.Supplier);
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsAuthorized)
            {
                ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
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

                UnitOfWork.MarkAsNew(Entity);
                UnitOfWork.Commit();

                ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
            }
        }

        #endregion
    }
}