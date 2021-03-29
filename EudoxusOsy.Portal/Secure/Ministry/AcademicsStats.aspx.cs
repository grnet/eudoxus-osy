using DevExpress.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Configuration;
using System.Web.UI.WebControls;

namespace EudoxusOsy.Portal.Secure.Ministry
{
    public partial class AcademicsStats : BaseEntityPortalPage
    {
        protected string ConnStr;


        protected void Page_Load(object sender, EventArgs e)
        {
            dllPhase.FillPhases(true);

        }


        #region [ Button Handlers ]



        #endregion

        #region [ DataSource Events ]

        protected void odsSuppliers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<Supplier> criteria = new Criteria<Supplier>();

            criteria.Include(x => x.Reporter)
                .Include(x => x.SupplierDetail)
                .Include(x => x.SupplierIBANs);

            criteria.Sort.OrderBy(x => x.ID);



            e.InputParameters["criteria"] = criteria;
        }

        #endregion

        #region [ GridView Events ]

        #endregion

        #region [ GridView Methods ]

        protected void gvAcademics_OnCustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();

            if (action == "refresh")
            {
                SqlDataSource.SelectCommand =
                    "SELECT vs.Debt AS Debt, vs.InstName AS Name, DepartmentCount, vs.InstitutionID AS ID " +
                    "FROM report.ViewStatisticsPerInstitution" + Get_PP() + " vs WHERE PhaseID = @phaseId AND vs.InstName like @Name";

                gvAcademics.DataBind();
                return;
            }

        }

        #endregion

        private string Get_PP()
        {
            var currenPhase = new PhaseRepository().GetCurrentPhase();

            if (dllPhase.GetSelectedInteger() > 0)
            {
                return (currenPhase.ID == dllPhase.GetSelectedInteger() ? "" : "_PP");
            }

            return "";
        }

        protected void gvAcademics_OnInit(object sender, EventArgs e)
        {
            SqlDataSource.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
        }

        protected void SqlDataSource_OnSelecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@phaseId"].Value = dllPhase.GetSelectedInteger();
            e.Command.Parameters["@Name"].Value = "%" + txtInistitutionName.GetText() + "%";
        }

        protected void SqlDataSourceInstitution_OnSelecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@phaseId"].Value = dllPhase.GetSelectedInteger();
        }

        protected void SqlDataSourceDepartment_OnSelecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@phaseId"].Value = dllPhase.GetSelectedInteger();
        }

        protected void btnExportIntitutions_Click(object sender, EventArgs e)
        {
            string fileName = string.Format("ExportIntitutions_{0}", DateTime.Now.ToString("yyyyMMdd"));

            SqlDataSourceInstitution.SelectCommand =
                "SELECT * FROM report.ViewStatisticsPerInstitution" + Get_PP() + " WHERE PhaseID = @phaseId";

            gvInstExport.Grid.DataBind();
            gvInstExport.Exporter.WriteXlsxToResponse(fileName);
        }

        protected void btnExportDepartments_Click(object sender, EventArgs e)
        {
            string fileName = string.Format("ExportDepartments_{0}", DateTime.Now.ToString("yyyyMMdd"));

            SqlDataSourceDepartment.SelectCommand = "SELECT * FROM report.ViewStatisticsPerDepartment" + Get_PP() + " WHERE PhaseID = @phaseId";

            gvDepExport.Grid.DataBind();
            gvDepExport.Exporter.WriteXlsxToResponse(fileName);
        }

        protected void gvInstExport_OnCustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();

            if (action == "refresh")
            {
                gvInstExport.DataBind();
                return;
            }
        }

        protected void gvDepExport_OnCustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();

            if (action == "refresh")
            {
                gvDepExport.DataBind();
                return;
            }
        }

        protected void gvInstExport_OnInit(object sender, EventArgs e)
        {
            SqlDataSourceInstitution.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
        }

        protected void gvDepExport_OnInit(object sender, EventArgs e)
        {
            SqlDataSourceDepartment.ConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("SuppliersStats.aspx");
        }
    }
}