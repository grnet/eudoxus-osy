using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;
using System.Web.Script.Serialization;

namespace EudoxusOsy.Portal.Controls.ScriptControls
{
    public partial class InvoiceItemCollectionEdit : BaseScriptControl
    {
        #region [ Properties ]

        public CatalogGroupInfo CatalogGroupInfo { get; set; }

        protected InvoiceItemCollection Collection { get; private set; }

        protected override string ClientControlName
        {
            get { return "EudoxusOsy.Portal.Controls.ScriptControls.InvoiceItemCollectionEdit"; }
        }

        public string ValidationGroup
        {
            get { return cvRequired.ValidationGroup; }
            set { cvRequired.ValidationGroup = value; }
        }

        public string FieldName { get; set; }

        public bool IsRequired { get; set; }

        public bool ReadOnly { get; set; }

        #endregion

        #region [ Bind and ExtractValue ]

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (Collection == null && !string.IsNullOrEmpty(hfClientState.Value))
            {
                InvoiceItem[] items = new JavaScriptSerializer().Deserialize<InvoiceItem[]>(hfClientState.Value);
                Collection = new InvoiceItemCollection(items);
            }

            cvRequired.ErrorMessage = string.Format(cvRequired.ErrorMessage, FieldName);
            seDate.MaxDate = DateTime.Today;

            base.OnPreRender(e);
        }

        public void Bind(InvoiceItemCollection collection)
        {
            Collection = collection;
        }

        public InvoiceItemCollection ExtractValue()
        {
            InvoiceItem[] items = new JavaScriptSerializer().Deserialize<InvoiceItem[]>(hfClientState.Value);

            if (items != null)
            {
                items.Where(c => c.ID == Guid.Empty).ToList().ForEach(c => c.ID = Guid.NewGuid());

                return new InvoiceItemCollection(items);
            }

            return new InvoiceItemCollection();
        }

        #endregion

        #region [ IScriptControl Members ]

        public override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            var editControlClientInstances = new Dictionary<string, string>();
            editControlClientInstances.Add("InvoiceNumber", tbDescription.ClientID);
            editControlClientInstances.Add("Date", seDate.ClientID);
            editControlClientInstances.Add("Amount", seUnitPrice.ClientID);

            ScriptControlDescriptor scd = new ScriptControlDescriptor(ClientControlName, ClientID);
            scd.AddProperty("records", Collection ?? new InvoiceItemCollection());
            scd.AddProperty("recordsCount", Collection == null ? 0 : Collection.Count);
            scd.AddProperty("inputControls", editControlClientInstances);
            scd.AddProperty("popupControl", dxEditPopup.ClientID);
            scd.AddProperty("isRequired", IsRequired);
            scd.AddProperty("readOnly", ReadOnly);
            scd.AddProperty("totalAmount", CatalogGroupInfo.TotalAmount);            
            scd.AddElementProperty("clientStateField", hfClientState.ClientID);
            scd.AddElementProperty("addHandle", lnkAddItem.ClientID);
            scd.AddElementProperty("addHandleFooter", lnkAddItemFooter.ClientID);
            scd.AddElementProperty("submitHandle", dxEditPopup.FindControl("btnSubmit").ClientID);
            scd.AddElementProperty("cancelHandle", dxEditPopup.FindControl("btnCancel").ClientID);
            scd.AddElementProperty("cvRequired", cvRequired.ClientID);
            scd.AddElementProperty("cvFinancialChecks", cvFinancialChecks.ClientID);
            scd.AddElementProperty("cvCurrentAmount", cvCurrentAmount.ClientID);

            yield return scd;
        }

        public override IEnumerable<ScriptReference> GetScriptReferences()
        {
            yield return new ScriptReference("~/Controls/ScriptControls/InvoiceItemCollectionEdit.js");
        }

        #endregion
    }
}