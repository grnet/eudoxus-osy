using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EudoxusOsy.Portal.Secure.EditorPopups
{
    public partial class EditVatDataPopup : BaseSecureEntityPortalPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsSecure)
            {
                ClientScript.RegisterStartupScript(GetType(), "closePopup", "window.parent.popUp.hide();", true);
            }

            if (!IsPostBack)
            {
                var deductions = new DeductionRepository(UnitOfWork).FindActive();

                txtVatHigh.Text = deductions.FirstOrDefault(x => x.VatType == enDeductionVatType.High).Vat.ToString();
                txtVatMedium.Text = deductions.FirstOrDefault(x => x.VatType == enDeductionVatType.Medium).Vat.ToString();
                txtVatSmall.Text = deductions.FirstOrDefault(x => x.VatType == enDeductionVatType.Small).Vat.ToString();
                txtVatHighLowered.Text = deductions.FirstOrDefault(x => x.VatType == enDeductionVatType.HighLowered).Vat.ToString();
                txtVatMediumLowered.Text = deductions.FirstOrDefault(x => x.VatType == enDeductionVatType.MediumLowered).Vat.ToString();
                txtVatSmallLowered.Text = deductions.FirstOrDefault(x => x.VatType == enDeductionVatType.SmallLowered).Vat.ToString();
                txtOGA.Text = deductions.FirstOrDefault().OgaPercentage.ToString();
                txtMTPY.Text = deductions.FirstOrDefault().MtpyPercentage.ToString();
            }
        }

        protected override bool Authenticate()
        {
            return EudoxusOsyRoleProvider.IsAuthorizedEditorUser();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var deductions = new DeductionRepository(UnitOfWork).FindActive();
            foreach (var deduction in deductions)
            {
                deduction.EndDate = DateTime.Today;
                deduction.UpdatedAt = DateTime.Today;
                deduction.UpdatedBy = User.Identity.Name;
                deduction.IsActive = false;

                var newDeduction = new Deduction()
                {
                    IsActive = true,
                    StartDate = DateTime.Today,
                    CreatedAt = DateTime.Today,
                    CreatedBy = User.Identity.Name,
                    VatType = deduction.VatType
                };

                switch (deduction.VatType)
                {
                    case enDeductionVatType.High:
                        newDeduction.Vat = Convert.ToDecimal(txtVatHigh.Text);
                        break;
                    case enDeductionVatType.Medium:
                        newDeduction.Vat = Convert.ToDecimal(txtVatMedium.Text);
                        break;
                    case enDeductionVatType.Small:
                        newDeduction.Vat = Convert.ToDecimal(txtVatSmall.Text);
                        break;
                    case enDeductionVatType.HighLowered:
                        newDeduction.Vat = Convert.ToDecimal(txtVatHighLowered.Text);
                        break;
                    case enDeductionVatType.MediumLowered:
                        newDeduction.Vat = Convert.ToDecimal(txtVatMediumLowered.Text);
                        break;
                    case enDeductionVatType.SmallLowered:
                        newDeduction.Vat = Convert.ToDecimal(txtVatSmallLowered.Text);
                        break;
                }
                newDeduction.OgaPercentage = Convert.ToDecimal(txtOGA.Text);
                newDeduction.MtpyPercentage = Convert.ToDecimal(txtMTPY.Text);

                UnitOfWork.MarkAsNew(newDeduction);
            }

            UnitOfWork.Commit();
            ClientScript.RegisterStartupScript(GetType(), "closePopup", "window.parent.popUp.hide();", true);
        }
    }
}