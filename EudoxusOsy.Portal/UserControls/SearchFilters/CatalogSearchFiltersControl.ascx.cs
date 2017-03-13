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
    public partial class CatalogSearchFiltersControl : BaseSearchFiltersControl<CatalogSearchFilters>
    {
        #region [ Control Inits ]

        protected void ddlPhase_Init(object sender, EventArgs e)
        {
            ddlPhase.FillPhasesIDs(true, "-- αδιάφορο --");
        }

        protected void ddlIsInGroup_Init(object sender, EventArgs e)
        {
            ddlIsInGroup.FillTrueFalse();            
        }

        protected void ddlCatalogState_Init(object sender, EventArgs e)
        {
            ddlCatalogState.FillFromEnum<enCatalogState>("-- αδιάφορο --", true, true);
        }

        protected void ddlGroupState_Init(object sender, EventArgs e)
        {
            ddlGroupState.FillFromEnum<enCatalogGroupState>("-- αδιάφορο --", true, true);
        }

        #endregion

        #region [ Search Filters ]

        public override CatalogSearchFilters GetSearchFilters()
        {
            var filters = new CatalogSearchFilters();

            filters.ID = txtCatalogID.GetInteger();
            filters.GroupID = txtGroupID.GetInteger();
            filters.PhaseID = ddlPhase.GetSelectedInteger();
            filters.BookKpsID = txtBookKpsID.GetInteger();
            filters.SupplierKpsID = txtSupplierKpsID.GetInteger();
            filters.SecretaryKpsID = txtSecretaryKpsID.GetInteger();
            filters.CreatedAt = txtCreatedAt.GetDate();
            filters.DiscountPercentage = txtDiscountPercentage.GetDecimal();
            filters.BookCount = txtBookCount.GetInteger();
            filters.Percentage = txtPercentage.GetDecimal();
            filters.Amount = txtAmount.GetDecimal();
            filters.IsInGroup = ddlIsInGroup.GetSelectedBoolean();
            filters.State = ddlCatalogState.GetSelectedInteger();
            filters.GroupState = ddlGroupState.GetSelectedInteger();
            filters.CatalogType = ddlCatalogType.GetSelectedEnum<enCatalogType>();
            filters.IsForLibrary = ddlIsLibrary.GetSelectedInteger();
            //filters.CatalogStatus = ddlCatalogStatus.GetSelectedEnum<enCatalogStatus>();

            return filters;
        }

        public override void SetSearchFilters(CatalogSearchFilters filters)
        {
            if (filters.ID.HasValue)
                txtCatalogID.Value = filters.ID;

            if (filters.GroupID.HasValue)
                txtGroupID.Value = filters.GroupID;

            if (filters.PhaseID.HasValue)
                ddlPhase.SelectedItem = ddlPhase.Items.FindByValue(filters.PhaseID);

            if (filters.BookKpsID.HasValue)
                txtBookKpsID.Value = filters.BookKpsID;

            if (filters.SupplierKpsID.HasValue)
                txtSupplierKpsID.Value = filters.SupplierKpsID;

            if (filters.SecretaryKpsID.HasValue)
                txtSecretaryKpsID.Value = filters.SecretaryKpsID;

            if (filters.CreatedAt.HasValue)
                txtCreatedAt.Value = filters.CreatedAt;

            if (filters.DiscountPercentage.HasValue)
                txtDiscountPercentage.Value = filters.DiscountPercentage;

            if (filters.BookCount.HasValue)
                txtBookCount.Value = filters.BookCount;

            if (filters.Percentage.HasValue)
                txtPercentage.Value = filters.Percentage;

            if (filters.Amount.HasValue)
                txtAmount.Value = filters.Amount;

            if (filters.IsInGroup.HasValue)
                ddlIsInGroup.SelectedItem = ddlIsInGroup.Items.FindByValue(filters.IsInGroup.Value);

            if (filters.State.HasValue)
                ddlCatalogState.SelectedItem = ddlCatalogState.Items.FindByValue(filters.State);

            if (filters.GroupState.HasValue)
                ddlGroupState.SelectedItem = ddlGroupState.Items.FindByValue(filters.GroupState);

            if(filters.CatalogType.HasValue)
            {
                ddlCatalogType.SelectedItem = ddlCatalogType.Items.FindByValue(filters.CatalogType);
            }

            if(filters.IsForLibrary.HasValue)
            {
                ddlIsLibrary.SelectedItem = ddlIsLibrary.Items.FindByValue(filters.IsForLibrary);
            }

            //if(filters.CatalogStatus.HasValue)
            //{
            //    ddlCatalogStatus.SelectedItem = ddlCatalogStatus.Items.FindByValue(filters.CatalogStatus);
            //}
        }

        #endregion

        protected void ddlCatalogType_Init(object sender, EventArgs e)
        {
            ddlCatalogType.FillFromEnum<enCatalogType>(includeZeroValue: true);
        }

        protected void ddlCatalogStatus_Init(object sender, EventArgs e)
        {
            //ddlCatalogStatus.FillFromEnum<enCatalogStatus>(includeZeroValue: true);
        }
    }
}