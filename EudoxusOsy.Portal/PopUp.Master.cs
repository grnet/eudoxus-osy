using Imis.Web.Controls;
using EudoxusOsy.Portal.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EudoxusOsy.Portal
{
    public partial class PopUp : BaseMasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var urls = new
            {
                AddInvoiceUrl = ResolveClientUrl("~/Secure/EditorPopups/AddInvoice.aspx"),
                AddFileUrl = ResolveClientUrl("~/Secure/EditorPopups/AddFile.aspx"),
                EditInvoiceUrl = ResolveClientUrl("~/Secure/EditorPopups/EditInvoice.aspx"),
                AddTransferUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/AddTransfer.aspx"),
                EditTransferUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/EditTransfer.aspx")
            };

            Page.ClientScript.RegisterStartupScript(
                GetType(),
                "popupUrls",
                string.Format("var popupUrls = {0};", new JavaScriptSerializer().Serialize(urls)),
                true);
        }
    }
}