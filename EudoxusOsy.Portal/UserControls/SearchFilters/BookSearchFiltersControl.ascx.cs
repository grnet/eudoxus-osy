using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using EudoxusOsy.Portal.Controls;
using EudoxusOsy.BusinessModel;

namespace EudoxusOsy.Portal.UserControls.SearchFilters
{
    public partial class BookSearchFiltersControl : BaseSearchFiltersControl<BookSearchFilters>
    {
        #region [ Control Inits ]

        protected void ddlIsActive_Init(object sender, EventArgs e)
        {
            ddlIsActive.FillTrueFalse();
        }

        #endregion

        #region [ Search Filters ]

        public override BookSearchFilters GetSearchFilters()
        {
            var filters = new BookSearchFilters();

            filters.BookKpsID = !string.IsNullOrEmpty(txtKpsID.Text) ? int.Parse(txtKpsID.Text): (int?)null;
            filters.Author = txtAuthor.Text;
            filters.Publisher = txtPublisher.Text;
            filters.ISBN = txtISBN.Text;
            filters.Title = txtTitle.Text;
            filters.IsActive = ddlIsActive.GetSelectedBoolean();

            return filters;
        }

        public override void SetSearchFilters(BookSearchFilters filters)
        {
            if (filters.BookKpsID.HasValue)
                txtKpsID.Text = filters.BookKpsID.ToString();

            txtAuthor.Text = filters.Author;
            txtISBN.Text = filters.ISBN;
            txtPublisher.Text = filters.Publisher;
            txtTitle.Text = filters.Title;

            if (filters.IsActive.HasValue)            
                ddlIsActive.SelectedItem = ddlIsActive.Items.FindByValue((filters.IsActive.Value ? 1 : 0));            
        }

        #endregion
    }
}