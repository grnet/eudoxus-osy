using System;
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
    public partial class Catalogs : BaseEntityPortalPage
    {
        Ministry _Master;
        List<Catalog> PageCatalogs = new List<Catalog>();
        List<EditCatalogsGridV> CatalogsToExport;

        protected int? CriterionPhaseID
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["fpID"]);
            }
        }


        protected int? CriterionBookID
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["bID"]);
            }
        }

        #region [ Page Inits ]

        protected void Page_Init(object sender, EventArgs e)
        {
            _Master = (Ministry)this.Master;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var hasQueryStringCriteria = false;

            var filters = new CatalogSearchFilters();

            if (CriterionPhaseID.HasValue && CriterionPhaseID > 0)
            {
                filters.PhaseID = CriterionPhaseID;
                hasQueryStringCriteria = true;
                ClientScript.RegisterStartupScript(GetType(), "catalogsCreated", "window.showAlertBox('Οι διανομές δημιουργήθηκαν επιτυχώς');", true);
            }

            if (CriterionBookID.HasValue && CriterionBookID > 0)
            {
                filters.BookKpsID = CriterionBookID;
                hasQueryStringCriteria = true;
            }

            if (hasQueryStringCriteria)
            {
                ucCatalogSearchFiltersControl.SetSearchFilters(filters);
            }
        }

        #endregion

        #region [ Button Handlers ]



        #endregion

        #region [ DataSource Events ]

        protected void odsCatalogs_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<Catalog> criteria = new Criteria<Catalog>();

            criteria.Include(x => x.Book)
                .Include(x => x.Supplier)
                .Include(x => x.Department)
                .Include(x => x.Discount)
                .Include(x => x.CatalogGroup);


            criteria.Sort.OrderByDescending(x => x.ID);

            criteria.Expression = ucCatalogSearchFiltersControl.GetSearchFilters().GetExpression();

            if (criteria.Expression == null)
            {
                criteria.Expression = Imis.Domain.EF.Search.Criteria<Catalog>.Empty;
            }

            criteria.Expression = criteria.Expression.Where(x => x.Phase.IsActive, true);
            e.InputParameters["criteria"] = criteria;
        }

        #endregion

        #region [ GridView Events ]

        protected void gvCatalogs_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.GridViewRowType.Data)
                return;

            Catalog Catalog = (Catalog)gvCatalogs.GetRow(e.VisibleIndex);

            if (Catalog != null)
            {
                e.Row.BackColor = Color.LightGreen;
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
            }
        }

        #endregion

        protected void gvCatalogs_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();
            if (action == "delete")
            {
                var selectedCatalog = new CatalogRepository(UnitOfWork).Load(Convert.ToInt32(parameters[1]), x => x.CatalogGroup);

                if (selectedCatalog.GroupID == null && selectedCatalog.Status == enCatalogStatus.Active)
                {
                    selectedCatalog.Status = enCatalogStatus.Deleted;
                    //UnitOfWork.MarkAsDeleted(selectedCatalog);

                    var catalogLog = selectedCatalog.CreateCatalogLog(enCatalogLogAction.Delete, User.Identity.Name, User.Identity.ReporterID);
                    UnitOfWork.MarkAsNew(catalogLog);
                    UnitOfWork.Commit();

                    var message = string.Format("Η διανομή με ID  {0}  διαγράφηκε επιτυχώς και η ενέργεια καταγράφηκε με κωδικό {1}.", selectedCatalog.ID, catalogLog.ID);

                    ClientScript.RegisterStartupScript(GetType(), "closePopup", "window.parent.showAlertBox('" + message + "');window.parent.popUp.hide();", true);
                    gvCatalogs.Grid.JSProperties["cperrors"] = message;
                }

            }
            else if (action == "reverse")
            {
                var selectedCatalog = new CatalogRepository(UnitOfWork).Load(Convert.ToInt32(parameters[1]), x => x.CatalogGroup,
                    x => x.Book,
                    x => x.Supplier,
                    x => x.Department,
                    x => x.Discount);

                if (selectedCatalog == null || selectedCatalog.Status != enCatalogStatus.Active)
                {
                    gvCatalogs.Grid.JSProperties["cperrors"] = "Σφάλμα κατά τον αντιλογισμό, δεν βρέθηκε ενεργή διανομή με το επιλεγμένο ID";
                }
                else
                {
                    var currentPhase = EudoxusOsyCacheManager<Phase>.Current.Get(selectedCatalog.PhaseID.Value);
                    var bookSupplier = new BookSupplierRepository(UnitOfWork).FindBySupplierIDAndBookIDAndYear(selectedCatalog.SupplierID, selectedCatalog.BookID, currentPhase.Year).FirstOrDefault();

                    if (selectedCatalog.GroupID == null || selectedCatalog.CatalogGroup.State != enCatalogGroupState.Sent)
                    {
                        gvCatalogs.Grid.JSProperties["cperrors"] = "Δεν μπορείτε να προχωρήσετε σε αντιλογισμό της διανομής γιατί δεν έχει αποσταλεί προς ΥΔΕ.";
                    }
                    else if (!selectedCatalog.Amount.HasValue)
                    {
                        gvCatalogs.Grid.JSProperties["cperrors"] = "Δεν μπορείτε να προχωρήσετε σε αντιλογισμό της διανομής γιατί δεν έχει κανένα ποσό.";
                    }
                    else
                    {
                        var possibleReversedCatalogs = new CatalogRepository(UnitOfWork).GetPossibleReversedCatalogs(selectedCatalog.SupplierID, selectedCatalog.BookID, selectedCatalog.DepartmentID, selectedCatalog.Amount.Value);
                        if (possibleReversedCatalogs.Count > 0)
                        {
                            gvCatalogs.Grid.JSProperties["cperrors"] = "Πιθανόν έχει γίνει αντιλογισμός σε αυτή την διανομή. Δείτε τις διανομές με ID: " + String.Join(",", possibleReversedCatalogs.Select(x => x.ID.ToString()).ToArray());
                        }
                        else
                        {
                            var newCatalog = selectedCatalog.Reverse(bookSupplier);

                            UnitOfWork.MarkAsNew(newCatalog);
                            UnitOfWork.Commit();


                            var catalogLog = selectedCatalog.CreateCatalogLog(enCatalogLogAction.Rollback, User.Identity.Name, User.Identity.ReporterID);

                            UnitOfWork.MarkAsNew(catalogLog);
                            UnitOfWork.Commit();

                            var message = string.Format("Ο αντιλογισμός της διανομής με ID  {0}  έγινε επιτυχώς και η ενέργεια καταγράφηκε με κωδικό {1}. Η νέα διανομή έχει ID {2}.", selectedCatalog.ID, catalogLog.ID, newCatalog.ID);

                            gvCatalogs.Grid.JSProperties["cperrors"] = message;
                        }
                    }
                }
            }
            gvCatalogs.DataBind();

        }

        protected void btnExport_OnClick(object sender, EventArgs e)
        {
            var criteria = new Criteria<EditCatalogsGridV>();
            var searchFilters = new EditCatalogSearchFilters();

            var uiFilters = ucCatalogSearchFiltersControl.GetSearchFilters();

            searchFilters.ID = uiFilters.ID;
            searchFilters.GroupID = uiFilters.GroupID;
            searchFilters.PhaseID = uiFilters.PhaseID;
            searchFilters.BookKpsID = uiFilters.BookKpsID;
            searchFilters.SupplierKpsID = uiFilters.SupplierKpsID;
            searchFilters.SecretaryKpsID = uiFilters.SecretaryKpsID;
            searchFilters.CreatedAt = uiFilters.CreatedAt;
            searchFilters.DiscountPercentage = uiFilters.DiscountPercentage;
            searchFilters.BookCount = uiFilters.BookCount;
            searchFilters.Percentage = uiFilters.Percentage;
            searchFilters.Amount = uiFilters.Amount;
            searchFilters.IsInGroup = uiFilters.IsInGroup;
            searchFilters.IsLocked = uiFilters.IsLocked;
            searchFilters.State = uiFilters.State;
            searchFilters.GroupState = uiFilters.GroupState;
            searchFilters.InstitutionID = uiFilters.InstitutionID;
            searchFilters.DepartmentID = uiFilters.DepartmentID;
            searchFilters.CatalogType = uiFilters.CatalogType;
            searchFilters.CatalogStatus = uiFilters.CatalogStatus;
            searchFilters.IsForLibrary = uiFilters.IsForLibrary;

            criteria.Expression = searchFilters.GetExpression();

            if (criteria.Expression == null)
            {
                criteria.Expression = Imis.Domain.EF.Search.Criteria<EditCatalogsGridV>.Empty;
            }            

            int count = 0;
            criteria.UsePaging = false;                      

            CatalogsToExport = new EditCatalogsGridVRepository(UnitOfWork).FindWithCriteria(criteria, out count);

            if (CatalogsToExport.Count < 100000)
            {
                gvCatalogsExport.Export(CatalogsToExport, "catalogs" + DateTime.Today);
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "showErrorMsg", "alert('Περιορίστε τα αποτελέσματα της αναζήτσης.');", true);
            }

        }

        protected void gvCatalogsExport_ExporterRenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            EditCatalogsGridV gridV = gvCatalogsExport.GetRow(e.VisibleIndex) as EditCatalogsGridV;

            if (gridV == null)
                return;
            
            if (e.Column.Name == "GroupState")
            {
                if (gridV.GroupState != null)
                {
                    e.Text = ((enCatalogGroupState) gridV.GroupState).GetLabel();
                }
                else
                {
                    e.Text = string.Empty;
                }
            }
            else if (e.Column.Name == "State")
            {
                e.Text = ((enCatalogState) gridV.State).GetLabel();
            }
            else if (e.Column.Name == "Status")
            {
                e.Text = ((enCatalogStatus)gridV.Status).GetLabel();
            }
            else if (e.Column.Name == "CatalogType")
            {
                e.Text = ((enCatalogType)gridV.CatalogType).GetLabel();
            }
            else if (e.Column.Name == "Discount")
            {
                e.Text = ((decimal?) gridV.DiscountPercentage).HasValue
                    ? (100 - (gridV.DiscountPercentage) * 100) + "%"
                    : string.Empty;
            }
            else if (e.Column.Name == "IsForLibrary")
            {
                e.Text = gridV.SecretaryKpsID != null ? "Φοιτητές" : "Βιβλιοθήκη";
            }
            else if (e.Column.Name == "Percentage")
            {
                e.Text = (gridV.Percentage).HasValue ? (gridV.Percentage) + "%" : string.Empty;
            }
            else if (e.Column.Name == "CreatedAt")
            {
                e.Text = gridV.CreatedAt > new DateTime(1970, 1, 2)
                    ? gridV.CreatedAt.ToShortDateString()
                    : string.Empty;
            }
             
            e.TextValue = e.Text;
        }
    }
}