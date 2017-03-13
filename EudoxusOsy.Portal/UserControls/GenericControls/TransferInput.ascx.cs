using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;

namespace EudoxusOsy.Portal.UserControls.GenericControls
{
    public partial class TransferInput : BaseEntityUserControl<BankTransfer>
    {
        #region [ Control Inits ]

        protected void ddlBank_Init(object sender, EventArgs e)
        {
            ddlBank.FillBanks(true);
        }

        protected void ddlPhase_Init(object sender, EventArgs e)
        {
            ddlPhase.FillPhases(true);
        }

        #endregion

        #region [ Extract - Bind ]

        public override void Bind()
        {
            txtInvoiceNumber.Text = Entity.InvoiceNumber;
            txtInvoiceDate.Date = Entity.InvoiceDate;
            txtInvoiceValue.Value = Entity.InvoiceValue;
            ddlBank.SelectedItem = ddlBank.Items.FindByValue(Entity.BankID);
            ddlPhase.SelectedItem = ddlPhase.Items.FindByValue(Entity.PhaseID);
        }

        public override BankTransfer Fill(BankTransfer entity)
        {
            entity.InvoiceNumber = txtInvoiceNumber.GetText();
            entity.InvoiceDate = txtInvoiceDate.GetDate().Value;
            entity.InvoiceValue = txtInvoiceValue.GetDecimal().Value;
            entity.BankID = ddlBank.GetSelectedInteger().Value;
            entity.PhaseID = ddlPhase.GetSelectedInteger().Value;

            return entity;
        }

        #endregion

        #region [ Validation ]

        public string ValidationGroup
        {
            get { return txtInvoiceNumber.ValidationSettings.ValidationGroup; }
            set
            {
                foreach (var control in this.RecursiveOfType<ASPxEdit>())
                    control.ValidationSettings.ValidationGroup = value;
            }
        }

        #endregion

        #region [ Properties ]

        public bool ReadOnly
        {
            get { return txtInvoiceNumber.ReadOnly; }
            set
            {
                foreach (var control in this.RecursiveOfType<ASPxEdit>())
                {
                    control.ReadOnly = value;
                    control.ClientEnabled = !value;
                }
            }
        }

        #endregion
    }
}