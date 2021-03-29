using DevExpress.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace EudoxusOsy.Portal.Secure.Ministry
{
    public partial class PriceVerificationMinistry : BaseEntityPortalPage<BookPricesGridV>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void gvBooks_OnCustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();
            var bookPriceId = int.Parse(parameters[1]);


            if (action == "refresh")
            {
                gvBooks.DataBind();
                return;
            }

            if (EudoxusOsyRoleProvider.IsAuthorizedEditorUser())
            {
                if (action == "lock" || action == "unlock")
                {
                    
                    BookPricesGridV bookPrice = new BookPricesGridVRepository(UnitOfWork).FindByBookPriceID(bookPriceId);

                    if (bookPrice.ChangeYear.HasValue)
                    {
                        List<Phase> phases = PhaseHelper.GetPhasesOfYear(bookPrice.ChangeYear.Value);
                        List<Phase> validPhases = new List<Phase>();

                        foreach (var phase in phases)
                        {
                            if (ValidateVerification(phase.ID, bookPrice))
                            {
                                validPhases.Add(phase);
                            }
                        }

                        if (validPhases.Count > 0)
                        {
                            BookHelper.DoUpdateBooksAndCatalogs(action, bookPrice, validPhases, UnitOfWork);
                        }
                        else if (action == "unlock")
                        {
                            bookPrice.ApproveFuturePrice(UnitOfWork);
                        }
                    }                                                           
                }
            }

            gvBooks.DataBind();
        }

        public bool ValidateVerification(int phaseID, BookPricesGridV bookPrice)
        {
            bool ok = false;

            var reversalOrSpecialCatalogs = new CatalogRepository(UnitOfWork).GetSpecialOrReversalCatalogsForBook(bookPrice.BookID, phaseID);

            if (reversalOrSpecialCatalogs != null && reversalOrSpecialCatalogs.Count > 0)
            {
                gvBooks.Grid.JSProperties["cperrors"] =
                    "Η τιμή του βιβλίου δεν μπορεί να εγκριθεί επειδή υπάρχουν δημιουργημένες αντιλογιστικές διανομές. Φάση:" + phaseID;
            }
            else if (bookPrice.Price.HasValue && (!bookPrice.PriceChecked.HasValue || !bookPrice.PriceChecked.Value))
            {
                gvBooks.Grid.JSProperties["cperrors"] =
                    "Η τιμή του βιβλίου δεν μπορεί να εγκριθεί επειδή πρόκειται για τιμή Υπουργείου που δεν έχει ελεγχθεί.";
            }
            else
            {
                ok = true;
            }

            return ok;
        }

        protected void gvBookPriceChange_OnCustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();
            gvBooks.DataBind();
        }

        protected void odsBooks_OnSelecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["criteria"] = SetCriteria();
        }

        private Criteria<BookPricesGridV> SetCriteria()
        {
            Criteria<BookPricesGridV> criteria = new Criteria<BookPricesGridV>();            
            criteria.Sort.OrderBy(x => x.BookID);

            var expression = ucSearchFilters.GetSearchFilters().GetExpression();
            
            if (expression == null)
            {
                expression = Imis.Domain.EF.Search.Criteria<BookPricesGridV>.Empty;
            }
                       
            criteria.Expression = expression.Where(x => x.HasPendingPriceVerification, true);

            return criteria;
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("PriceVerification.aspx");
        }

        protected void btnExportBooks_Click(object sender, EventArgs e)
        {
            var criteria = SetCriteria();
            int count = 0;

            var books = new BookPricesGridVRepository(UnitOfWork).FindWithCriteria(criteria, out count);

            if (books != null && books.Count > 0)
            {
                var booksExport = books.Select(x => new PriceVerificationExportInfo()
                {
                    BookKpsID = x.BookKpsID,
                    Title = x.Title,
                    //CreatedAt = x.LastPriceChange == null ? string.Empty : x.LastPriceChange.CreatedAt.ToShortDateString(),
                    //SuggestedPrice = x.LastPriceChange == null ? string.Empty : x.LastPriceChange.SuggestedPrice.ToString(),
                    //Price = x.LastPriceChange == null ||
                    //    (x.LastPriceChange.Price == 0m && !x.LastPriceChange.PriceChecked)
                    //    ? string.Empty : x.LastPriceChange.Price.ToString()
                });

                gvBooksExport.Export(booksExport, "price_verification");
            }
        }

        protected void btnExport_OnClick(object sender, EventArgs e)
        {
            var criteria = SetCriteria();
     
            int count = 0;
            criteria.UsePaging = false;
            var booksToExport = new BookPricesGridVRepository(UnitOfWork).FindWithCriteria(criteria, out count);
            gvBooks.Export(booksToExport, "PriceVerificationMinistry_" + DateTime.Today);
        }
    }
}