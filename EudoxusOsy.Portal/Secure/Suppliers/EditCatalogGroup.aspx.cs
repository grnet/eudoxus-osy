using DevExpress.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EudoxusOsy.Portal.Secure.Suppliers
{
    public partial class EditCatalogGroup : BaseSecureEntityPortalPage<CatalogGroupInfo>
    {
        #region [ Entity Fill ]

        public Supplier CurrentSupplier { get; set; }
        protected int SelectedPhaseID;
        private List<Catalog> _connectedCatalogs = new List<Catalog>();

        protected override void Fill()
        {
            int catalogGroupID;
            if (int.TryParse(Request.QueryString["gID"], out catalogGroupID))
            {
                using (UnitOfWork.SingleConnection())
                {
                    CurrentSupplier = new SupplierRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);
                    CurrentSupplier.SaveToCurrentContext();

                    LoadEntities(catalogGroupID);
                }
            }

            int.TryParse(Request.QueryString["pID"], out SelectedPhaseID);

            if (Entity == null || !Entity.CanSupplierEditGroup || SelectedPhaseID <= 0)
            {
                Response.Redirect(string.Format("CatalogGroups.aspx?pID={0}&t=true", SelectedPhaseID));
            }
        }

        protected override bool Authorize()
        {
            return Entity.SupplierID == CurrentSupplier.ID || EudoxusOsyRoleProvider.IsAuthorizedEditorUser();
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
            ((Suppliers)Page.Master).HideSiteMap = true;
            BindGroupInfo();
            BindConnectedCatalogs();

            ucConnectedCatalogSearchFilters.SetInstitution(Entity.InstitutionID);
            ucCatalogSearchFilters.SetInstitution(Entity.InstitutionID);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsAuthorized)
            {
                Response.Redirect(string.Format("CatalogGroups.aspx?pID={0}&t=true", SelectedPhaseID), true);
            }
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
            Response.Redirect(string.Format("CatalogGroups.aspx?pID={0}&t=true", SelectedPhaseID));
        }

        protected void btnAddToGroup_Click(object sender, EventArgs e)
        {
            var catalogID = int.Parse(hfCatalogID.Value);
            var catalog = new CatalogRepository(UnitOfWork).Load(catalogID);

            if (CatalogGroupHelper.CanAddToGroup(catalog))
            {
                catalog.GroupID = Entity.ID;
                UnitOfWork.Commit();
            }

            LoadEntities(Entity.ID);

            Bind();
        }

        protected void btnRemoveFromGroup_Click(object sender, EventArgs e)
        {
            var catalogID = int.Parse(hfCatalogID.Value);
            var catalog = new CatalogRepository(UnitOfWork).Load(catalogID);

            if (CatalogGroupHelper.CanEditGroup(Entity))
            {
                catalog.GroupID = null;
                UnitOfWork.Commit();

                LoadEntities(Entity.ID);

                if (!_connectedCatalogs.Any())
                {
                    var catalogGroup = new CatalogGroupRepository(UnitOfWork).Load(Entity.ID, x => x.CatalogGroupLogs, x => x.Invoices);

                    catalogGroup.Invoices.ToList().ForEach(y => { UnitOfWork.MarkAsDeleted(y); });

                    if (catalogGroup.State == enCatalogGroupState.New)
                    {
                        catalogGroup.CatalogGroupLogs.ToList().ForEach(y => UnitOfWork.MarkAsDeleted(y));
                        UnitOfWork.MarkAsDeleted(catalogGroup);
                    }

                    UnitOfWork.Commit();

                    Response.Redirect(string.Format("CatalogGroups.aspx?pID={0}&t=true", SelectedPhaseID));
                }
                Bind();
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "alertCanNotDelete", "window.showAlertBox('Δεν επιτρέπεται η επεξεργασία της κατάστασης.');", true);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvNotConnectedCatalogs.DataBind();
        }

        protected void btnSearchConnected_OnClick(object sender, EventArgs e)
        {
            gvConnectedCatalogs.DataBind();
        }
        #endregion

        #region [ DataSource Events ]
        protected void odsConnectedCatalogs_OnSelecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<Catalog> criteria = new Criteria<Catalog>();

            criteria.Include(x => x.Book)
                .Include(x => x.Department);

            var filters = ucConnectedCatalogSearchFilters.GetSearchFilters();
            criteria.Expression = filters.GetExpression();

            if (criteria.Expression == null)
            {
                criteria.Expression = Imis.Domain.EF.Search.Criteria<Catalog>.Empty;
            }

            criteria.Expression =
                criteria.Expression.Where(x => x.PhaseID, SelectedPhaseID)
                    .Where(x => x.SupplierID, Entity.SupplierID)
                    .Where(x => x.Department.InstitutionID, Entity.InstitutionID)
                    .Where(x => x.GroupID, Entity.ID)
                    .Where(x => x.Status, enCatalogStatus.Active)
                    .Where(string.Format("it.StateInt IN MULTISET ({0}, {1})", enCatalogState.Normal.GetValue(), enCatalogState.FromMove.GetValue()));

            e.InputParameters["criteria"] = criteria;
        }
        protected void odsNotConnectedCatalogs_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<Catalog> criteria = new Criteria<Catalog>();

            criteria.Include(x => x.Book)
                    .Include(x => x.Department);

            var filters = ucCatalogSearchFilters.GetSearchFilters();
            criteria.Expression = filters.GetExpression();

            if (criteria.Expression == null)
            {
                criteria.Expression = Imis.Domain.EF.Search.Criteria<Catalog>.Empty;
            }

            criteria.Expression =
                criteria.Expression.Where(x => x.PhaseID, SelectedPhaseID)
                                    .Where(x => x.SupplierID, CurrentSupplier.ID)
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

            if (action == "addtogroup" && CanAddToGroup(catalog) && CatalogGroupHelper.CanEditGroup(Entity))
            {
                catalog.GroupID = Entity.ID;
                UnitOfWork.Commit();

                _connectedCatalogs = new CatalogRepository(UnitOfWork).FindConnectedCatalogsByGroupID(Entity.ID);

                gvNotConnectedCatalogs.DataBind();
                gvConnectedCatalogs.DataBind();
            }
        }

        #endregion

        #region [ GridView Methods ]

        protected bool CanAddToGroup(Catalog catalog)
        {
            return CatalogGroupHelper.CanAddToGroup(catalog);
        }
        #endregion

        protected void gvConnectedCatalogs_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            Catalog catalog = (Catalog)gvConnectedCatalogs.GetRow(e.VisibleIndex);

            if (catalog != null && ( catalog.HasPendingPriceVerification || catalog.HasUnexpectedPriceChange))
            {
                e.Row.BackColor = Color.LightSalmon;
            }
        }

        protected void gvNotConnectedCatalogs_OnHtmlRowPreparedatalogs_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            Catalog catalog = (Catalog)gvNotConnectedCatalogs.GetRow(e.VisibleIndex);

            if (catalog != null && (catalog.HasPendingPriceVerification || catalog.HasUnexpectedPriceChange))
            {
                e.Row.BackColor = Color.LightSalmon;
            }
        }
    }
}