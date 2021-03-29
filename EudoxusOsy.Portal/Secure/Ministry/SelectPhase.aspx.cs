using DevExpress.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

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

        protected void ddlSelectSupplierPhase_Init(object sender, EventArgs e)
        {
            ddlSelectSupplierPhase.FillPhases(true);
        }

        protected void ddlSelectSupplierPhaseMinistry_Init(object sender, EventArgs e)
        {
            ddlSelectSupplierPhaseMinistry.FillPhases(true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var currentPhaseID = EudoxusOsyCacheManager<Phase>.Current.GetItems().Max(x => x.ID);
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
            criteria.Expression = criteria.Expression.Where(x => x.IsActive, true);

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

                int numOfcatalogs = new CatalogRepository().CountByPhaseID(phaseID);

                var amount = Convert.ToDecimal(txtPhaseAmount.Text);
                var amountLimit = Convert.ToDecimal(txtAmountLimit.Text);

                if (numOfcatalogs > 0)
                {
                    var isMoneySetOK = PhaseHelper.SetMoney(amount, amountLimit, phaseID);

                    if (!isMoneySetOK)
                    {
                        cbSubmitAmount.JSProperties.Add("cperror", true);
                    }
                }
                else
                {
                    cbSubmitAmount.JSProperties.Add("cperrorcatalogs", true);
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

        protected void cbRemoveExtraSupplierAmount_Callback(object source, CallbackEventArgs e)
        {
            if(!ddlSelectSupplierPhase.GetSelectedInteger().HasValue)
            {
                cbRemoveExtraSupplierAmount.JSProperties.Add("cperror", true);
                return;
            } 

            var phaseID = ddlSelectSupplierPhase.GetSelectedInteger().Value;

            try
            {                                
                if (!new SupplierPhaseRepository().GetPhaseMoney(phaseID).HasValue)
                    return;

                var catalogsDictionary = new CatalogRepository().GetSupplierAmountPerPhase(phaseID);
                var supplierPhases = new SupplierPhaseRepository(UnitOfWork).GetAllActive(phaseID);

                bool noExtraAmount = false;

                foreach (var supplierPhase in supplierPhases)
                {
                    if (!catalogsDictionary.ContainsKey(supplierPhase.SupplierID))
                        continue;

                    decimal? amount;

                    if (!catalogsDictionary.TryGetValue(supplierPhase.SupplierID, out amount)) continue;
                    if (amount != null && supplierPhase.TotalDebt > (double) amount.Value)
                    {
                        noExtraAmount = true;
                        supplierPhase.TotalDebt = (double) amount;
                    }
                }

                if (!noExtraAmount)
                {
                    cbRemoveExtraSupplierAmount.JSProperties.Add("cpnoextraamount", true);
                    return;
                }
                UnitOfWork.Commit();                                              
            }
            catch (Exception ex)
            {
                cbRemoveExtraSupplierAmount.JSProperties.Add("cperror", true);
                return;
            }
                        
            try
            {
                var phase = new PhaseRepository(UnitOfWork).Load(phaseID);
                phase.PhaseAmount = Math.Round(new SupplierPhaseRepository(UnitOfWork).GetPhaseMoney(phaseID).Value,
                    2, MidpointRounding.AwayFromZero);
                UnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                cbRemoveExtraSupplierAmount.JSProperties.Add("cperror", true);                    
            }
        
        }

        
        protected void cbPhaseAmountMinistry_Callback(object source, CallbackEventArgs e)
        {
            if(!ddlSelectSupplierPhaseMinistry.GetSelectedInteger().HasValue)
            {
                cbPhaseAmountMinistry.JSProperties.Add("cperror", true);
                return;
            } 

            var phaseID = ddlSelectSupplierPhaseMinistry.GetSelectedInteger().Value;
                                   
            try
            {
                var phase = new PhaseRepository(UnitOfWork).Load(phaseID);
                phase.PhaseAmountMinistry =  Convert.ToDecimal(txtPhaseAmountMinistry.Text);
                UnitOfWork.Commit();                
            }
            catch (Exception ex)
            {
                cbPhaseAmountMinistry.JSProperties.Add("cperror", true);                    
            }

        }
    }
}