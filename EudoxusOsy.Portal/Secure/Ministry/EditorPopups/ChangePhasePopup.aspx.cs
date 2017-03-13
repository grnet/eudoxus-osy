using EudoxusOsy.BusinessModel;
using EudoxusOsy.BusinessModel.Flow;
using EudoxusOsy.Portal.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EudoxusOsy.Portal.Secure.Ministry.EditorPopups
{
    public partial class ChangePhasePopup : BaseSecureEntityPortalPage<Catalog>
    {
        protected int CatalogID
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["id"]);
            }
        }

        protected override void Fill()
        {
            Entity = new CatalogRepository(UnitOfWork).Load(CatalogID);
        }

        protected override bool Authorize()
        {
            return User.IsInRole(RoleNames.Helpdesk) || User.IsInRole(RoleNames.SuperHelpdesk) || User.IsInRole(RoleNames.SystemAdministrator);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsAuthorized)
            {
                ClientScript.RegisterStartupScript(GetType(), "closePopup", "window.parent.popUp.hide();", true);
            }
       }

        protected void btnSubmitHidden_Click(object sender, EventArgs e)
        {
            if (Entity.PhaseID < Convert.ToInt32(txtNewPhaseID.Text) && Entity.State != enCatalogState.FromMove && Entity.Amount < 0 && Entity.GroupID == null)
            {
                var catalogCopy = new Catalog();
                catalogCopy.Status = Entity.Status;
                catalogCopy.SupplierID = Entity.SupplierID;
                catalogCopy.DepartmentID = Entity.DepartmentID;
                catalogCopy.PhaseID = Convert.ToInt32(txtNewPhaseID.Text);
                catalogCopy.State = enCatalogState.FromMove;
                catalogCopy.BookID = Entity.BookID;
                catalogCopy.BookPriceID = Entity.BookPriceID;
                catalogCopy.CreatedAt = DateTime.Today;
                catalogCopy.CreatedBy = User.Identity.Name;
                catalogCopy.DiscountID = Entity.DiscountID;
                catalogCopy.Percentage = Entity.Percentage;

                UnitOfWork.MarkAsNew(catalogCopy);

                Entity.State = enCatalogState.Moved;
                UnitOfWork.Commit();
                ClientScript.RegisterStartupScript(GetType(), "closePopup", "window.parent.cmdRefresh();window.parent.popUp.hide();", true);
            }
        }
    }
}