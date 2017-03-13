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

namespace EudoxusOsy.Portal.UserControls.InvoiceControls.InputControls
{
    public partial class InvoiceInput : BaseEntityUserControl<Invoice>
    {
        #region [ Extract - Bind ]

        public override void Bind()
        {
            txtInvoiceNumber.Text = Entity.InvoiceNumber;
            txtInvoiceDate.Date = Entity.InvoiceDate;
            txtInvoiceValue.Value = Entity.InvoiceValue;
        }

        public override Invoice Fill(Invoice entity)
        {
            entity.InvoiceNumber = txtInvoiceNumber.GetText();
            entity.InvoiceDate = txtInvoiceDate.GetDate().Value;
            entity.InvoiceValue = txtInvoiceValue.GetDecimal().Value;

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