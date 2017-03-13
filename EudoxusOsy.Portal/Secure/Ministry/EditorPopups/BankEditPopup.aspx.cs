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
    public partial class BankEditPopup : BaseEntityPortalPage
    {
        protected List<Bank> Banks
        {
            get
            {
                return EudoxusOsyCacheManager<Bank>.Current.GetItems();
            }
        }

        protected int PhaseID
        {
            get
            {
                return int.Parse(Request.QueryString["pID"]);
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                cmbBankInput.SelectedItem = cmbBankInput.Items.FindByValue(Banks[0].ID);
                txtBankName.Text = Banks[0].Name;
                chkIsBankEdit.Checked = Banks[0].IsBank;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

        }


        protected void btnAddNewBank_Click(object sender, EventArgs e)
        {

            Bank newBank = new Bank();
            newBank.IsActive = true;
            newBank.IsBank = chkIsBank.Checked;
            newBank.Name = txtBankNameInput.Text;
            newBank.CreatedAt = DateTime.Today;
            newBank.CreatedBy = User.Identity.Name;
            UnitOfWork.MarkAsNew(newBank);
            UnitOfWork.Commit();
            doFillBanks();
            txtBankNameInput.Text = null;
            chkIsBank.Checked = false;
        }

        protected void cmbBankInput_Init(object sender, EventArgs e)
        {
                doFillBanks();
        }

        private void doFillBanks()
        {
            EudoxusOsyCacheManager<Bank>.Current.Flush();
            EudoxusOsyCacheManager<Bank>.Current.Refresh();
            cmbBankInput.DataSource = Banks;
            cmbBankInput.DataBind();
        }

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            var selectedItemID = cmbBankInput.GetSelectedInteger();
            if (selectedItemID.HasValue)
            {
                var bank = new BankRepository(UnitOfWork).Load(selectedItemID.Value);
                bank.Name = txtBankName.Text;
                bank.IsBank = chkIsBankEdit.Checked;
                UnitOfWork.Commit();
                Banks.FirstOrDefault(x => x.ID == bank.ID).Name = bank.Name;
            }
            cmbBankInput.DataSource = null;
            cmbBankInput.SelectedItem = cmbBankInput.Items.FindByValue(selectedItemID.Value);
        }

        protected void btnDeleteBank_Click(object sender, EventArgs e)
        {
            var selectedBankID = cmbBankInput.GetSelectedInteger();
            if (selectedBankID.HasValue)
            {
                var bankToDelete = new BankRepository(UnitOfWork).Load(selectedBankID.Value);
                UnitOfWork.MarkAsDeleted(bankToDelete);
                UnitOfWork.Commit();

                cmbBankInput.SelectedItem = cmbBankInput.Items.FindByValue(Banks[0].ID);
                txtBankName.Text = Banks[0].Name;
                chkIsBankEdit.Checked = Banks[0].IsBank;
                doFillBanks();
            }
        }

        protected void cmbBankInput_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItemID = cmbBankInput.GetSelectedInteger();
            if(selectedItemID.HasValue)
            {
                var bank = new BankRepository(UnitOfWork).Load(selectedItemID.Value);
                txtBankName.Text = bank.Name;
                chkIsBankEdit.Checked = bank.IsBank;
            }
        }
    }
}
