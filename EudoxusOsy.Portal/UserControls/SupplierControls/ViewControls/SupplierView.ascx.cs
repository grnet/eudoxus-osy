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
    public partial class SupplierView : BaseEntityUserControl<Supplier>
    {
        public override void Bind()
        {
            if (Entity == null)
                return;

            var sd = Entity.SupplierDetail;

            if (Entity.SupplierType == enSupplierType.SelfPublisher)
            {
                if (Entity.HasLogisticBooks.HasValue && Entity.HasLogisticBooks.Value)
                {
                    lblSupplierType.Text = "Φυσικό Πρόσωπο - Υπόχρεος τήρησης λογιστικών βιβλίων";
                }
                else
                {
                    lblSupplierType.Text = "Φυσικό Πρόσωπο - Μη υπόχρεος τήρησης λογιστικών βιβλίων";
                }
            }
            else
            {
                lblSupplierType.Text = Entity.SupplierType.GetLabel();
            }
            
            lblSupplierName.Text = Entity.Name;
            lblTradeName.Text = Entity.TradeName;
            lblSupplierAFM.Text = Entity.AFM;
            lblAddress.Text = sd.PublisherAddress;
            lblZipCode.Text = sd.PublisherZipCode;
            
            if (sd.PublisherCityID.HasValue)
            {
                lblCity.Text = CacheManager.Cities.Get(sd.PublisherCityID.Value).Name;
            }

            if (sd.PublisherPrefectureID.HasValue)
            {
                lblPrefecture.Text = CacheManager.Prefectures.Get(sd.PublisherPrefectureID.Value).Name;
            }

            lblContactName.Text = Entity.Reporter.ContactName;
            lblSupplierPhone.Text = Entity.SupplierDetail.PublisherPhone;
            lblSupplierMobilePhone.Text = Entity.SupplierDetail.PublisherMobilePhone;
            lblSupplierEmail.Text = Entity.SupplierDetail.PublisherEmail;
            lblSupplierUrl.Text = Entity.SupplierDetail.PublisherUrl;
        }
    }
}