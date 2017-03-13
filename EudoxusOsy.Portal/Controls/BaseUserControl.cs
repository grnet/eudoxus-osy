using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using EudoxusOsy.BusinessModel;

namespace EudoxusOsy.Portal.Controls
{
    public class BaseUserControl : UserControl
    {
        public EudoxusOsyPrincipal User
        {
            get { return base.Page.User as EudoxusOsyPrincipal; }
        }
    }

    public class BaseUserControl<TPage> : BaseUserControl where TPage : Page
    {
        public new TPage Page { get { return (TPage)base.Page; } }
    }
}
