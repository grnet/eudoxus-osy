using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;
using System.Threading;
using EudoxusOsy.Utils;
using Imis.Domain;
using EudoxusOsy.Portal.Controls;

namespace EudoxusOsy.Portal.Admin
{
    public partial class RefreshCache : BaseEntityPortalPage
    {
        protected void btnRefreshCache_Click(object sender, EventArgs e)
        {
            CacheManager.Refresh();

            Notify("Η Cache ανανεώθηκε επιτυχώς");
        }
    }
}