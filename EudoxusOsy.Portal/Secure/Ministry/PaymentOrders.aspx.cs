using DevExpress.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using EudoxusOsy.Portal.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EudoxusOsy.Portal.Secure.Ministry
{
    public partial class PaymentOrders : BaseEntityPortalPage<Supplier>
    {
        protected Phase SelectedPhase;
        protected SupplierPhaseStatistics SupplierPhaseStatistics;
        protected bool ManageCatalogGroups;

        protected List<CatalogGroup> SupplierGroups;
        protected List<BookInCatalogInfo> _items;

        #region [ Entity Fill ]

        protected override void Fill()
        {
            int phaseID;
            if (int.TryParse(Request.QueryString["pID"], out phaseID) && phaseID > 0)
            {
                SelectedPhase = new PhaseRepository(UnitOfWork).Load(phaseID);
            }

            int supplierID;
            if (int.TryParse(Request.QueryString["sID"], out supplierID) && supplierID > 0)
            {
                Entity = new SupplierRepository(UnitOfWork).Load(supplierID, x => x.SupplierIBANs);
                Entity.SaveToCurrentContext();
            }            

            bool.TryParse(Request.QueryString["t"], out ManageCatalogGroups);
        }

        #endregion

        #region [ Page Inits ]

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Entity == null)
            {
                mvPaymentOrders.SetActiveView(vSelectSupplier);
            }
            else
            {
                if (!ManageCatalogGroups)
                {
                    mvPaymentOrders.SetActiveView(vSelectPhase);

                    ltSelectPhase.InnerHtml = string.Format("Επιλεγμένος Εκδότης: <span style='color:black'>{0}</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a runat='server' href='PaymentOrders.aspx'>Αλλαγή Εκδότη</a>", Entity.Name);

                    gvSupplierPhaseStatistics.DataSource = PhaseHelper.GetSupplierPhaseStatistics(Entity.ID);
                    gvSupplierPhaseStatistics.DataBind();
                }
                else
                {
                    mvPaymentOrders.SetActiveView(vManagePaymentOrders);

                    ltSelectedPhase.Text = string.Format("Επιλεγμένος Εκδότης: <span style='color:black'>{0}</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a runat='server' href='PaymentOrders.aspx'>Αλλαγή Εκδότη</a>", Entity.Name);
                    ltSelectedPhase.Text += string.Format("<br/>Επιλεγμένη Περίοδος Πληρωμών: <span style='color:black'><b>{0:dd/MM/yyyy}</b> με <b>{1}</b></span>.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a runat='server' href='PaymentOrders.aspx?sID={2}&pID={3}&t=false'>Αλλαγή Περιόδου Πληρωμών</a>", SelectedPhase.StartDate, SelectedPhase.EndDate.HasValue ? string.Format("{0:dd/MM/yyyy}", SelectedPhase.EndDate) : "σήμερα", Entity.ID, SelectedPhase.ID);

                    DisplayMoney();
                }
            }
        }

        private void DisplayMoney()
        {
            SupplierPhaseStatistics = PhaseHelper.GetSupplierSpecificPhaseStatistics(Entity.ID, SelectedPhase.ID);

            ucSupplierPhaseStatisticsView.Entity = SupplierPhaseStatistics;
            ucSupplierPhaseStatisticsView.Bind();
        }

        #endregion

        #region [ Button Handlers ]

        protected void btnGroupCatalogs_Click(object sender, EventArgs e)
        {
            if (Entity != null)
            {
                CatalogGroupHelper.GroupCatalogsByInstitution(Entity.ID, Entity.GetLatestIBANID(), SelectedPhase.ID, UnitOfWork);
                Response.Redirect(string.Format("~/Secure/Ministry/PaymentOrders.aspx?sID={0}&pID={1}&t=true", Entity.ID, SelectedPhase.ID));
            }
        }

        protected void btnUngroupCatalogs_Click(object sender, EventArgs e)
        {
            if (Entity != null)
            {
                CatalogGroupHelper.UngroupCatalogs(Entity.ID, SelectedPhase.ID, UnitOfWork);
                gvCatalogs.DataBind();
                gvCatalogGroups.DataBind();
            }
        }

        protected void btnCreateGroup_Click(object sender, EventArgs e)
        {

            var catalogID = int.Parse(hfCatalogID.Value);
            var catalog = new CatalogRepository(UnitOfWork).Load(catalogID, x => x.Department);

            if (CanCreateGroup(catalog))
            {
                var catalogGroup = new CatalogGroup()
                {
                    PhaseID = SelectedPhase.ID,
                    SupplierID = Entity.ID,
                    InstitutionID = catalog.Department.InstitutionID,
                    State = enCatalogGroupState.New,
                    SupplierIBANID = Entity.GetLatestIBANID(),
                    IsActive = true
                };

                var deduction = CatalogGroupHelper.FindActiveDeductionForSupplier(Entity.ID);

                if (deduction != null)
                {
                    catalogGroup.DeductionID = deduction.ID;
                }

                catalog.CatalogGroup = catalogGroup;

                UnitOfWork.MarkAsNew(catalogGroup);
                UnitOfWork.Commit();
                Response.Redirect(string.Format("EditCatalogGroup.aspx?pID={0}&gID={1}", SelectedPhase.ID, catalogGroup.ID));
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "canNotAddToGroup", "window.showAlertBox('H διανομή δεν είναι δυνατόν να προστεθεί σε κατάσταση. Πιθανώς έχουν γίνει αλλαγές στη διανομή από άλλον χρήστη του συστήματος.');", true);
            }
        }

        protected void btnDeleteGroup_Click(object sender, EventArgs e)
        {
            var catalogGroupID = int.Parse(hfCatalogGroupID.Value);
            var currentGroup = new CatalogGroupRepository(UnitOfWork).Load(catalogGroupID, x => x.Catalogs, y => y.Invoices, z => z.CatalogGroupLogs);

            if (CanDeleteGroup(currentGroup.ToCatalogGroupInfo()))
            {
                if (currentGroup.ReversalCount.HasValue && currentGroup.ReversalCount.Value > 0)
                {
                    lblCatalogGroupError.Visible = true;
                    lblCatalogGroupError.Text = "Δεν μπορείτε να διαγράψετε τη συγκεκριμένη κατάσταση, γιατί έχει ήδη ελεγχθεί από το Υπουργείο.";
                    return;
                }

                lblCatalogGroupError.Visible = false;

                currentGroup.Catalogs.ToList().ForEach(x => { x.GroupID = null; });
                currentGroup.Invoices.ToList().ForEach(x => { UnitOfWork.MarkAsDeleted(x); });
                currentGroup.CatalogGroupLogs.ToList().ForEach(y => UnitOfWork.MarkAsDeleted(y));

                UnitOfWork.MarkAsDeleted(currentGroup);
                UnitOfWork.Commit();

                gvCatalogGroups.DataBind();
                gvCatalogs.DataBind();
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "canNotDeleteGroup", "window.showAlertBox('Δεν μπορείτε να διαγράψετε την κατάσταση. Πιθανώς έχουν γίνει αλλαγές στην κατάσταση από άλλον χρήστη του συστήματος');", true);
            }
        }

        protected void cbpStatistics_Callback(object sender, CallbackEventArgsBase e)
        {
            DisplayMoney();
        }

        protected void btnExportHidden_Click(object sender, EventArgs e)
        {
            var catalogID = int.Parse(hfExportCatalogID.Value);
            var currentGroup = new CatalogGroupRepository(UnitOfWork).Load(catalogID);
            _items = currentGroup.GetBooksInCatalogGroup(currentGroup.ID, UnitOfWork);

            gvBooks.Export(_items, string.Format("catalog_{0}_{1}", catalogID, Entity.ID));
        }

        protected void btnExportPDFHidden_Click(object sender, EventArgs e)
        {
            var catalogID = int.Parse(hfExportCatalogID.Value);
            Response.Redirect(string.Format("~/Secure/GenerateCatalogPDF.ashx?id={0}", catalogID), true);            
        }


        #endregion

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

        protected void odsCatalogGroups_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["supplierID"] = Entity.ID;
            e.InputParameters["phaseID"] = SelectedPhase.ID;

            int? groupID = txtGroupID.GetInteger();
            if (groupID.HasValue)
            {
                e.InputParameters["groupID"] = groupID.Value;
            }
        }

        protected void odsCatalogs_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<Catalog> criteria = new Criteria<Catalog>();

            criteria.Include(x => x.Book)
                    .Include(x => x.Department.Institution);

            var filters = ucUnconnectedCatalogSearchFilters.GetSearchFilters();
            criteria.Expression = filters.GetExpression();

            if (criteria.Expression == null)
            {
                criteria.Expression = Imis.Domain.EF.Search.Criteria<Catalog>.Empty;
            }

            criteria.Expression =
                criteria.Expression.Where(x => x.PhaseID, SelectedPhase.ID)
                                    .Where(x => x.SupplierID, Entity.ID)
                                    .IsNull(x => x.GroupID)
                                    .Where(x => x.Status, enCatalogStatus.Active)
                                    .Where(string.Format("it.StateInt IN MULTISET ({0}, {1})", enCatalogState.Normal.GetValue(), enCatalogState.FromMove.GetValue()));

            e.InputParameters["criteria"] = criteria;
        }

        #endregion

        #region [ GridView Events ]

        protected void gvSupplierPhaseStatistics_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.GridViewRowType.Data)
                return;

            SupplierPhaseStatistics statistics = (SupplierPhaseStatistics)gvSupplierPhaseStatistics.GetRow(e.VisibleIndex);

            if (statistics.Phase != null && SelectedPhase != null)
            {
                if (statistics.Phase.ID == SelectedPhase.ID)
                {
                    e.Row.BackColor = Color.LightGreen;
                }
            }
        }

        protected void gvCatalogGroups_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.GridViewRowType.Data)
                return;

            CatalogGroupInfo group = (CatalogGroupInfo)gvCatalogGroups.GetRow(e.VisibleIndex);

            if (group != null)
            {
                if (group != null)
                {
                    if (group.InvoiceSum < group.TotalAmount)
                    {
                        e.Row.CssClass = "dxgvDataRow grid-warning-row";
                    }
                    else if (group.InvoiceSum > group.TotalAmount)
                    {
                        e.Row.CssClass = "dxgvDataRow grid-error-row";
                    }
                }
            }
        }

        protected void gvCatalogGroups_HtmlCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            CatalogGroupInfo group = (CatalogGroupInfo)gvCatalogGroups.GetRow(e.VisibleIndex);

            if (group != null)
            {
                if (group.IsLocked)
                {
                    e.Cell.CssClass = "dxgv locked-cataloggroup";
                }
            }
        }

        protected void gvCatalogs_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.GridViewRowType.Data)
                return;

            Catalog catalog = (Catalog)gvCatalogs.GetRow(e.VisibleIndex);

            if (catalog != null)
            {
                if (catalog.HasPendingPriceVerification || catalog.HasUnexpectedPriceChange)
                {
                    e.Row.BackColor = Color.LightSalmon;
                }
                else if (catalog.BookPriceID.HasValue && catalog.IsBookActive)
                {
                    e.Row.BackColor = Color.LightGreen;
                }

            }
        }

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

        protected void gvCatalogGroups_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();

            if (action == "refresh")
            {
                gvCatalogGroups.DataBind();
            }
            else if (action == "exportPDF")
            {
                gvCatalogGroups.Grid.JSProperties.Add("cpexportPDF", true);
            }
            else
            {
                string error;
                string jsProperty;

                PaymentOrdersUserManagement poManagement = new PaymentOrdersUserManagement(UnitOfWork);

                if (!poManagement.ManageActions(Page.User.Identity.Name, true, action, parameters,
                    out error, out jsProperty))
                {
                    gvCatalogGroups.Grid.JSProperties[jsProperty] = error;
                }
                gvCatalogGroups.DataBind();
                gvCatalogs.DataBind();
            }
        }



        protected void gvCatalogs_CustomDataCallback(object sender, ASPxGridViewCustomDataCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();

            if (action == "refresh")
            {
                gvCatalogs.DataBind();
            }
            else
            {
                e.Result = ResolveClientUrl("~/Secure/Suppliers/SelectCatalog.aspx");
            }
        }

        #endregion

        #region [ GridView Methods ]

        protected bool IsZeroVatElegible()
        {
            return Entity.ZeroVatEligible;
        }

        protected bool IsSelected(SupplierPhaseStatistics statistics)
        {
            if (statistics == null || SelectedPhase == null)
                return false;

            return statistics.Phase.ID == SelectedPhase.ID;
        }

        protected bool ContainsInActiveBooks(CatalogGroupInfo group)
        {
            if (group == null)
                return false;

            return group.ContainsInActiveBooks;
        }

        protected bool HasPendingPriceVerification(CatalogGroupInfo group)
        {
            if (group == null)
                return false;

            return group.HasPendingPriceVerification;
        }

        protected bool HasUnexpectedPriceChange(CatalogGroupInfo group)
        {
            if (group == null)
                return false;

            return group.HasUnexpectedPriceChange;
        }

        protected bool HasWarning(CatalogGroupInfo group)
        {
            if (group == null)
                return false;

            return group.InvoiceSum < group.TotalAmount;
        }

        protected bool HasError(CatalogGroupInfo group)
        {
            if (group == null)
                return false;

            return group.InvoiceSum > group.TotalAmount;
        }

        protected string GetGroupDeduction(CatalogGroupInfo group)
        {
            if (group == null)
                return string.Empty;

            if (group.Deduction != null)
            {
                return group.Deduction.VatType.GetLabel();
            }   
            else if (Entity.ZeroVatEligible && group.Deduction == null && group.Vat == null)
            {
                return "Μηδενικό";
            }         
            else
            {
                return string.Format("{0} ({1:C})", enDeductionVatType.Custom.GetLabel(), group.Vat);
            }
        }

        protected bool CanEditGroup(CatalogGroupInfo group)
        {
            return CatalogGroupHelper.CanEditGroup(group);
        }

        protected bool CanDeleteGroup(CatalogGroupInfo group)
        {
            return CatalogGroupHelper.CanDeleteGroup(group);
        }

        protected bool CanCreateGroup(Catalog catalog)
        {
            return CatalogGroupHelper.CanAddToGroup(catalog);
        }

        protected bool CanApproveGroup(CatalogGroupInfo group)
        {
            return CatalogGroupHelper.CanApproveGroup(group);
        }

        protected bool CanRevertApproval(CatalogGroupInfo group)
        {
            return CatalogGroupHelper.CanRevertApproval(group);
        }

        protected bool CanSendToYDE(CatalogGroupInfo group)
        {
            return CatalogGroupHelper.CanSendToYDE(group);
        }

        protected bool CanReturnFromYDE(CatalogGroupInfo group)
        {
            return CatalogGroupHelper.CanReturnFromYDE(group);
        }

        protected bool CanMovePhase(Catalog catalog)
        {
            return CatalogGroupHelper.CanMovePhase(catalog);
        }

        protected string InabilityToCreateGroup(Catalog catalog)
        {
            return CatalogGroupHelper.InabilityToCreateGroup(catalog);
        }

        #endregion

        protected void btnSearchCatalogs_Click(object sender, EventArgs e)
        {
            gvCatalogs.DataBind();
        }

        protected void gvCatalogs_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();
            var id = int.Parse(parameters[1]);

            if (action == "refresh")
            {
                gvCatalogs.DataBind();
            }
            else if (action == "movetophase")
            {
                if (new PhaseRepository().IsActive(id))
                {
                    var catalogId = Convert.ToInt32(hfCatalogID.Value);
                    var catalog = new CatalogRepository(UnitOfWork).Load(catalogId);
                    var newCatalog = catalog.Move(id);
                    UnitOfWork.MarkAsNew(newCatalog);
                    UnitOfWork.Commit();
                    catalog.NewCatalogID = newCatalog.ID;
                    UnitOfWork.Commit();
                }
                gvCatalogs.DataBind();
            }
        }
    }
}