using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;

namespace EudoxusOsy.Portal.Secure.Suppliers
{
    public partial class Suppliers : System.Web.UI.MasterPage
    {
        protected Supplier CurrentSupplier { get; set; }
        public bool HideSiteMap { get; set; }

        protected override void OnPreRender(EventArgs e)
        {
            if (HideSiteMap)
            {
                alertsArea.Visible = false;
                registeredUsersMenu.Visible = false;
            }

            base.OnPreRender(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var urls = new
            {
                ChangePasswordUrl = ResolveClientUrl("~/Common/AlterPassword.aspx"),
                ContactFormUrl = ResolveClientUrl("~/Secure/EditorPopups/ContactForm.aspx"),
                ContactHistoryUrl = ResolveClientUrl("~/Secure/EditorPopups/ContactHistory.aspx"),
                ViewCatalogGroupDetailsUrl = ResolveClientUrl("~/Secure/EditorPopups/ViewCatalogGroupDetails.aspx"),
                ManageInvoicesUrl = ResolveClientUrl("~/Secure/EditorPopups/ManageInvoices.aspx"),
                SetBankTransferUrl = ResolveClientUrl("~/Secure/EditorPopups/SetBankTransfer.aspx"),
                ViewBankTransferUrl = ResolveClientUrl("~/Secure/EditorPopups/ViewBankTransfer.aspx")
            };

            Page.ClientScript.RegisterStartupScript(
                GetType(),
                "supplierUrls",
                string.Format("var supplierUrls = {0};", new JavaScriptSerializer().Serialize(urls)),
                true);

            using (var uow = UnitOfWorkFactory.Create())
            {
                CurrentSupplier = Context.LoadSupplier() ??
                                   new SupplierRepository(uow).FindByUsername(Page.User.Identity.Name);
            }

            if (CurrentSupplier != null)
            {
                lblName.Text = CurrentSupplier.Name;
            }

            loginBar.ChangePasswordButton.Visible = false;
            loginBar.UserDetails.Visible = false;
        }

        protected bool IsCurrentOrParentNode(object node)
        {
            var smNode = node as SiteMapNode;
            if (smNode != null && smNode.Provider.CurrentNode != null)
            {
                return smNode == smNode.Provider.CurrentNode || smNode == smNode.Provider.CurrentNode.ParentNode;
            }

            return false;
        }

        protected void repMenu_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SiteMapNode thisMapNode = (SiteMapNode)e.Item.DataItem;
            }
        }
    }
}