using DevExpress.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using Imis.Domain.EF.Search;
using System;
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
            var id = int.Parse(parameters[1]);

            if (action == "refresh")
            {
                gvBooks.DataBind();
                return;
            }
            else if (action == "lock" || action == "unlock")
            {
                var currentPhase = new PhaseRepository(UnitOfWork).GetCurrentPhase();
                var selectedBook = new BookRepository(UnitOfWork).Load(id, x => x.BookPriceChanges);
                var newPrice = selectedBook.LastPriceChange == null ? null : (selectedBook.LastPriceChange.Price.HasValue ? selectedBook.LastPriceChange.Price : selectedBook.LastPriceChange.SuggestedPrice);
                BookHelper.DoUpdateBooksAndCatalogsUnexpected(action, id, currentPhase, newPrice, UnitOfWork);
            }

            gvBooks.DataBind();
        }

        protected void odsBooks_OnSelecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            BusinessModel.Criteria<Book> criteria = new BusinessModel.Criteria<Book>();
            criteria.Include(x => x.BookPriceChanges);
            criteria.Sort.OrderBy(x => x.ID);

            var expression = ucSearchFilters.GetSearchFilters().GetExpression();

            if (expression == null)
            {
                expression = Imis.Domain.EF.Search.Criteria<Book>.Empty;
            }

            expression = expression.Where(x => x.HasUnexpectedPriceChangePhaseID, 13, enCriteriaOperator.GreaterThanEquals);
            criteria.Expression = string.IsNullOrEmpty(expression.CommandText) ? null : expression;

            e.InputParameters["criteria"] = criteria;
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("PriceVerification.aspx");
        }
    }
}