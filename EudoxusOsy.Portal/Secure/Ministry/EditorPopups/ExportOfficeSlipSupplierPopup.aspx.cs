using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;

namespace EudoxusOsy.Portal.Secure.Ministry.EditorPopups
{
    public partial class ExportOfficeSlipSupplierPopup : BaseSecureEntityPortalPage
    {

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
        }

        protected void btnSubmitHidden_Click(object sender, EventArgs e)
        {
            var currentSupplier = new SupplierRepository().FindByKpsID(int.Parse(txtSupplierKpsID.Text), x => x.SupplierIBANs);
            var paymentOrders = new PaymentOrderRepository(UnitOfWork).FindSentByOfficeSlipDateWithInvoices((DateTime)dateSentAt.Value, currentSupplier.ID);
            int paymentOrdersCount = 0;
            if (paymentOrders != null && paymentOrders.Count > 0)
            {
                paymentOrdersCount = paymentOrders.Count;
            }

            if (paymentOrdersCount > 0 && currentSupplier != null)
            {
                Response.Redirect(string.Format("~/Secure/GenerateOfficeSlipSupplierExcel.ashx?year={0}&month={1}&date={2}&SupplierKpsID={3}",
                    ((DateTime)dateSentAt.Value).Year, ((DateTime)dateSentAt.Value).Month, ((DateTime)dateSentAt.Value).Day, txtSupplierKpsID.Text), true);
                ClientScript.RegisterStartupScript(GetType(), "closePopup", "window.parent.cmdRefresh();window.parent.popUp.hide();", false);
            }
            else
            {
                var cstext = "alert('Δεν βρέθηκαν εγγραφές προς εξαγωγή για τον συνδιασμό ημερομηνίας αποστολής προς ΥΔΕ/ID εκδότη.');";
                ClientScript.RegisterStartupScript(GetType(), "PopupScript", cstext, true);
            }
        }
    }
}