using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.Security;

namespace EudoxusOsy.Portal.Secure.Ministry
{
    public partial class SuppliersStats : BaseEntityPortalPage
    {
        #region [ Button Handlers ]

        protected void btnExportSuppliers_Click(object sender, EventArgs e)
        {
            //var criteria = new Criteria<Supplier>();
            //criteria.Expression = ucSearchFilters.GetSearchFilters().GetExpression();

            //if (criteria.Expression == null)
            //{
            //    criteria.Expression = Imis.Domain.EF.Search.Criteria<Supplier>.Empty;
            //}

            //int count = 0;
            //criteria.UsePaging = false;
            //criteria.Include(x => x.Reporter);
            //criteria.Include(x => x.SupplierDetail);
            //criteria.Include(x => x.SupplierIBANs);
            //var suppliers = new SupplierRepository(UnitOfWork).FindWithCriteria(criteria, out count);

            //var suppliersExport = suppliers.Select(x => new SupplierExportInfo()
            //{
            //    SupplierKpsID = x.SupplierKpsID,
            //    Name = x.Name,
            //    TradeName = x.TradeName,
            //    AFM = x.AFM,
            //    ContactName = x.Reporter.ContactName,
            //    SupplierType = x.SupplierType.GetLabel(),
            //    SupplierStatus = x.Status.GetLabel(),
            //    Address = x.SupplierDetail.PublisherAddress,
            //    DOY = x.DOY,
            //    PaymentDOY = x.PaymentPfoID.HasValue ? EudoxusOsyCacheManager<PublicFinancialOffice>.Current.Get(x.PaymentPfoID.Value).Name : x.PaymentPfo,
            //    Telephone = x.SupplierDetail.PublisherPhone,
            //    Email = x.SupplierDetail.PublisherEmail,
            //    Fax = x.SupplierDetail.PublisherFax,
            //    ZipCode = x.SupplierDetail.PublisherZipCode,
            //    IBAN = x.SupplierIBANs != null && x.SupplierIBANs.Count > 0 ? x.SupplierIBANs.OrderByDescending(y => y.CreatedAt).First().IBAN : string.Empty,
            //    Url = x.SupplierDetail.PublisherUrl,
            //    NoLogisticBooks = x.SupplierType != enSupplierType.SelfPublisher ? "ΟΧΙ" : (x.HasLogisticBooks == true ? "ΟΧΙ" : "ΝΑΙ")
            //});
            //gvSuppliersExport.Export(suppliersExport, "suppliers_stats");
        }

        #endregion

        #region [ DataSource Events ]

        //protected void odsSuppliers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        //{
        //    Criteria<Supplier> criteria = new Criteria<Supplier>();

        //    criteria.Include(x => x.Reporter)
        //            .Include(x => x.SupplierDetail)
        //            .Include(x => x.SupplierIBANs);

        //    criteria.Sort.OrderBy(x => x.ID);

        //    var filters = ucSearchFilters.GetSearchFilters();
        //    var exp = filters.GetExpression();
        //    if (exp != null)
        //        criteria.Expression = criteria.Expression.And(exp);

        //    e.InputParameters["criteria"] = criteria;
        //}

        #endregion

        #region [ GridView Events ]

        protected void gvSuppliersStats_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();

            if (action == "refresh")
            {
                gvSuppliersStats.DataBind();
                return;
            }

            gvSuppliersStats.DataBind();
        }

        #endregion

        #region [ GridView Methods ]

        #endregion

        protected void odsSuppliers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            var filters = ucSearchFilters.GetSearchFilters();

            e.InputParameters["supplierKpsID"] = filters.SupplierKpsID;
            e.InputParameters["afm"] = filters.SupplierAFM;
            e.InputParameters["supplierType"] = (int?)filters.SupplierType;
            e.InputParameters["name"] = filters.SupplierName;
        }
    }
}