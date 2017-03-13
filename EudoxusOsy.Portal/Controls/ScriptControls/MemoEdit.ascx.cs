using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EudoxusOsy.Portal.Controls.ScriptControls
{
    public partial class MemoEdit : BaseScriptControl
    {
        #region [ Properties ]

        private const int _MaxLength = 10000;
        public string FieldDescription { get; set; }
        public bool IsRequired { get; set; }
        public bool IsReadOnly { get; set; }
        public int MaxLength { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string CounterText { get; set; }

        public string ErrorText
        {
            get
            {
                return memoArea.ValidationSettings.ErrorText;
            }
            set
            {
                memoArea.ValidationSettings.ErrorText = value;
                memoArea.ValidationSettings.RequiredField.ErrorText = value;
            }
        }

        public int Rows
        {
            get { return memoArea.Rows; }
            set
            {
                memoArea.Rows = value;
            }
        }

        public string Tooltip
        {
            get { return memoArea.ToolTip; }
            set
            {
                memoArea.CssClass = "hint";
                memoArea.ToolTip = value;
            }
        }

        public bool EnableCharCounter { get; set; }


        public string MemoValue
        {
            get
            {
                return string.IsNullOrEmpty(HttpUtility.HtmlDecode(this.hfClientState.Value))
                    ? null
                    : HttpUtility.HtmlDecode(this.hfClientState.Value).Replace("\t", " ").ToNull();
            }
            set
            {
                this.hfClientState.Value = value;
            }
        }

        public override string ClientControlPath { get { return string.Empty; } }

        protected override string ClientControlName
        {
            get { return "EudoxusOsy.Portal.Controls.ScriptControls.MemoEdit"; }
        }

        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            tbMemo.Attributes["style"] += Width > 0 ? string.Format("width: {0}px;", Width) : " width: 100%;";
            tbMemo.Attributes["style"] += Height > 0 ? string.Format("height: {0}px;", Height) : " height: 100%;";

            if (MaxLength == 0) MaxLength = _MaxLength;


            memoArea.ReadOnly = IsReadOnly;
            memoArea.ValidationSettings.RequiredField.IsRequired = IsRequired;
            if (!EnableCharCounter)
                counterRow.Attributes.Add("style", "display:none");
            base.OnPreRender(e);

        }

        #region [ IScriptControl Members ]

        public override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            var scd = new ScriptControlDescriptor(ClientControlName, ClientID);
            scd.AddProperty("isRequired", IsRequired);
            scd.AddProperty("maxLength", MaxLength);
            scd.AddProperty("decodedValue", MemoValue);
            scd.AddProperty("counterText", CounterText);
            scd.AddElementProperty("hfClientState", hfClientState.ClientID);
            scd.AddElementProperty("spCharCounter", spCharCounter.ClientID);
            scd.AddProperty("memoArea", memoArea.ClientID);

            yield return scd;
        }

        public override IEnumerable<ScriptReference> GetScriptReferences()
        {
            yield return new ScriptReference("~/Controls/ScriptControls/MemoEdit.js");
        }

        #endregion
    }
}