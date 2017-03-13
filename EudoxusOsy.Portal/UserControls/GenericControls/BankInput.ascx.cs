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
    public partial class BankInput : BaseEntityUserControl<Bank>
    {
        #region [ Control Inits ]

        protected void ddlIsBank_Init(object sender, EventArgs e)
        {
            ddlIsBank.FillTrueFalse("-- παρακαλώ επιλέξτε --");
        }

        #endregion

        #region [ Extract - Bind ]

        public override void Bind()
        {
            txtName.Text = Entity.Name;
            ddlIsBank.SelectedItem = ddlIsBank.Items.FindByValue((Entity.IsBank ? 1 : 0));
        }

        public override Bank Fill(Bank entity)
        {
            entity.Name = txtName.GetText();
            entity.IsBank = ddlIsBank.GetSelectedBoolean().Value;

            return entity;
        }

        #endregion

        #region [ Validation ]

        public string ValidationGroup
        {
            get { return txtName.ValidationSettings.ValidationGroup; }
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
            get { return txtName.ReadOnly; }
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