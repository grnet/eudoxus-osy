using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.Portal.Controls;
using EudoxusOsy.BusinessModel;

namespace EudoxusOsy.Portal.UserControls.SupplierControls.ViewControls
{
    public partial class SupplierMinistryView : BaseEntityUserControl<Supplier>
    {
        public override void Bind()
        {
            if (Entity == null)
                return;

            if (Entity.SupplierType != enSupplierType.SelfPublisher)
            {
                chkNoLogisticBooks.ClientEnabled = false;
            }

            lblSupplierKpsID.Text = Entity.SupplierKpsID.ToString();
            lblSupplierType.Text = Entity.SupplierType.GetLabel();
            lblSupplierName.Text = Entity.Name;
            lblSupplierAFM.Text = Entity.AFM;
            lblTradeName.Text = Entity.TradeName;
            //lblDOY.Text = Entity.DOY;

            if (Entity.PaymentPfoID.HasValue && Entity.PaymentPfoID != EudoxusOsyConstants.FOREIGN_PFO_ID)
            {
                lblPaymentPfo.Text = EudoxusOsyCacheManager<PublicFinancialOffice>.Current.Get(Entity.PaymentPfoID.Value).Name;
            }
            else
            {
                lblPaymentPfo.Text = Entity.PaymentPfo;
            }

            if (Entity.SupplierIBANs != null && Entity.SupplierIBANs.Count > 0 && Entity.SupplierIBANs.First() != null)
            {
                lblIBAN.Text = Entity.SupplierIBANs.OrderByDescending(x => x.CreatedAt).First().IBAN;
            }

            lblAddress.Text = Entity.SupplierDetail.PublisherAddress;
            lblZipCode.Text = Entity.SupplierDetail.PublisherZipCode;
            lblContactName.Text = Entity.Reporter.ContactName;
            lblSupplierPhone.Text = Entity.SupplierDetail.PublisherPhone;
            lblSupplierFax.Text = Entity.SupplierDetail.PublisherFax;
            lblSupplierEmail.Text = Entity.SupplierDetail.PublisherEmail;
            lblSupplierUrl.Text = Entity.SupplierDetail.PublisherUrl;
            chkNoLogisticBooks.Checked = Entity.HasLogisticBooks.HasValue ? !Entity.HasLogisticBooks.Value : false;
        }

        public SupplierChanges Extract()
        {
            return new SupplierChanges()
            {
                NoLogisticBooks = chkNoLogisticBooks.Checked,
                //FileID = (int?)ucUploadFiles.ExtractValue()
            };
        }

        public void BindFile(File file)
        {
            //ucUploadFiles.BindFile(file);
        }
    }

    public class SupplierChanges
    {
        public bool NoLogisticBooks { get; set; }
        public int? FileID { get; set; }
    }
}