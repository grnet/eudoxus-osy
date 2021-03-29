using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;

namespace EudoxusOsy.Portal.Secure.Ministry.EditorPopups
{
    public partial class ExportOfficeSlipPopup : BaseSecureEntityPortalPage
    {
        protected bool IsPDF
        {
            get
            {
                return Request.QueryString["pdf"] != null;
            }
        }

        protected override bool Authenticate()
        {
            return EudoxusOsyRoleProvider.IsAuthorizedEditorUser();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsSecure)
            {
                ClientScript.RegisterStartupScript(GetType(), "closePopup", "window.parent.popUp.hide();", true);
            }

            if (IsPDF)
            {
                trDecision.Visible = false;
                trProtocol.Visible = true;
            }
            else
            {
                trDecision.Visible = false;
                trProtocol.Visible = false;
            }
        }

        protected void btnSubmitHidden_Click(object sender, EventArgs e)
        {
            var paymentOrders = new PaymentOrderRepository(UnitOfWork).FindSentByOfficeSlipDate((DateTime)dateSentAt.Value);

            if (!IsPDF)
            {
                ucOfficeSlipExportGridView.Export(paymentOrders, "OfficeSlip_" + ((DateTime)dateSentAt.Value).ToShortDateString());
                ClientScript.RegisterStartupScript(GetType(), "closePopup", "window.parent.cmdRefresh();window.parent.popUp.hide();", true);
            }
            else
            {
                Response.Redirect(string.Format("~/Secure/GenerateOfficeSlipPDF.ashx?year={0}&month={1}&date={2}&decision={3}&protocol={4}",
                    ((DateTime)dateSentAt.Value).Year, ((DateTime)dateSentAt.Value).Month, ((DateTime)dateSentAt.Value).Day, txtDecision.Text, txtProtocolNumber.Text), true);
            }
        }
    }
}