using DevExpress.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Web.UI.WebControls;
using System.Linq.Expressions;
using System.Linq;

namespace EudoxusOsy.Portal.Secure.Ministry
{
    public partial class PriceVerificationHistory : BaseEntityPortalPage<Book>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void gvBookPriceChange_OnCustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();
            gvBookPriceChange.DataBind();
        }

        protected void odsBookPriceChanges_OnSelecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<BookPriceChange> criteria = new Criteria<BookPriceChange>();
            if (!string.IsNullOrEmpty(txtBookKpsID.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.Book.BookKpsID, int.Parse(txtBookKpsID.Text));
            }
            criteria.Include(x => x.Book);
            criteria.Sort.OrderBy(x => x.ID);

            e.InputParameters["criteria"] = criteria;
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("PriceVerification.aspx");
        }

        protected void gvBookPriceChange_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if(e.RowType == GridViewRowType.Data)
            {
                BookPriceChange bpChange = (BookPriceChange)gvBookPriceChange.GetRow(e.VisibleIndex);
                if(bpChange.IsUnexpected == true)
                {
                    e.Row.BackColor = System.Drawing.Color.LightYellow;
                }
            }
        }

        protected void btnExport_OnClick(object sender, EventArgs e)
        {
            Criteria<BookPriceChange> criteria = new Criteria<BookPriceChange>();
            if (!string.IsNullOrEmpty(txtBookKpsID.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.Book.BookKpsID, int.Parse(txtBookKpsID.Text));
            }
            criteria.Include(x => x.Book);
            criteria.Sort.OrderBy(x => x.ID);
            
     
            int count = 0;
            criteria.UsePaging = false;
            var booksToExport = new BookPriceChangeRepository(UnitOfWork).FindWithCriteria(criteria, out count);
            gvBookPriceChange.Export(booksToExport, "PriceVerificationHistory_" + DateTime.Today);
        }
    }
}