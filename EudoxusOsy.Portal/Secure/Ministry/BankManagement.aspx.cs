using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;
using Imis.Domain;
using DevExpress.Web;
using EudoxusOsy.Portal.Controls;
using System.Web.Security;
using EudoxusOsy.Utils;
using System.Drawing;
using System.Text;

namespace EudoxusOsy.Portal.Secure.Ministry
{
    public partial class BankManagement : BaseEntityPortalPage
    {
        #region [ Button Handlers ]

        protected void btnExport_Click(object sender, EventArgs e)
        {
            gvBanks.Columns.Where(x => x.Name == "Actions").First().Visible = false;
            gvBanks.Exporter.FileName = string.Format("Banks_{0:yyyyMMdd}", DateTime.Now);
            gvBanks.Exporter.ExportWithDefaults();
        }

        #endregion

        #region [ DataSource Events ]

        protected void odsBanks_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<Bank> criteria = new Criteria<Bank>();

            criteria.Sort.OrderByDescending(x => x.ID);

            var filters = ucSearchFilters.GetSearchFilters();
            var exp = filters.GetExpression();
            if (exp != null)
                criteria.Expression = criteria.Expression.And(exp);

            e.InputParameters["criteria"] = criteria;
        }

        #endregion

        #region [ GridView Events ]

        protected void gvBanks_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.GridViewRowType.Data)
                return;

            Bank bank = (Bank)gvBanks.GetRow(e.VisibleIndex);

            if (bank != null)
            {
                e.Row.BackColor = bank.IsActive ? Color.LightGreen : Color.DarkGray;
            }
        }

        protected void gvBanks_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();

            if (action == "refresh")
            {
                gvBanks.DataBind();
                return;
            }

            var bankID = int.Parse(parameters[1]);

            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                Bank bank = new BankRepository(unitOfWork).Load(bankID);

                if (action == "delete")
                {
                    if (new CatalogGroupRepository(unitOfWork).BankExistsInCatalogGroup(bankID)
                        || new BankTransferRepository(unitOfWork).BankExistsInTransfer(bankID))
                    {
                        gvBanks.Grid.JSProperties["cpError"] = "Δεν μπορείτε να διαγράψετε τη συγκεκριμένη τράπεζα, γιατί έχει ήδη χρησιμοποιηθεί σε εκχωρήσεις.<br/><br/>Αν θέλετε η τράπεζα να μην είναι πλέον διαθέσιμη στην εφαρμογή μπορείτε να την απενεργοποιήσετε πατώντας το εικονίδιο με το λουκετάκι κάτω από τη στήλη 'Ενέργειες'";
                        return;
                    }

                    unitOfWork.MarkAsDeleted(bank);
                    unitOfWork.Commit();
                }
                else if (action == "deactivate")
                {
                    if (bank.IsActive)
                    {
                        bank.IsActive = false;
                        unitOfWork.Commit();
                    }
                }
                else if (action == "activate")
                {
                    if (!bank.IsActive)
                    {
                        bank.IsActive = true;
                        unitOfWork.Commit();
                    }
                }

                gvBanks.DataBind();
            }
        }

        #endregion

        #region [ Exporter Events ]

        protected void gveBanks_RenderBrick(object sender, DevExpress.Web.ASPxGridViewExportRenderingEventArgs e)
        {
            Bank bank = gvBanks.GetRow(e.VisibleIndex) as Bank;

            if (bank == null)
                return;

            if (e.Column.Name == "IsBank")
            {
                e.Text = bank.IsBank
                            ? "ΝΑΙ"
                            : "ΟΧΙ";
            }

            e.TextValue = e.Text;
        }

        #endregion
    }
}
