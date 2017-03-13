using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using DevExpress.Web;
using System.Text.RegularExpressions;
using EudoxusOsy.Portal.Utils;
using Imis.Domain;

namespace EudoxusOsy.Portal.UserControls.GenericControls
{
    public partial class HyperLinkInput : UserControl
    {
        #region [ Page Inits ]

        protected void Page_Init(object sender, EventArgs e)
        {
            txtHyperLink.ValidationSettings.RegularExpression.ValidationExpression = @"^(http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?$";

            if (IsRequired)
            {
                txtHyperLink.ValidationSettings.RequiredField.IsRequired = true;
                txtHyperLink.ValidationSettings.RequiredField.ErrorText = string.Format("Το πεδίο '{0}' είναι υποχρεωτικό", Label);
            }
        }

        #endregion

        #region [ Extract - Bind ]

        public string GetHyperLink()
        {
            Regex validHyperLinkRegex = new Regex(@"^(http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?$", RegexOptions.IgnoreCase);
            var hyperLink = txtHyperLink.GetText();

            return !string.IsNullOrEmpty(hyperLink) && validHyperLinkRegex.IsMatch(hyperLink) ? hyperLink : null;
        }

        public void Bind(string hyperLink)
        {
            if (ReadOnly)
            {
                lnkHyperLink.Attributes["href"] = hyperLink;
                lnkHyperLink.InnerHtml = hyperLink;

                mvModeType.SetActiveView(vViewMode);
            }
            else
            {
                txtHyperLink.Text = hyperLink;

                mvModeType.SetActiveView(vEditMode);
            }
        }

        #endregion

        #region [ Validation ]

        public string ValidationGroup
        {
            get { return txtHyperLink.ValidationSettings.ValidationGroup; }
            set
            {
                txtHyperLink.ValidationSettings.ValidationGroup = value;
            }
        }

        #endregion

        #region [ Properties ]

        public Unit Width
        {
            set
            {
                txtHyperLink.Width = value;
            }
        }

        public string Tooltip
        {
            set
            {
                txtHyperLink.ToolTip = value;
            }
        }

        public bool HideTooltip
        {
            set
            {
                if (value)
                {
                    txtHyperLink.CssClass = string.Empty;
                }
            }
        }

        public string Label { get; set; }
        public bool IsRequired { get; set; }
        public bool ReadOnly
        {
            get { return txtHyperLink.ReadOnly; }
            set
            {
                txtHyperLink.ReadOnly = value;
                txtHyperLink.ClientEnabled = !value;
            }
        }

        #endregion
    }
}