using DevExpress.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using Imis.Domain.EF.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace EudoxusOsy.Portal.Secure.Ministry
{
    /// <summary>
    /// this page handles the books with unexpected price changes (books that weren't marked as PendingPriceVerification)
    /// </summary>
    public partial class PriceVerificationUnexpected : BaseEntityPortalPage<Book>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void gvBooks_OnCustomCallbackstomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
                            BookHelper.DoUpdateBooksAndCatalogsUnexpected(action, bookPrice, validPhases, UnitOfWork);
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

        protected void odsBooks_OnSelecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["criteria"] = SetCriteria();
        }

        private BusinessModel.Criteria<BookPricesGridV> SetCriteria()
        {
            BusinessModel.Criteria<BookPricesGridV> criteria = new BusinessModel.Criteria<BookPricesGridV>();
            criteria.Sort.OrderBy(x => x.BookID);

            var expression = ucSearchFilters.GetSearchFilters().GetExpression();

            if (expression == null)
            {
                expression = Imis.Domain.EF.Search.Criteria<BookPricesGridV>.Empty;
            }
                        
            criteria.Expression = expression.Where(x => x.HasUnexpectedPriceChange, true);

            return criteria;
        }
        
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("PriceVerification.aspx");
        }

        protected void btnExport_OnClickClick(object sender, EventArgs e)
        {
            var criteria = SetCriteria();
     
            int count = 0;
            criteria.UsePaging = false;
            var booksToExport = new BookPricesGridVRepository(UnitOfWork).FindWithCriteria(criteria, out count);
            gvBooks.Export(booksToExport, "PriceVerificationUnexpected_" + DateTime.Today);
        }
    }
}