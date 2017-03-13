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

        protected int? CriterionPhaseID
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["fpID"]);
            }
        }

        #region [ Page Inits ]

        protected void Page_Init(object sender, EventArgs e)
        {
            _Master = (Ministry)this.Master;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            var filters = new CatalogSearchFilters();
            filters.PhaseID = CriterionPhaseID;

            if (CriterionPhaseID.HasValue && CriterionPhaseID > 0)
            {
                ucCatalogSearchFiltersControl.SetSearchFilters(filters);
                ClientScript.RegisterStartupScript(GetType(), "catalogsCreated", "window.showAlertBox('Οι διανομές δημιουργήθηκαν επιτυχώς');", true);
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

                    var message = string.Format("Η διανομή με ID  {0}  διαγράφηκε επιτυχώς και η ενέργεια καταγράφηκε με κωδικό {1}.",selectedCatalog.ID,catalogLog.ID);

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
                    ClientScript.RegisterStartupScript(GetType(), "alertError", "window.showAlertBox('Σφάλμα κατά τον αντιλογισμό, δεν βρέθηκε ενεργή διανομή με το επιλεγμένο ID')", true);
                    gvCatalogs.Grid.JSProperties["cperrors"] = "Σφάλμα κατά τον αντιλογισμό, δεν βρέθηκε ενεργή διανομή με το επιλεγμένο ID";
                }
                else
                {
                    var currentPhase = EudoxusOsyCacheManager<Phase>.Current.Get(selectedCatalog.PhaseID.Value);
                    var bookSupplier = new BookSupplierRepository(UnitOfWork).FindBySupplierIDAndBookIDAndYear(selectedCatalog.SupplierID, selectedCatalog.BookID, currentPhase.Year).FirstOrDefault();

                    if (selectedCatalog.GroupID == null || selectedCatalog.CatalogGroup.State != enCatalogGroupState.Sent)
                    {
                        ClientScript.RegisterStartupScript(GetType(), "alertError", "window.showAlertBox('Δεν μπορείτε να προχωρήσετε σε αντιλογισμό της διανομής γιατί δεν έχει αποσταλεί προς ΥΔΕ.')", true);
                        gvCatalogs.Grid.JSProperties["cperrors"] = "Δεν μπορείτε να προχωρήσετε σε αντιλογισμό της διανομής γιατί δεν έχει αποσταλεί προς ΥΔΕ.";
                    }
                    else if (!selectedCatalog.Amount.HasValue)
                    {
                        ClientScript.RegisterStartupScript(GetType(), "alertError", "window.showAlertBox('Δεν μπορείτε να προχωρήσετε σε αντιλογισμό της διανομής γιατί δεν έχει κανένα ποσό.')", true);
                        gvCatalogs.Grid.JSProperties["cperrors"] = "Δεν μπορείτε να προχωρήσετε σε αντιλογισμό της διανομής γιατί δεν έχει κανένα ποσό.";
                    }
                    else
                    {
                        var possibleReversedCatalogs = new CatalogRepository(UnitOfWork).GetPossibleReversedCatalogs(selectedCatalog.SupplierID, selectedCatalog.BookID, selectedCatalog.DepartmentID, selectedCatalog.Amount.Value);
                        if (possibleReversedCatalogs.Count > 0)
                        {
                            ClientScript.RegisterStartupScript(GetType(), "alertError", "window.showAlertBox('Πιθανόν έχει γίνει αντιλογισμός σε αυτή την διανομή. Δείτε IDs: " + String.Join(",", possibleReversedCatalogs.Select(x => x.ID.ToString()).ToArray()) + "')", true);
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

                            ClientScript.RegisterStartupScript(GetType(), "closePopup", "window.showAlertBox('" + message + "');window.parent.popUp.hide();", true);
                            gvCatalogs.Grid.JSProperties["cperrors"] = message;
                        }
                    }
                }
            }
            gvCatalogs.DataBind();

        }
    }
}