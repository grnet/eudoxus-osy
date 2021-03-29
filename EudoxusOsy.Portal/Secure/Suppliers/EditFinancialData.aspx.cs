using DevExpress.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Linq;
using System.Web.UI;

namespace EudoxusOsy.Portal.Secure.Suppliers
{
    public partial class EditFinancialData : BaseEntityPortalPage<Supplier>
    {
        #region [ Control Inits ]

        protected void ddlPaymentPfo_Init(object sender, EventArgs e)
        {
            ddlPaymentPfo.FillPfos();
        }

        protected void cbpForeignPfo_Callback(object source, CallbackEventArgsBase e)
        {
            int pfoID;
            if (int.TryParse(e.Parameter, out pfoID) && pfoID >= -1)
            {
                ShowForeignPfo(pfoID);
            }
        }

        #endregion

        #region [ Entity Fill ]

        protected override void Fill()
        {
            Entity = new SupplierRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name,
                                                                        x => x.Reporter,
                                                                        x => x.SupplierIBANs);
            Entity.SaveToCurrentContext();
        }
        #endregion

        #region [ Page Inits ]

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Entity.PaymentPfoID.HasValue)
                {
                    ddlPaymentPfo.SelectedItem = ddlPaymentPfo.Items.FindByValue(Entity.PaymentPfoID.Value);

                    if (Entity.PaymentPfoID == EudoxusOsyConstants.FOREIGN_PFO_ID)
                    {
                        txtForeignPfo.Text = Entity.PaymentPfo;
                    }
                }

                txtIBAN.Text = Entity.SupplierIBANs.OrderByDescending(x => x.ID).FirstOrDefault() != null
                                ? Entity.SupplierIBANs.OrderByDescending(x => x.ID).FirstOrDefault().IBAN
                                : null;
                                       
            }

            if (CurrentIBAN != null && CurrentIBAN.IBANCertificateID != null)
            {
                ucFileUpload.BindFile(CurrentIBAN.IBANCertificateID.Value);
            }

            if (Entity.PaymentPfoID.HasValue)
            {
                ShowForeignPfo(Entity.PaymentPfoID.Value);
            }
        }

        #endregion

        #region [ Button Handlers ]

        protected void btnSavePaymentPfo_Click(object sender, EventArgs e)
        {
            var selectedInteger = ddlPaymentPfo.GetSelectedInteger();

            if (!((selectedInteger != null && selectedInteger.Value == -1 && ASPxEdit.ValidateEditorsInContainer(Page, "vgForeignPfo")) ||
                                                                ASPxEdit.ValidateEditorsInContainer(Page, "vgPaymentPfo")))
                return;

            int? pfoID = selectedInteger != null ? (int?)selectedInteger.Value : null;

            Entity.PaymentPfoID = pfoID;

            if (Entity.PaymentPfoID == EudoxusOsyConstants.FOREIGN_PFO_ID)
            {
                Entity.PaymentPfo = txtForeignPfo.GetText();
            }
            else
            {
                Entity.PaymentPfo = null;
            }

            UnitOfWork.Commit();

            RedirectAndNotify(Request.RawUrl, "Η ενημέρωση της Δ.Ο.Υ. Πληρωμών πραγματοποιήθηκε επιτυχώς");
        }

        protected void btnSaveIBAN_Click(object sender, EventArgs e)
        {           
            if (!ASPxEdit.ValidateEditorsInContainer(Page, "vgIBAN"))
                return;

            var iban = txtIBAN.GetText();

            enIBANValidationResult validationResult = ValidationHelper.ValidateIBAN(iban);

            if (validationResult == enIBANValidationResult.NotGreekIBAN)
            {
                lblError.Text = "Επιτρέπονται μόνο ελληνικοί IBAN";
                return;
            }

            if (validationResult != enIBANValidationResult.IsValid)
            {
                lblError.Text = "Η μορφή του IBAN δεν είναι έγκυρη";
                return;
            }

            int? fileID = (int?) ucFileUpload.ExtractValue();

            if (CurrentIBAN != null)
            {
                CurrentIBAN.IBANCertificateID = fileID;
            }

            if (CurrentIBAN == null || CurrentIBAN.IBAN != iban)
            {
                var supplierIBAN = new SupplierIBAN()
                {
                    SupplierID = Entity.ID,
                    IBAN = iban,
                    IBANCertificateID = fileID,
                    CreatedAt = DateTime.Now,
                    CreatedBy = Page.User.Identity.Name
                };

                UnitOfWork.MarkAsNew(supplierIBAN);                

                if (Entity.SupplierIBANs.Count > 1)
                {
                    string ibanChanges = string.Format("Ο διαθέτης με ID {0} έχει προβεί στις ακόλουθες τροποποιήσεις του IBAN:\n\n", Entity.SupplierKpsID);

                    foreach (var ibanChange in Entity.SupplierIBANs)
                    {
                        ibanChanges += string.Format("{0:dd/MM/yyyy HH:mm} - > {1}\n", ibanChange.CreatedAt, ibanChange.IBAN);
                    }

                    var email = EmailFactory.GetIBANChanges(Entity.Reporter, ibanChanges);
                    UnitOfWork.MarkAsNew(email);                    

                    EmailQueueWorker.Current.AddEmailDispatchToQueue(email.ID);
                }            
            }

            UnitOfWork.Commit();

            if (fileID != null)
            {
                ucFileUpload.BindFile(fileID.Value);
            }            
            
            RedirectAndNotify(Request.RawUrl, "Η ενημέρωση πραγματοποιήθηκε επιτυχώς");
            
        }

        private SupplierIBAN _supplierIBAN;
        private SupplierIBAN CurrentIBAN
        {
            get
            {                
                if (_supplierIBAN == null && Entity.SupplierIBANs != null)
                {
                    _supplierIBAN = Entity.SupplierIBANs.OrderByDescending(x => x.ID).FirstOrDefault();
                }
                
                return _supplierIBAN;
            }
        }

        #endregion

        #region [ Helper Methods ]

        private void ShowForeignPfo(int pfoID)
        {
            pcForeignPfo.Visible = pfoID == EudoxusOsyConstants.FOREIGN_PFO_ID;

            cbpForeignPfo.DataBind();
        }

        #endregion
    }
}