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
    public enum enBookSearchFiltersControlMode
    {
        Normal = 0,
        BookPriceChanges = 1,
        UnExpexted = 2
    }

    public partial class BookSearchFiltersControl : BaseSearchFiltersControl<BookSearchFilters>
    {
        public enBookSearchFiltersControlMode Mode { get; set; }

        #region [ Control Inits ]

        protected void ddlIsActive_Init(object sender, EventArgs e)
        {
            ddlIsActive.FillTrueFalse();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
                ddlIsActive.Visible = Mode == enBookSearchFiltersControlMode.Normal;
                spanIsActive.Visible = Mode == enBookSearchFiltersControlMode.Normal;
                ddlHasBookPriceChanges.Visible = Mode == enBookSearchFiltersControlMode.BookPriceChanges;
                spanHasBookPriceChanges.Visible = Mode == enBookSearchFiltersControlMode.BookPriceChanges;
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

            if (Mode == enBookSearchFiltersControlMode.Normal)
            {
                filters.IsActive = ddlIsActive.GetSelectedBoolean();
            }
            else if(Mode == enBookSearchFiltersControlMode.BookPriceChanges)
            {
                filters.HasBookPriceChanges = ddlHasBookPriceChanges.GetSelectedBoolean();
            }


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