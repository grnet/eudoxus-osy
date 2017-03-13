using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EudoxusOsy.Portal.Secure.Ministry.EditorPopups
{
    public partial class SelectPhasePopup : BaseEntityPortalPage
    {
        protected string Type
        {
            get
            {
                return Request.QueryString["type"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(Type== "nolog" || Type == "commit")
            {
                divSelectPhase.Visible = false;
                divSelectYear.Visible = true;
                divMoveToPhase.Visible = false;
            }
            else if(Type == "coauthors")
            {
                divSelectPhase.Visible = true;
                divSelectYear.Visible = false;
                divMoveToPhase.Visible = false;
            }
            else if(Type == "move")
            {
                divMoveToPhase.Visible = true;
                divSelectPhase.Visible = false;
                divSelectYear.Visible = false;
            }
            else if (Type == "supplierstats")
            {
                divMoveToPhase.Visible = false;
                divSelectPhase.Visible = true;
                divSelectYear.Visible = false;
            }
            else 
            {
                divSelectPhase.Visible = true;
                divSelectYear.Visible = false;
                divMoveToPhase.Visible = false;
            }
        }

        protected void btnSubmitHidden_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtPhaseID.Text) && string.IsNullOrEmpty(ddlSelectYear.Text))
            {
                return;
            }

            if (Type == "nolog")
            {
                int? yearInt = ddlSelectYear.GetSelectedInteger();
                var isYear = yearInt.HasValue;
                List<SuppliersNoLogisticBooks> suppliersNoLogisticBooks = new List<SuppliersNoLogisticBooks>();
                if (isYear && yearInt > 0)
                {
                    suppliersNoLogisticBooks = new SupplierRepository(UnitOfWork).GetSuppliersNoLogisticBooks(yearInt.Value);
                }
                else
                {
                    suppliersNoLogisticBooks = new SupplierRepository(UnitOfWork).GetSuppliersNoLogisticBooks(0);
                }

                gveNoLogisticBooks.Export(suppliersNoLogisticBooks, "no_logistics_year_" + (yearInt > 0 ? yearInt.ToString() : "all_phases"));
            }
            else if(Type == "coauthors")
            {
                var phaseID = int.Parse(txtPhaseID.Text);
                var coAuthors = new SupplierRepository(UnitOfWork).GetCoAuthors(phaseID);
                ucCoAuthorExport.Export(coAuthors, "coauthors_info");
            }
            else if (Type == "supplierstats")
            {
                var phaseID = int.Parse(txtPhaseID.GetText());
                var supplierStats = new SupplierRepository(UnitOfWork).GetSupplierStatsForExport(phaseID);
                ucSupplierStats.Export(supplierStats, "supplier_stats_info");
            }
            else if (Type == "move")
            {
                var phaseID = int.Parse(txtMoveToPhaseID.Text);
                ClientScript.RegisterStartupScript(GetType(), "hidePopup", "window.parent.doAction('movetophase', " + phaseID +", 'PaymentOrders');window.parent.popUp.hide();", true);
            }
            else if(Type == "commit")
            {

                int? yearInt = ddlSelectYear.GetSelectedInteger();
                var isYear = yearInt.HasValue;

                int phaseID = 0;
                Phase phase = new Phase();
                if (isYear)
                {
                    phase = CacheManager.Phases.GetItems().First(x => x.Year == yearInt);
                    phaseID = phase.ID;
                }

                var commitmentsRegistry = new SupplierRepository(UnitOfWork).GetCommitments(phaseID);
                gveExportCommitments.Export(commitmentsRegistry, "commitmentRecords_phase_" + phase.AcademicYearString);
            }
        }

        protected void ddlSelectYear_Init(object sender, EventArgs e)
        {
            ddlSelectYear.FillYears(false, "-- επιλέξτε ακαδημαϊκό έτος --");
        }
    }
}