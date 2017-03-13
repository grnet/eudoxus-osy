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

namespace EudoxusOsy.Portal.Secure.Ministry
{
    public partial class SelectPhase : BaseEntityPortalPage<Reporter>
    {
        #region [ Entity Fill ]

        protected override void Fill()
        {
            Entity = new ReporterRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);
            Entity.SaveToCurrentContext();
        }

        #endregion

        #region [ Page Inits ]

        protected void Page_Init(object sender, EventArgs e)
        {
            gvPhases.PageSize = int.MaxValue;
        }

        protected void ddlSelectPhase_Init(object sender, EventArgs e)
        {
            ddlSelectPhase.FillPhases(true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var currentPhaseID = EudoxusOsyCacheManager<Phase>.Current.GetItems().Max(x=> x.ID);
            btnCreateCatalogs.Text = "Δημιουργία Διανομών για την περίοδο " + currentPhaseID;
            var currentPhase = EudoxusOsyCacheManager<Phase>.Current.Get(currentPhaseID);
            if (currentPhase.CatalogsCreated)
            {
                btnCreateCatalogs.ClientEnabled = false;
            }
        }

        #endregion

        #region [ DataSource Events ]

        protected void odsPhases_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<Phase> criteria = new Criteria<Phase>();

            e.InputParameters["criteria"] = criteria;
        }

        #endregion

        #region [ GridView Events ]

        protected void gvPhases_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.GridViewRowType.Data)
                return;

            Phase phase = (Phase)gvPhases.GetRow(e.VisibleIndex);

            //if (phase != null)
            //{
            //    if (phase.ID == ((Ministry)Page.Master).SelectedPhaseID)
            //    {
            //        e.Row.BackColor = Color.LightGreen;
            //    }
            //}
        }

        protected void gvPhases_CustomDataCallback(object sender, ASPxGridViewCustomDataCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();

            if (action == "refresh")
            {
                gvPhases.DataBind();
            }
            else
            {
                int selectedPhaseID = int.Parse(parameters[1]);
                //((Ministry)Page.Master).SelectedPhaseID = selectedPhaseID;

                e.Result = ResolveClientUrl(string.Format("SelectPhase.aspx?pID={0}", selectedPhaseID));
            }
        }

        #endregion

        #region [ GridView Methods ]

        protected bool IsSelected(Phase phase)
        {

            return false;
            //if (phase == null)
            //    return false;

            //return phase.ID == ((Ministry)Page.Master).SelectedPhaseID;
        }

        #endregion

        protected void cbSubmitAmount_Callback(object source, CallbackEventArgs e)
        {
            if (ddlSelectPhase.GetSelectedInteger().HasValue)
            {
                int phaseID = ddlSelectPhase.GetSelectedInteger().Value;
                var amount = Convert.ToDecimal(txtPhaseAmount.Text);
                var amountLimit = Convert.ToDecimal(txtAmountLimit.Text);
                var isMoneySetOK = PhaseHelper.SetMoney(amount, amountLimit, phaseID);
                if(!isMoneySetOK)
                {
                    cbSubmitAmount.JSProperties.Add("cperror",true);
                }
            }
        }

        protected void btnCreateCatalogs_Click(object sender, EventArgs e)
        {
            var currentPhaseID = EudoxusOsyCacheManager<Phase>.Current.GetItems().Max(x => x.ID);
            new CatalogRepository(UnitOfWork).CreateCatalogsForPhase(currentPhaseID);
            var currentPhase = new PhaseRepository(UnitOfWork).Load(currentPhaseID);
            currentPhase.CatalogsCreated = true;
            UnitOfWork.Commit();
            Response.Redirect("Catalogs.aspx?fpID=" + currentPhaseID);
        }
    }
}