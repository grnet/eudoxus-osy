﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System.Drawing;
using Imis.Domain;
using DevExpress.Web;

namespace EudoxusOsy.Portal.Secure.Ministry
{
    public partial class EditCatalogGroup : BaseEntityPortalPage<CatalogGroupInfo>
    {
        #region [ Entity Fill ]

        protected int SelectedPhaseID;
        private List<Catalog> _connectedCatalogs = new List<Catalog>();

        protected override void Fill()
        {
            int catalogGroupID;
            if (int.TryParse(Request.QueryString["gID"], out catalogGroupID))
            {
                using (UnitOfWork.SingleConnection())
                {   
                    LoadEntities(catalogGroupID);
                }
            }

            int.TryParse(Request.QueryString["pID"], out SelectedPhaseID);            

            if (Entity == null || SelectedPhaseID <= 0)
            {
                Response.Redirect("PaymentOrders.aspx");
            }
        }

        private void LoadEntities(int catalogGroupID)
        {
            Entity = new CatalogGroupRepository(UnitOfWork).GetByID(catalogGroupID);

            if (Entity != null)
            {
                _connectedCatalogs = new CatalogRepository(UnitOfWork).FindConnectedCatalogsByGroupID(Entity.ID);
            }
        }

        #endregion

        #region [ Page Inits ]

        protected void Page_Init(object sender, EventArgs e)
        {
            ((Ministry)Page.Master).HideSiteMap = true;
            BindGroupInfo();
            BindConnectedCatalogs();

            ucCatalogSearchFilters.SetInstitution(Entity.InstitutionID);
        }

        private void Bind()
        {
            BindGroupInfo();
            BindConnectedCatalogs();
            BindNotConnectedCatalogs();
        }

        private void BindGroupInfo()
        {
            lblGroupID.Text = Entity.ID.ToString();
            lblInstitution.Text = CacheManager.Institutions.Get(Entity.InstitutionID).Name;
            lblCatalogCount.Text = Entity.CatalogCount.ToString();
            lblTotalAmount.Text = string.Format("{0:C}", Entity.TotalAmount);
        }

        private void BindConnectedCatalogs()
        {
            gvConnectedCatalogs.DataSource = _connectedCatalogs;
            gvConnectedCatalogs.DataBind();
        }

        private void BindNotConnectedCatalogs()
        {
            gvNotConnectedCatalogs.DataBind();
        }

        #endregion

        #region [ Button Clicks ]

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("PaymentOrders.aspx?sID={0}&pID={1}&t=true", Entity.SupplierID, SelectedPhaseID));
        }

        protected void btnAddToGroup_Click(object sender, EventArgs e)
        {
            var catalogID = int.Parse(hfCatalogID.Value);
            var catalog = new CatalogRepository(UnitOfWork).Load(catalogID);

            catalog.GroupID = Entity.ID;
            UnitOfWork.Commit();

            LoadEntities(Entity.ID);            

            Bind();
        }

        protected void btnRemoveFromGroup_Click(object sender, EventArgs e)
        {
            var catalogID = int.Parse(hfCatalogID.Value);
            var catalog = new CatalogRepository(UnitOfWork).Load(catalogID);

            catalog.GroupID = null;
            UnitOfWork.Commit();

            LoadEntities(Entity.ID);

            if (_connectedCatalogs.Count() == 0)
            {
                var catalogGroup = new CatalogGroupRepository(UnitOfWork).Load(Entity.ID, x => x.CatalogGroupLogs, x => x.Invoices);

                catalogGroup.Invoices.ToList().ForEach(y => { UnitOfWork.MarkAsDeleted(y); });
                catalogGroup.CatalogGroupLogs.ToList().ForEach(y => UnitOfWork.MarkAsDeleted(y));
                UnitOfWork.MarkAsDeleted(catalogGroup);

                UnitOfWork.Commit();

                Response.Redirect(string.Format("PaymentOrders.aspx?sID={0}&pID={1}&t=true", Entity.SupplierID, SelectedPhaseID));
            }

            Bind();
        }

        #endregion

        #region [ DataSource Events ]

        protected void odsNotConnectedCatalogs_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<Catalog> criteria = new Criteria<Catalog>();

            criteria.Include(x => x.Book)
                    .Include(x => x.Department);

            var filters = ucCatalogSearchFilters.GetSearchFilters();
            criteria.Expression = filters.GetExpression();

            if(criteria.Expression == null)
            {
                criteria.Expression = Imis.Domain.EF.Search.Criteria<Catalog>.Empty;
            }

            criteria.Expression =
                criteria.Expression.Where(x => x.PhaseID, SelectedPhaseID)
                                    .Where(x => x.SupplierID, Entity.SupplierID)
                                    .Where(x => x.Department.InstitutionID, Entity.InstitutionID)
                                    .IsNull(x => x.GroupID)
                                    .IsNotNull(x => x.BookPriceID)
                                    .Where(x => x.Status, enCatalogStatus.Active)
                                    .Where(string.Format("it.StateInt IN MULTISET ({0}, {1})", enCatalogState.Normal.GetValue(), enCatalogState.FromMove.GetValue()));

            e.InputParameters["criteria"] = criteria;
        }

        #endregion

        #region [ GridView Events ]

        protected void gvNotConnectedCatalogs_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();

            if (action == "refresh")
            {
                gvNotConnectedCatalogs.DataBind();
                return;
            }

            var catalogID = int.Parse(parameters[1]);
            var catalog = new CatalogRepository(UnitOfWork).Load(catalogID);

            if (action == "addtogroup")
            {
                catalog.GroupID = Entity.ID;
                UnitOfWork.Commit();

                _connectedCatalogs = new CatalogRepository(UnitOfWork).FindConnectedCatalogsByGroupID(Entity.ID);

                gvNotConnectedCatalogs.DataBind();

                gvConnectedCatalogs.DataSource = _connectedCatalogs;
                gvConnectedCatalogs.DataBind();
            }


        }

        #endregion

        #region [ GridView Methods ]

        protected bool CanAddToGroup(Catalog catalog)
        {
            if (catalog == null)
                return false;

            return catalog.IsBookActive;
        }

        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvNotConnectedCatalogs.DataBind();
        }
    }
}