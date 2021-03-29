using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace EudoxusOsy.Portal.Secure.Ministry
{
    public partial class SearchSuppliers : BaseEntityPortalPage
    {
        #region [ Button Handlers ]

        protected void btnExportSuppliers_Click(object sender, EventArgs e)
        {
            var criteria = new Criteria<Supplier>();
            criteria.Expression = ucSearchFilters.GetSearchFilters().GetExpression();

            if (criteria.Expression == null)
            {
                criteria.Expression = Imis.Domain.EF.Search.Criteria<Supplier>.Empty;
            }

            int count = 0;
            criteria.UsePaging = false;
            criteria.Include(x => x.Reporter);
            criteria.Include(x => x.SupplierDetail);
            criteria.Include(x => x.SupplierIBANs);
            var suppliers = new SupplierRepository(UnitOfWork).FindWithCriteria(criteria, out count);

            var suppliersExport = new List<SupplierExportInfo>();

            foreach (var supplier in suppliers)
            {
                var sup = new SupplierExportInfo();

                try
                {
                    sup.SupplierKpsID = supplier.SupplierKpsID;
                    sup.Name = supplier.Name;
                    sup.TradeName = supplier.TradeName;
                    sup.AFM = supplier.AFM;
                    sup.ContactName = supplier.Reporter.ContactName;
                    sup.SupplierType = supplier.SupplierType.GetLabel();
                    sup.SupplierStatus = supplier.Status.GetLabel();
                    sup.Address = supplier.SupplierDetail.PublisherAddress;
                    sup.DOY = supplier.DOY;
                    sup.PaymentDOY = supplier.PaymentPfoID.HasValue
                        ? EudoxusOsyCacheManager<PublicFinancialOffice>.Current.Get(supplier.PaymentPfoID.Value).Name
                        : supplier.PaymentPfo;
                    sup.Telephone = supplier.SupplierDetail.PublisherPhone;
                    sup.Email = supplier.SupplierDetail.PublisherEmail;
                    sup.Fax = supplier.SupplierDetail.PublisherFax;
                    sup.ZipCode = supplier.SupplierDetail.PublisherZipCode;
                    sup.IBAN = supplier.SupplierIBANs != null && supplier.SupplierIBANs.Count > 0
                        ? supplier.SupplierIBANs.OrderByDescending(y => y.CreatedAt).First().IBAN
                        : string.Empty;
                    sup.Url = supplier.SupplierDetail.PublisherUrl;
                    sup.NoLogisticBooks = supplier.SupplierType != enSupplierType.SelfPublisher
                        ? "ΟΧΙ"
                        : (supplier.HasLogisticBooks == true ? "ΟΧΙ" : "ΝΑΙ");

                    suppliersExport.Add(sup);
                }
                catch (Exception ex)
                {

                }
            }

            gvSuppliersExport.Export(suppliersExport, "suppliers_info");
        }

        #endregion

        #region [ DataSource Events ]

        protected void odsSuppliers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<Supplier> criteria = new Criteria<Supplier>();

            criteria.Include(x => x.Reporter)
                    .Include(x => x.SupplierDetail)
                    .Include(x => x.SupplierIBANs);

            criteria.Sort.OrderBy(x => x.ID);

            var filters = ucSearchFilters.GetSearchFilters();
            var exp = filters.GetExpression();
            if (exp != null)
                criteria.Expression = criteria.Expression.And(exp);

            e.InputParameters["criteria"] = criteria;
        }

        #endregion

        #region [ GridView Events ]

        protected void gvSuppliers_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();

            if (action == "refresh")
            {
                gvSuppliers.DataBind();
                return;
            }

            gvSuppliers.DataBind();
        }

        #endregion

        #region [ GridView Methods ]

        protected string GetIbanChangeCount(Supplier supplier)
        {
            if (supplier == null || supplier.SupplierIBANs == null)
                return string.Empty;

            return supplier.SupplierIBANs.Count().ToString();
        }

        protected bool HasIBANCertificate(Supplier supplier)
        {            
            return supplier != null && supplier.CurrentIBAN != null && supplier.CurrentIBAN.IBANCertificateID != null;
        }

        #endregion

        protected bool CanChangeIBAN(Supplier containerDataItem)
        {
            var roles = Roles.GetRolesForUser(User.Identity.Name);

            return !roles.Contains(RoleNames.MinistryAuditor);
        }
    }
}