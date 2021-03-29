using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using Imis.Domain.EF.Search;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace EudoxusOsy.Portal.Secure.Ministry
{
    public partial class Books : BaseEntityPortalPage
    {
        List<Book> BooksToExport;

        #region [ DataSource Events ]

        protected void odsBooks_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            var criteria = new BusinessModel.Criteria<Book>();
            criteria.Expression = ucSearchFilters.GetSearchFilters().GetExpression();

            if (criteria.Expression == null)
            {
                criteria.Expression = Imis.Domain.EF.Search.Criteria<Book>.Empty;
            }

            e.InputParameters["criteria"] = criteria;
        }

        #endregion

        #region [ GridView Events ]

        protected void gvBooks_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();

            if (action != "refresh")
            {
                var id = int.Parse(parameters[1]);
                var book = new BookRepository(UnitOfWork).Load(id);

                book.IsActive = !book.IsActive;
                UnitOfWork.Commit();
            }
            else
            {
                gvBooks.DataBind();
            }
        }

        #endregion

        #region [ Events ]


        #endregion

        protected void btnExport_Click(object sender, EventArgs e)
        {
            var criteria = new BusinessModel.Criteria<Book>();
            criteria.Expression = ucSearchFilters.GetSearchFilters().GetExpression();

            if (criteria.Expression == null)
            {
                criteria.Expression = Imis.Domain.EF.Search.Criteria<Book>.Empty;
            }

            //Remove ebooks and ProfessorNotes

            criteria.Expression = criteria.Expression.Where(x => x.BookTypeInt, (int)enBookType.eBook, enCriteriaOperator.NotEquals);
            criteria.Expression = criteria.Expression.Where(x => x.BookTypeInt, (int)enBookType.ProfessorNotes, enCriteriaOperator.NotEquals);

            int count = 0;
            criteria.UsePaging = false;
            BooksToExport = new BookRepository(UnitOfWork).FindWithCriteria(criteria, out count);
            gvBooksExport.Export(BooksToExport, "books" + DateTime.Today);
        }

        protected void odsBooks_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.ReturnValue is IList<Book>)
            {
                BooksToExport = (List<Book>)e.ReturnValue;
            }
        }

        protected void gvBooks_ExporterRenderBrick(object sender, DevExpress.Web.ASPxGridViewExportRenderingEventArgs e)
        {
            Book book = gvBooksExport.GetRow(e.VisibleIndex) as Book;

            if (book == null)
                return;

            if (e.Column.Name == "BookType")
            {
                e.Text = book.BookType.GetLabel();
            }

            e.TextValue = e.Text;
        }
    }
}