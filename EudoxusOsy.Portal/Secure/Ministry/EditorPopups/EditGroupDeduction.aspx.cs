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
    public partial class EditGroupDeduction : BaseEntityPortalPage<CatalogGroup>
    {
        #region [ Entity Fill ]

        protected override void Fill()
        {
            int groupID;
            if (int.TryParse(Request.QueryString["id"], out groupID) && groupID > 0)
            {
                Entity = new CatalogGroupRepository(UnitOfWork).Load(groupID, x => x.Deduction);
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "hidePopup", "window.parent.popUp.hide();", true);
            }
        }

        #endregion

        #region [ Control Inits ]

        protected void ddlNewDeductionType_Init(object sender, EventArgs e)
        {
            ddlNewDeductionType.FillFromEnum<enDeductionVatType>("-- επιλέξτε καθεστώς Φ.Π.Α. --");
        }

        protected void cbpNewVat_Callback(object source, CallbackEventArgsBase e)
        {
            int deductionType;
            if (int.TryParse(e.Parameter, out deductionType) && deductionType > 0)
            {
                pcNewVat.Visible = deductionType == (int)enDeductionVatType.Custom;
                txtNewVat.Value = null;
            }
        }

        #endregion

        #region [ Page Inits ]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lblCurrentDeductionType.Text = Entity.DeductionID.HasValue
                                                ? Entity.Deduction.VatType.GetLabel()
                                                : enDeductionVatType.Custom.GetLabel();

                if (!Entity.DeductionID.HasValue)
                {
                    trCurrentVat.Visible = true;
                    lblCurrentVat.Text = string.Format("{0:C}", Entity.Vat);
                }
            }
        }

        #endregion

        #region [ Button Handlers ]

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ASPxEdit.ValidateEditorsInContainer(Page, "vgGroupDeduction"))
                return;

            enDeductionVatType newDeductionType = (enDeductionVatType)ddlNewDeductionType.GetSelectedInteger().Value;

            if (newDeductionType != enDeductionVatType.Custom)
            {
                Entity.DeductionID = CatalogGroupHelper.FindDeductionFromDeductionVatType(newDeductionType).ID;
                Entity.Vat = null;
            }
            else
            {
                Entity.DeductionID = null;
                Entity.Vat = (decimal)txtNewVat.Value;
            }

            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }

        #endregion
    }
}