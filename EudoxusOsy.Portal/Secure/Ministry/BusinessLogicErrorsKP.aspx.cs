using DevExpress.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Web.UI.WebControls;
using System.Linq.Expressions;
using System.Linq;

namespace EudoxusOsy.Portal.Secure.Ministry
{
    public partial class BusinessLogikErrorsKP : BaseEntityPortalPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void gvBookPriceChange_OnCustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();
            gvBusinessLogicErrors.DataBind();
        }

        protected void odsBusinessLogicErrorsKP_OnSelecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<BusinessLogicErrorsKP> criteria = new Criteria<BusinessLogicErrorsKP>();
            if (!string.IsNullOrEmpty(txtBookKpsID.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.EntityID, int.Parse(txtBookKpsID.Text));
            }

            if (dateErrorDate.GetDate().HasValue)
            {
                criteria.Expression = criteria.Expression.Where(x => x.CreatedAt, dateErrorDate.GetDate().Value, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThanEquals);
                criteria.Expression = criteria.Expression.Where(x => x.CreatedAt, dateErrorDate.GetDate().Value.AddDays(1), Imis.Domain.EF.Search.enCriteriaOperator.LessThan);
            }

            criteria.Sort.OrderBy(x => x.ID);

            e.InputParameters["criteria"] = criteria;
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("BusinessLogicErrorsKP.aspx");
        }

    }
}