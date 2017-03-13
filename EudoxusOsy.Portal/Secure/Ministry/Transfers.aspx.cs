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
    public partial class Transfers : BaseEntityPortalPage
    {
        #region [ DataSource Events ]

        protected void odsSuppliers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<Supplier> criteria = new Criteria<Supplier>();

            criteria.Include(x => x.Reporter)
                    .Include(x => x.SupplierDetail);

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

        #region [ Events ]


        #endregion

        protected void gvTransfers_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();

            if (action == "refresh")
            {
                gvTransfers.DataBind();
                return;
            }
        }

        protected void odsTransfers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<BankTransfer> criteria = new Criteria<BankTransfer>();

            var filters = ucTransferSearchFilters.GetSearchFilters();
            criteria.Expression = filters.GetExpression();

            if (criteria.Expression == null)
            {
                criteria.Expression = Imis.Domain.EF.Search.Criteria<BankTransfer>.Empty;
            }

            criteria.Include(x => x.Bank)
                    .Include(x => x.Supplier)
                    .Include(x => x.Phase);

            criteria.Sort.OrderByDescending(x => x.ID);



            criteria.Expression = criteria.Expression.Where(x => x.IsActive, true);

            e.InputParameters["criteria"] = criteria;
        }

        protected void btnExportTransfers_Click(object sender, EventArgs e)
        {
            var criteria = new Criteria<BankTransfer>();

            var filters = ucTransferSearchFilters.GetSearchFilters();
            criteria.Expression = filters.GetExpression();

            if (criteria.Expression == null)
            {
                criteria.Expression = Imis.Domain.EF.Search.Criteria<BankTransfer>.Empty;
            }

            criteria.Include(x => x.Bank)
                    .Include(x => x.Supplier)
                    .Include(x => x.Phase);

            criteria.Sort.OrderByDescending(x => x.ID);
            criteria.UsePaging = false;
            criteria.Expression = criteria.Expression.Where(x => x.IsActive, true);
            int recordCount = 0;
            var transfers = new BankTransferRepository(UnitOfWork).FindWithCriteria(criteria, out recordCount);

            gvTransfers.Export(transfers, "transfers");
        }
    }
}