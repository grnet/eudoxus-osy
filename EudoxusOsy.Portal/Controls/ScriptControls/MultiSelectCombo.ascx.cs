using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using EudoxusOsy.BusinessModel;

namespace EudoxusOsy.Portal.Controls.ScriptControls
{
    public partial class MultiSelectCombo : BaseScriptControl, IScriptControl
    {
        #region [ Properties ]

        public const int _DefaultHeight = 150;
        public const string _DefaultEmptyFieldText = "-- αδιάφορο --";

        private ASPxListBox _ListBox = null;
        protected ASPxListBox ListBox
        {
            get { return (_ListBox ?? (_ListBox = (ASPxListBox)ddxDropDownEdit.FindControlRecursive("ddlListBox"))); }
            set { _ListBox = value; }
        }

        private string _EmptyFieldText = null;
        protected string EmptyFieldText
        {
            get { return (_EmptyFieldText ?? (_EmptyFieldText = _DefaultEmptyFieldText)); }
            set { _EmptyFieldText = value; }
        }

        public string FieldDescription { get; set; }
        public bool IsRequired { get; set; }
        public bool IsReadOnly { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public ASPxDropDownEdit DropDownEdit
        {
            get
            {
                return ddxDropDownEdit;
            }
        }

        public string ErrorText
        {
            get
            {
                return ddxDropDownEdit.ValidationSettings.ErrorText;
            }
            set
            {
                ddxDropDownEdit.ValidationSettings.ErrorText = value;
                ddxDropDownEdit.ValidationSettings.RequiredField.ErrorText = value;
            }
        }

        private string _valGroup = string.Empty;
        public string ValidationGroup
        {
            get { return _valGroup; }
            set
            {
                ddxDropDownEdit.ValidationSettings.ValidationGroup =
                ListBox.ValidationSettings.ValidationGroup =
                _valGroup = value;
            }
        }

        protected override string ClientControlName
        {
            get { return "EudoxusOsy.Portal.Controls.ScriptControls.MultiSelectCombo"; }
        }

        #endregion

        #region [ Control Inits ]

        public void SetDataSource(IEnumerable<Tuple<string, object>> items)
        {
            var firstItem = items.FirstOrDefault();
            if (firstItem != null)
                ListBox.ValueType = firstItem.Item2.GetType();

            SetDataSource(items.Select(x => new ListEditItem(x.Item1, x.Item2)));
        }

        public void SetDataSource(IEnumerable<ListEditItem> items, bool allSelected = false)
        {
            foreach (var item in items)
            {
                ListBox.Items.Add(item);
            }

            if (allSelected)
            {
                foreach (ListEditItem item in ListBox.Items)
                {
                    item.Selected = true;
                }
            }
        }

        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            ListBox.Height = Height > 0 ? new Unit(Height, UnitType.Pixel) : new Unit(_DefaultHeight, UnitType.Pixel);

            ddxDropDownEdit.ReadOnly = IsReadOnly;
            ddxDropDownEdit.ClientEnabled = !IsReadOnly;

            if (string.IsNullOrEmpty(ddxDropDownEdit.Text))
            {
                ddxDropDownEdit.Text = EmptyFieldText;
            }

            base.OnPreRender(e);
        }

        #region [ Bind And Extract Value ]

        public void Bind(List<object> selectedValues)
        {
            foreach (ListEditItem item in ListBox.Items)
            {
                item.Selected = false;
            }

            List<string> selectedValuesString = new List<string>();

            foreach (var item in selectedValues)
            {
                var selectedItem = ListBox.Items.FindByValue(item);

                selectedItem.Selected = true;
                selectedValuesString.Add(selectedItem.Text);
            }

            ddxDropDownEdit.Text = string.Join(";", selectedValuesString);
            hfClientState.Value = string.Join(";", selectedValuesString);
        }

        public object ExtractValue()
        {
            return string.Join(",", SelectedValues);
        }

        public List<string> SelectedValues
        {
            get
            {
                if (!string.IsNullOrEmpty(hfClientState.Value))
                    return hfClientState.Value.Split(';').ToList();
                else
                    return new List<string>();
            }
        }

        #endregion

        #region [ IScriptControl Members ]

        public override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            var scd = new ScriptControlDescriptor(ClientControlName, ClientID);

            scd.AddProperty("isRequired", IsRequired);
            scd.AddProperty("emptyFieldText", EmptyFieldText);
            scd.AddScriptProperty("dropDownEdit", ddxDropDownEdit.ClientID);
            scd.AddScriptProperty("listBox", ListBox.ClientID);
            scd.AddScriptProperty("hfClientState", hfClientState.ClientID);
            scd.AddProperty("proposalFieldName", FieldDescription);
            scd.AddProperty("validationGroup", ValidationGroup);

            yield return scd;
        }

        public override IEnumerable<ScriptReference> GetScriptReferences()
        {
            yield return new ScriptReference("~/Controls/ScriptControls/MultiSelectCombo.js");
        }

        #endregion
    }
}