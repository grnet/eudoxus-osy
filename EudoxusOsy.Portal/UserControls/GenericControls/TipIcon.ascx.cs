using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace EudoxusOsy.Portal.UserControls.GenericControls
{
    [DefaultProperty("Text")]
    [ParseChildren(true, "Text")]
    public partial class TipIcon : System.Web.UI.UserControl, ITextControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        public string Text
        {
            get { return imgIcon.ToolTip; }
            set { imgIcon.ToolTip = value; }
        }


    }
}