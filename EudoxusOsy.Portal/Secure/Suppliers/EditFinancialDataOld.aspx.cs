using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EudoxusOsy.Portal.Secure.Suppliers
{
    public partial class EditFinancialData : BaseEntityPortalPage<Supplier>
    {
        protected PublicFinancialOffice _otherCountryOffice;
    

        protected PublicFinancialOffice OtherCountryOffice
        {
            get
            {
                if (_otherCountryOffice == null)
                {
                    _otherCountryOffice = EudoxusOsyCacheManager<PublicFinancialOffice>.Current.Get(-1);
                }

                return _otherCountryOffice;
            }
        }

        protected override void Fill()
        {
            Entity = new SupplierRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);
            Entity.SupplierIBANs.EnsureLoad();
            Entity.SaveToCurrentContext();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            cmbSelectOffice.DataSource = CacheManager.GetOrderedDOYs();
            cmbSelectOffice.DataBind();

            cmbSelectOffice.SelectedItem = cmbSelectOffice.Items.FindByValue(Entity.PfoID);
            if(Entity.PfoID == OtherCountryOffice.ID)
            {
                txtOtherOfficeDescription.ClientVisible = true;
                txtOtherOfficeDescription.Text = Entity.Pfo;
            }

            txtInsertIBAN.Text = Entity.SupplierIBANs.OrderByDescending(x=> x.CreatedAt).FirstOrDefault()  != null ? Entity.SupplierIBANs.OrderByDescending(x => x.CreatedAt).FirstOrDefault().IBAN: string.Empty;
        }

        protected void cbUpdateFinancialData_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            var parameters = e.Parameter.Split(':');

            var pfoID = Convert.ToInt32(parameters[0]);
            var iban = parameters[1];

            if (!ValidationHelper.CheckIBAN(iban))
            {
                lblError.Text = "Η μορφή του IBAN δεν είναι έγκυρη";
                return;
            }

            Entity.PfoID = pfoID;

            if (Entity.PfoID == OtherCountryOffice.ID)
            {
                Entity.Pfo = !string.IsNullOrEmpty(txtOtherOfficeDescription.Text) ? txtOtherOfficeDescription.Text : OtherCountryOffice.Name;
            }
            else
            {
                Entity.Pfo = string.Empty;
            }

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
    }
}