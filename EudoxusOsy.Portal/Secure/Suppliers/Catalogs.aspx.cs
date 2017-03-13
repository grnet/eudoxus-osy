using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System.Drawing;
using Imis.Domain;
using DevExpress.Web;

namespace EudoxusOsy.Portal.Secure.Suppliers
{
    public partial class Catalogs : BaseEntityPortalPage<Supplier>
    {
        //Suppliers _Master;

        //#region [ Entity Fill ]

        //protected override void Fill()
        //{
        //    Entity = new SupplierRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);
        //    Entity.SaveToCurrentContext();
        //}

        //#endregion

        //#region [ Page Inits ]

        //protected void Page_Init(object sender, EventArgs e)
        //{
        //    gvCatalogs.PageSize = int.MaxValue;
        //    _Master = (Suppliers)this.Master;
        //}

        //#endregion

        //#region [ Button Handlers ]

        //protected void btnGroupCatalogs_Click(object sender, EventArgs e)
        //{
        //    if (Entity != null)
        //    {
        //        CatalogGroupHelper.GroupCatalogsByInstitution(Entity.ID, _Master.SelectedPhaseID, UnitOfWork);
        //        Response.Redirect("~/Secure/Suppliers/CatalogGroups.aspx");
        //    }
        //}

        //protected void btnUngroupCatalogs_Click(object sender, EventArgs e)
        //{
        //    if (Entity != null)
        //    {
        //        CatalogGroupHelper.UngroupCatalogs(Entity.ID, _Master.SelectedPhaseID, UnitOfWork);
        //        this.gvCatalogs.DataBind();
        //    }
        //}

        //protected void btnCreateGroup_Click(object sender, EventArgs e)
        //{
        //    var catalogID = int.Parse(hfCatalogID.Value);
        //    var catalog = new CatalogRepository(UnitOfWork).Load(catalogID, x => x.Department);

        //    var catalogGroup = new CatalogGroup()
        //    {
        //        PhaseID = _Master.SelectedPhaseID,
        //        SupplierID = Entity.ID,
        //        InstitutionID = catalog.Department.InstitutionID,
        //        State = enCatalogGroupState.New,
        //        IsActive = true
        //    };

        //    catalog.CatalogGroup = catalogGroup;

        //    UnitOfWork.MarkAsNew(catalogGroup);
        //    UnitOfWork.Commit();

        //    Response.Redirect(string.Format("EditCatalogGroup.aspx?pID={0}&gID={1}", _Master.SelectedPhaseID, catalogGroup.ID));
        //}

        //#endregion

        //#region [ DataSource Events ]

        //protected void odsCatalogs_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        //{
        //    Criteria<Catalog> criteria = new Criteria<Catalog>();

        //    criteria.Include(x => x.Book)
        //            .Include(x => x.Department.Institution);

        //    criteria.Expression =
        //        criteria.Expression.Where(x => x.PhaseID, _Master.SelectedPhaseID)
        //                            .Where(x => x.SupplierID, Entity.ID)
        //                            .IsNull(x => x.GroupID)
        //                            .Where(x => x.Status, enCatalogStatus.Active)
        //                            .Where(string.Format("it.StateInt IN MULTISET ({0}, {1})", enCatalogState.Normal.GetValue(), enCatalogState.FromMove.GetValue()));

        //    e.InputParameters["criteria"] = criteria;
        //}

        //#endregion

        //#region [ GridView Events ]

        //protected void gvCatalogs_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        //{
        //    if (e.RowType != DevExpress.Web.GridViewRowType.Data)
        //        return;

        //    Catalog Catalog = (Catalog)gvCatalogs.GetRow(e.VisibleIndex);

        //    if (Catalog != null)
        //    {
        //        e.Row.BackColor = Color.LightGreen;
        //    }
        //}

        //protected void gvCatalogs_CustomDataCallback(object sender, ASPxGridViewCustomDataCallbackEventArgs e)
        //{
        //    var parameters = e.Parameters.Split(':');
        //    var action = parameters[0].ToLower();

        //    if (action == "refresh")
        //    {
        //        gvCatalogs.DataBind();
        //    }
        //    else
        //    {
        //        e.Result = ResolveClientUrl("~/Secure/Suppliers/SelectCatalog.aspx");
        //    }
        //}

        //#endregion
    }
}