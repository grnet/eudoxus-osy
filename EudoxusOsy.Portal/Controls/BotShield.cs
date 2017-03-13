using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BotDetect;
using BotDetect.Web;
using BotDetect.Web.UI;
using DevExpress.Web;

namespace EudoxusOsy.Portal.Controls
{
    [ToolboxData("<{0}:BotShield runat=server></{0}:BotShield>")]
    public class BotShield : WebControl, INamingContainer
    {
        private ITemplate contentTemplate;

        [TemplateContainer(typeof(BotShield))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate ContentTemplate
        {
            get { return contentTemplate; }
            set { contentTemplate = value; }
        }

        protected virtual ITemplate CreateDefaultTemplate()
        {
            return new LanapTemplate();
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();
            ITemplate tmpl = contentTemplate != null ? contentTemplate : CreateDefaultTemplate();
            tmpl.InstantiateIn(this);
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Captcha c = FindControl(SHIELD_ID) as Captcha;

            if (c != null)
            {
                Array a = Enum.GetValues(typeof(ImageStyle));
                Random r = new Random();
                int x = r.Next(a.Length);
                c.ImageStyle = (ImageStyle)a.GetValue(x);
            }

            if (this.Page != null && !this.Page.ClientScript.IsClientScriptIncludeRegistered(this.GetType(), SCRIPT_KEY))
            {
                if (c != null)
                    this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), SCRIPT_KEY + "1", string.Format("var BS_MAX_LENGTH = {0}; var DEFAULT_BS_ERROR_MESSAGE = '{1}';", c.CodeLength, DEFAULT_ERROR_MESSAGE), true);
            }

            if (Page.IsPostBack)
            {
                ASPxTextBox formShieldTextBox = FindControl(TEXT_ID) as ASPxTextBox;

                if (!formShieldTextBox.IsValid)
                {
                    formShieldTextBox.Text = string.Empty;
                }
            }
        }

        public string ValidationGroup
        {
            get
            {
                return (FindControl(TEXT_ID) as ASPxTextBox).ValidationSettings.ValidationGroup;
            }
            set
            {
                (FindControl(TEXT_ID) as ASPxTextBox).ValidationSettings.ValidationGroup = value;
            }
        }

        public const string LABEL_ID = "lblFormShield";
        public const string SHIELD_ID = "formShield";
        public const string TEXT_ID = "txtFormShield";

        public const string REQUIRED_FIELD_ERROR_MESSAGE = "Παρακαλώ συμπληρώστε το κείμενο που εμφανίζεται στην εικόνα";
        public const string DEFAULT_ERROR_MESSAGE = "Παρακαλώ συμπληρώστε σωστά το κείμενο που εμφανίζεται στην εικόνα";

        public const string SCRIPT_KEY = "EudoxusOsy.Portal.Controls.ChangeBotShieldInput";
    }

    sealed class LanapTemplate : ITemplate
    {
        private bool? isValid = null;
        private Label lt = new Label();
        private Table tbl = new Table();
        private Captcha captcha = new Captcha();
        private ASPxTextBox txtFormShield = new ASPxTextBox();

        internal LanapTemplate()
        {
            lt.Text = "<em>Γράψτε τους χαρακτήρες που εμφανίζονται στην εικόνα</em>";
            lt.ID = BotShield.LABEL_ID;
            lt.AssociatedControlID = "txtFormShield";

            TableRow tr;
            TableCell tc;

            tbl.Rows.Add(tr = new TableRow());

            tr.Cells.Add(tc = new TableCell());
            captcha.ID = BotShield.SHIELD_ID;
            tc.Controls.Add(captcha);

            tr.Cells.Add(tc = new TableCell());
            txtFormShield.ID = BotShield.TEXT_ID;
            txtFormShield.ClientInstanceName = BotShield.TEXT_ID;
            txtFormShield.MaxLength = 5;
            txtFormShield.ClientSideEvents.KeyUp = "Imis.Lib.ToEnUpper";

            txtFormShield.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
            txtFormShield.ValidationSettings.ErrorText = BotShield.DEFAULT_ERROR_MESSAGE;
            txtFormShield.ValidationSettings.RequiredField.IsRequired = true;
            txtFormShield.ValidationSettings.RequiredField.ErrorText = BotShield.REQUIRED_FIELD_ERROR_MESSAGE;

            txtFormShield.Validation += txtFormShield_Validation;

            tc.Controls.Add(txtFormShield);
        }

        public void txtFormShield_Validation(object sender, DevExpress.Web.ValidationEventArgs e)
        {
            if (!isValid.HasValue)
            {
                isValid = captcha.Validate(txtFormShield.Text);
            }

            e.IsValid = isValid.Value;
        }

        public void InstantiateIn(Control container)
        {
            container.Controls.Add(lt);
            container.Controls.Add(tbl);
        }
    }
}
