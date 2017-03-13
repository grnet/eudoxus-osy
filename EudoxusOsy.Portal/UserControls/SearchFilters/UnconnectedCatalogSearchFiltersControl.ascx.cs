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
    public partial class UnconnectedCatalogSearchFiltersControl : BaseSearchFiltersControl<CatalogSearchFilters>
    {
        public enum enUnconnectedCatalogSearchFilterMode
        {
            Ministry = 0,
            Supplier = 1
        }

        public enUnconnectedCatalogSearchFilterMode Mode { get; set; }

        #region [ Control Inits ]

        protected void ddlInstitution_Init(object sender, EventArgs e)
        {
            ddlInstitution.FillInstitutions();
        }

        #endregion

        #region [ Search Filters ]

        protected void Page_Load(object source, EventArgs e)
        {
            //if(Mode == enUnconnectedCatalogSearchFilterMode.Supplier)
            //{
            //    trInstitution.Visible = true;
            //}
            //else
            //{
            //    trInstitution.Visible = false;
            //}
        }

        public void SetInstitution(int institutionID)
        {
            ddlInstitution.SelectedItem = ddlInstitution.Items.FindByValue(institutionID);
            ddlInstitution.ClientEnabled = false;

            ddlDepartment.FillDepartments(institutionID.ToString());
        }

        public override CatalogSearchFilters GetSearchFilters()
        {
            var filters = new CatalogSearchFilters();

            filters.InstitutionID = ddlInstitution.GetSelectedInteger();
            filters.DepartmentID = ddlDepartment.GetSelectedInteger();
            filters.BookKpsID = txtBookID.GetInteger();
            filters.Title = txtTitle.Text;
            filters.Author = txtAuthor.Text;

            return filters;
        }

        public override void SetSearchFilters(CatalogSearchFilters filters)
        {
            if (filters.InstitutionID.HasValue)
                ddlInstitution.SelectedItem = ddlInstitution.Items.FindByValue(filters.InstitutionID);

            if (filters.DepartmentID.HasValue)
                ddlDepartment.SelectedItem = ddlDepartment.Items.FindByValue(filters.DepartmentID);

            if (filters.BookKpsID.HasValue)            
                txtBookID.Value = filters.BookKpsID;

            if (!string.IsNullOrEmpty(filters.Title))
                txtTitle.Text = filters.Title;

            if (!string.IsNullOrEmpty(filters.Author))
                txtAuthor.Text = filters.Author; 
        }

        #endregion
    }    
}