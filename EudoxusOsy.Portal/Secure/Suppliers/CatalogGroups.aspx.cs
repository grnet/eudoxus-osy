using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System.Drawing;
using Imis.Domain;
using DevExpress.Web;
using EudoxusOsy.BusinessModel.Flow;

namespace EudoxusOsy.Portal.Secure.Suppliers
{
    public partial class CatalogGroups : BaseEntityPortalPage<Supplier>
    {
        protected Phase SelectedPhase;
        protected SupplierPhaseStatistics SupplierPhaseStatistics;
        protected bool ManageCatalogGroups;

        protected List<CatalogGroup> SupplierGroups;
        protected List<BookInCatalogInfo> _items;

        #region [ Entity Fill ]

        protected override void Fill()
        {
            Entity = new SupplierRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);
            Entity.SaveToCurrentContext();

            int phaseID;
            if (int.TryParse(Request.QueryString["pID"], out phaseID) && phaseID > 0)
            {
                SelectedPhase = new PhaseRepository(UnitOfWork).Load(phaseID);
            }

            bool.TryParse(Request.QueryString["t"], out ManageCatalogGroups);
        }

        #endregion

        #region [ Page Inits ]

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!ManageCatalogGroups)
            {
                mvCatalogGroups.SetActiveView(vSelectPhase);

                gvSupplierPhaseStatistics.DataSource = PhaseHelper.GetSupplierPhaseStatistics(Entity.ID);
                gvSupplierPhaseStatistics.DataBind();
            }
            else
            {
                if (SelectedPhase == null)
                {
                    SelectedPhase = new PhaseRepository(UnitOfWork).GetCurrentPhase();
                }

                mvCatalogGroups.SetActiveView(vManageCatalogGroups);

                ltSelectedPhase.Text += string.Format("Επιλεγμένη Περίοδος Πληρωμών: <b>{0:dd/MM/yyyy}</b> με <b>{1}</b>.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a runat='server' href='CatalogGroups.aspx?pID={2}&t=false'>Αλλαγή Περιόδου Πληρωμών</a>", SelectedPhase.StartDate, SelectedPhase.EndDate.HasValue ? string.Format("{0:dd/MM/yyyy}", SelectedPhase.EndDate) : "σήμερα", SelectedPhase.ID);

                DisplayMoney();
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
                CatalogGroupHelper.GroupCatalogsByInstitution(Entity.ID, SelectedPhase.ID, UnitOfWork);
                Response.Redirect(string.Format("CatalogGroups.aspx?pID={0}&t=true", SelectedPhase.ID));
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

            var catalogGroup = new CatalogGroup()
            {
                PhaseID = SelectedPhase.ID,
                SupplierID = Entity.ID,
                InstitutionID = catalog.Department.InstitutionID,
                State = enCatalogGroupState.New,
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

        protected void btnDeleteGroup_Click(object sender, EventArgs e)
        {
            var catalogGroupID = int.Parse(hfCatalogGroupID.Value);
            var currentGroup = new CatalogGroupRepository(UnitOfWork).Load(catalogGroupID,
                x => x.Catalogs,
                x => x.Invoices,
                x => x.CatalogGroupLogs,
                x => x.LockedCatalogGroups);

            if (currentGroup.ReversalCount.HasValue && currentGroup.ReversalCount.Value > 0)
            {
                lblError.Visible = true;
                lblError.Text = "Δεν μπορείτε να διαγράψετε τη συγκεκριμένη κατάσταση, γιατί έχει ήδη ελεγχθεί από το Υπουργείο.";
                return;
            }

            if (currentGroup.LockedCatalogGroups.Count() > 0)
            {
                lblError.Visible = true;
                lblError.Text = "Δεν μπορείτε να διαγράψετε τη συγκεκριμένη κατάσταση, γιατί έχει ήδη ελεγχθεί από το Υπουργείο.";
                return;
            }

            lblError.Visible = false;

            currentGroup.Catalogs.ToList().ForEach(x => { x.GroupID = null; });
            currentGroup.Invoices.ToList().ForEach(x => { UnitOfWork.MarkAsDeleted(x); });
            currentGroup.CatalogGroupLogs.ToList().ForEach(y => UnitOfWork.MarkAsDeleted(y));

            UnitOfWork.MarkAsDeleted(currentGroup);
            UnitOfWork.Commit();

            gvCatalogGroups.DataBind();
            gvCatalogs.DataBind();
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

        #endregion

        #region [ DataSource Events ]

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

            if (statistics.Phase != null)
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
                if (catalog.BookPriceID.HasValue && catalog.IsBookActive)
                {
                    e.Row.BackColor = Color.LightGreen;
                }
            }
        }

        protected void gvCatalogGroups_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();

            if (action == "refresh")
            {
                gvCatalogGroups.DataBind();
            }
            else
            {
                var id = int.Parse(parameters[1]);

                if (action == "include")
                {
                    var currentGroup = new CatalogGroupRepository(UnitOfWork).Load(id, x => x.Invoices, x => x.Catalogs, x => x.LockedCatalogGroups);

                    if (currentGroup.IsLocked)
                    {
                        if (currentGroup.State == enCatalogGroupState.New)
                        {
                            gvCatalogGroups.Grid.JSProperties["cpError"] = "Η κατάσταση δεν μπορεί να επιλεχθεί για πληρωμή, γιατί είναι κλειδωμένη από το Υπουργείο.";
                            gvCatalogGroups.DataBind();
                            return;
                        }
                        else if (currentGroup.State == enCatalogGroupState.Selected)
                        {
                            gvCatalogGroups.Grid.JSProperties["cpError"] = "Η αναίρεση της επιλογής για πληρωμή δεν μπορεί να πραγματοποιηθεί, γιατί είναι κλειδωμένη από το Υπουργείο.";
                            gvCatalogGroups.DataBind();
                            return;
                        }
                    }
                    else
                    {
                        var cgStateMachine = new CatalogGroupStateMachine(currentGroup);

                        if (cgStateMachine.CanFire(enCatalogGroupTriggers.Select))
                        {
                            var triggerParams = new CatalogGroupTriggerParams()
                            {
                                UnitOfWork = UnitOfWork,
                                Username = Page.User.Identity.Name
                            };

                            cgStateMachine.Select(triggerParams);

                            UnitOfWork.Commit();
                        }
                        else if (cgStateMachine.CanFire(enCatalogGroupTriggers.RevertSelection))
                        {
                            var triggerParams = new CatalogGroupTriggerParams()
                            {
                                UnitOfWork = UnitOfWork,
                                Username = Page.User.Identity.Name
                            };

                            cgStateMachine.RevertSelection(triggerParams);

                            UnitOfWork.Commit();
                        }
                        else if (currentGroup.ContainsInActiveBooks.Value)
                        {
                            gvCatalogGroups.Grid.JSProperties["cpError"] = "Η κατάσταση δεν μπορεί να επιλεχθεί για πληρωμή, γιατί περιέχει διανομές βιβλίων που δεν είναι δυνατόν να τιμολογηθούν και να αποζημιωθούν. Για περισσότερες διευκρινίσεις επικοινωνήστε με το Γραφείο Αρωγής Χρηστών.";
                            gvCatalogGroups.DataBind();
                            return;
                        }
                        else if (currentGroup.PhaseID >= 13 && currentGroup.Catalogs.Any(c => c.HasPendingPriceVerification))
                        {
                            gvCatalogGroups.Grid.JSProperties["cpError"] = "Η κατάσταση δεν μπορεί να επιλεχθεί για πληρωμή, γιατί περιέχει διανομές βιβλίων των οποίων η τιμή εξετάζεται από την επιτροπή κοστολόγησης. Για περισσότερες διευκρινίσεις επικοινωνήστε με το Γραφείο Αρωγής Χρηστών.";
                            gvCatalogGroups.DataBind();
                            return;
                        }
                        /**
                            do not allow group selection if it contains books that have unexpected price change and phase >= 13
                        */
                        else if (currentGroup.PhaseID >= 13 && currentGroup.Catalogs.Any(c => c.HasUnexpectedPriceChange))
                        {
                            gvCatalogGroups.Grid.JSProperties["cpError"] = "Η κατάσταση δεν μπορεί να επιλεχθεί για πληρωμή, γιατί περιέχει διανομές βιβλίων με μη αναμενόμενη αλλαγή στην τιμή τους. Για περισσότερες διευκρινίσεις επικοινωνήστε με το Γραφείο Αρωγής Χρηστών.";
                            gvCatalogGroups.DataBind();
                            return;
                        }
                        else if (currentGroup.InvoiceSum > currentGroup.TotalAmount)
                        {
                            gvCatalogGroups.Grid.JSProperties["cpError"] = "Η κατάσταση δεν μπορεί να επιλεχθεί για πληρωμή, γιατί το συνολικό ποσό των παραστατικών είναι μεγαλύτερο από το συνολικό ποσό της κατάστασης.";
                            gvCatalogGroups.DataBind();
                            return;
                        }
                        else if (currentGroup.StateInt > (int)enCatalogGroupState.Selected)
                        {
                            gvCatalogGroups.Grid.JSProperties["cpError"] = "Η κατάσταση δεν μπορεί να απο-επιλεγεί για πληρωμή, γιατί βρίσκεται σε έλεγχο από το Υπουργείο.";
                            gvCatalogGroups.DataBind();
                            return;
                        }

                        gvCatalogGroups.DataBind();
                    }
                }
                else if (action == "removebanktransfer")
                {
                    var currentGroup = new CatalogGroupRepository(UnitOfWork).Load(id);

                    currentGroup.IsTransfered = false;
                    currentGroup.BankID = null;

                    UnitOfWork.Commit();

                    gvCatalogGroups.DataBind();
                }
                else if (action == "delete")
                {
                    var currentGroup = new CatalogGroupRepository(UnitOfWork).Load(id, x => x.Catalogs, y => y.Invoices, z => z.CatalogGroupLogs);

                    if (currentGroup.ReversalCount.HasValue && currentGroup.ReversalCount.Value > 0)
                    {
                        gvCatalogGroups.Grid.JSProperties["cpError"] = "Δεν μπορείτε να διαγράψετε τη συγκεκριμένη κατάσταση γιατί έχει ήδη ελεγχθεί από το Υπουργείο.";
                        return;
                    }

                    if (currentGroup.LockedCatalogGroups.Count() > 0)
                    {
                        gvCatalogGroups.Grid.JSProperties["cpError"] = "Δεν μπορείτε να διαγράψετε τη συγκεκριμένη κατάσταση γιατί έχει ήδη ελεγχθεί από το Υπουργείο.";
                        return;
                    }

                    currentGroup.Catalogs.ToList().ForEach(x => { x.GroupID = null; });
                    currentGroup.Invoices.ToList().ForEach(x => { UnitOfWork.MarkAsDeleted(x); });
                    currentGroup.CatalogGroupLogs.ToList().ForEach(y => UnitOfWork.MarkAsDeleted(y));

                    UnitOfWork.MarkAsDeleted(currentGroup);
                    UnitOfWork.Commit();

                    gvCatalogGroups.DataBind();
                    gvCatalogs.DataBind();
                }
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

        protected bool IsSelected(SupplierPhaseStatistics statistics)
        {
            if (statistics == null)
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

        protected bool CanEditGroup(CatalogGroupInfo group)
        {
            if (group == null)
                return false;
            //Check when it can be edited (one should be able to remove the pending books and get it paid)
            return group.CanSupplierEditGroup;
        }

        protected bool CanDeleteGroup(CatalogGroupInfo group)
        {
            if (group == null)
                return false;

            return !group.IsLocked
                    && group.GroupStateInt == (int)enCatalogGroupState.New;
        }

        protected bool CanCreateGroup(Catalog catalog)
        {
            return CatalogGroupHelper.CanAddToGroup(catalog);
        }

        #endregion

        protected void btnSearchCatalogs_Click(object sender, EventArgs e)
        {
            gvCatalogs.DataBind();
        }
    }
}