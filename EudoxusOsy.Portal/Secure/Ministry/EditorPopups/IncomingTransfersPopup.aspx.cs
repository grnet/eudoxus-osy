using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.Portal.Controls;
using EudoxusOsy.BusinessModel;
using System.Web.Security;
using DevExpress.Web;
using EudoxusOsy.Portal.Utils;

namespace EudoxusOsy.Portal.Secure.Ministry.EditorPopups
{
    public partial class IncomingTransfersPopup : BaseEntityPortalPage<Supplier>
    {
        protected int PhaseID
        {
            get
            {
                return int.Parse(Request.QueryString["pID"]);
            }

        }

        protected override void Fill()
        {
            int reporterID;
            if (int.TryParse(Request.QueryString["id"], out reporterID) && reporterID > 0)
            {
                Entity = new SupplierRepository(UnitOfWork).Load(reporterID);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

        }

        protected void odsIncomingTransfers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<BankTransfer> criteria = new Criteria<BankTransfer>();
            criteria.Expression = ucIncomingTransfersSearchFilters.GetSearchFilters().GetExpression();

            if (criteria.Expression == null)
            {
                criteria.Expression = Imis.Domain.EF.Search.Criteria<BankTransfer>.Empty;
            }

            criteria.Expression = criteria.Expression.Where(x => x.SupplierID, Entity.ID);
            criteria.Expression = criteria.Expression.Where(x => x.PhaseID, PhaseID);
            e.InputParameters["criteria"] = criteria;
        }

        protected void ucIncomingTransfersInvoicesGridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();

            if (action == "refresh")
            {
                ucIncomingTransfersInvoicesGridView.DataBind();
            }
            else if(action == "delete" && EudoxusOsyRoleProvider.IsAuthorizedEditorUser())
            {
                var id = Convert.ToInt32(parameters[1]);
                var invoice = new BankTransferRepository(UnitOfWork).Load(id);
                UnitOfWork.MarkAsDeleted(invoice);
                UnitOfWork.Commit();

                ucIncomingTransfersInvoicesGridView.DataBind();
            }
        }

        protected void btnAddNewTransfer_Click(object sender, EventArgs e)
        {
            var newTransfer = new BankTransfer();
            newTransfer.InvoiceNumber = txtInvoiceNumberInput.Text;
            newTransfer.InvoiceValue = (decimal)spinAmountInput.Value;
            newTransfer.InvoiceDate = dateInvoiceDateInput.Date;
            newTransfer.SupplierID = Entity.ID;
            newTransfer.IsActive = true;
            newTransfer.PhaseID = PhaseID;
            newTransfer.BankID = (int)cmbBankInput.GetSelectedInteger();
            newTransfer.CreatedAt = DateTime.Today;
            newTransfer.CreatedBy = User.Identity.Name;

            UnitOfWork.MarkAsNew(newTransfer);
            UnitOfWork.Commit();

            ucIncomingTransfersInvoicesGridView.Grid.DataBind();
            txtInvoiceNumberInput.Text = null;
            dateInvoiceDateInput.Value = null;
            cmbBankInput.Value = null;
            spinAmountInput.Value = null;
        }

        protected void cmbBankInput_Init(object sender, EventArgs e)
        {
            var banks = EudoxusOsyCacheManager<Bank>.Current.GetItems().Where(x => x.IsBank);
            cmbBankInput.DataSource = banks;
            cmbBankInput.DataBind();
        }
    }
}
