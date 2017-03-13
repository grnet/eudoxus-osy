using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EudoxusOsy.Portal.UserControls.Popups
{
    public partial class HintPopup : System.Web.UI.UserControl
    {
        #region [ Page Inits ]

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (HttpContext.Current.Items["__hintPopupRendered"] == null)
            {
                dxHintPopup.Enabled = true;
                HttpContext.Current.Items["__hintPopupRendered"] = true;
            }
            else
            {
                dxHintPopup.Enabled = false;
            }
        }

        #endregion
    }
}