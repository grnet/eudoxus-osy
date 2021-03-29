using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;

namespace EudoxusOsy.Portal.UserControls.SearchFilters
{
    public partial class BookPriceVerificationFiltersControl : BaseSearchFiltersControl<BookPriceVerificationSearchFilters>
    {                       
        #region [ Control Inits ]        
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        #endregion

        #region [ Search Filters ]

        public override BookPriceVerificationSearchFilters GetSearchFilters()
        {
            var filters = new BookPriceVerificationSearchFilters();

            filters.BookKpsID = !string.IsNullOrEmpty(txtKpsID.Text) ? int.Parse(txtKpsID.Text) : (int?)null;
            filters.Author = txtAuthor.Text;
            filters.Publisher = txtPublisher.Text;
            filters.ISBN = txtISBN.Text;
            filters.Title = txtTitle.Text;
                       
            filters.HasBookPriceChanges = ddlHasBookPriceChanges.GetSelectedBoolean();
            filters.ChangeYear = !string.IsNullOrEmpty(txtChangeYear.Text) ? int.Parse(txtChangeYear.Text) : (int?)null;

            return filters;
        }

        public override void SetSearchFilters(BookPriceVerificationSearchFilters filters)
        {
            if (filters.BookKpsID.HasValue)
                txtKpsID.Text = filters.BookKpsID.ToString();

            txtAuthor.Text = filters.Author;
            txtISBN.Text = filters.ISBN;
            txtPublisher.Text = filters.Publisher;
            txtTitle.Text = filters.Title;

            if (filters.ChangeYear.HasValue)                
                txtChangeYear.Text = filters.ChangeYear.ToString();

            if (filters.HasBookPriceChanges.HasValue)
                ddlHasBookPriceChanges.SelectedItem = ddlHasBookPriceChanges.Items.FindByValue((filters.HasBookPriceChanges.Value ? 1 : 0));
        }

        #endregion

        protected void ddlHasBookPriceChanges_Init(object sender, EventArgs e)
        {
            ddlHasBookPriceChanges.FillTrueFalse();
        }
    }
}