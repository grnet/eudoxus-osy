using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.Security;

namespace EudoxusOsy.Portal.Secure.Ministry
{
    public partial class Books : BaseEntityPortalPage
    {
        List<Book> BooksToExport;

        #region [ DataSource Events ]

        protected void odsBooks_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            var criteria = new Criteria<Book>();
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
            var criteria = new Criteria<Book>();
            criteria.Expression = ucSearchFilters.GetSearchFilters().GetExpression();

            if (criteria.Expression == null)
            {
                criteria.Expression = Imis.Domain.EF.Search.Criteria<Book>.Empty;
            }

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

    }
}