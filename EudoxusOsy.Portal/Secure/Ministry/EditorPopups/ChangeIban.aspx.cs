using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using EudoxusOsy.Utils;

namespace EudoxusOsy.Portal.Secure.Ministry.EditorPopups
{
    public partial class ChangeIban : BaseEntityPortalPage<Supplier>
    {
        #region [ Entity Fill ]

        protected string CurrentIBAN 
        {
            get
            {
                var iban = Entity.SupplierIBANs.OrderByDescending(x => x.CreatedAt).FirstOrDefault();

                return iban != null
                        ? iban.IBAN
                        : null;
            }
        }

        protected override void Fill()
        {
            int supplierID;
            if (int.TryParse(Request.QueryString["id"], out supplierID) && supplierID > 0)
            {
                Entity = new SupplierRepository(UnitOfWork).Load(supplierID, x => x.SupplierIBANs);
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
            }
        }

        #endregion

        #region [ Page Inits ]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtIBAN.Text = CurrentIBAN;
            }
        }

        #endregion

        #region [ Button Handlers ]

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ASPxEdit.ValidateEditorsInContainer(this, "vgIBAN"))
                return;

            var iban = txtIBAN.GetText();

            if (!ValidationHelper.CheckIBAN(iban, false))
            {
                phErrors.Visible = true;
                lblErrors.Text = "Η μορφή του IBAN δεν είναι έγκυρη";
                return;
            }

            if (CurrentIBAN == null || CurrentIBAN != iban)
            {
                var supplierIBAN = new SupplierIBAN()
                {
                    SupplierID = Entity.ID,
                    IBAN = iban,
                    CreatedAt = DateTime.Now,
                    CreatedBy = Page.User.Identity.Name
                };

                UnitOfWork.MarkAsNew(supplierIBAN);
                UnitOfWork.Commit();
            }

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }

        #endregion
    }
}