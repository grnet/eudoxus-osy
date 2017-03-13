using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;

namespace EudoxusOsy.Portal.Secure.Ministry
{
    public partial class Ministry : System.Web.UI.MasterPage
    {
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
                VatDataEditPopupUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/EditVatDataPopup.aspx"),
                ViewSupplierUrl = ResolveClientUrl("~/Secure/EditorPopups/ViewSupplier.aspx"),
                ViewIbanChangesUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/ViewIbanChanges.aspx"),
                ChangeIbanUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/ChangeIban.aspx"),
                ViewCatalogGroupDetailsUrl = ResolveClientUrl("~/Secure/EditorPopups/ViewCatalogGroupDetails.aspx"),
                ManageInvoicesUrl = ResolveClientUrl("~/Secure/EditorPopups/ManageInvoices.aspx"),
                ManageTransfersUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/ManageTransfers.aspx"),
                EditGroupDeductionUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/EditGroupDeduction.aspx"),
                SetBankTransferUrl = ResolveClientUrl("~/Secure/EditorPopups/SetBankTransfer.aspx"),
                ViewBankTransferUrl = ResolveClientUrl("~/Secure/EditorPopups/ViewBankTransfer.aspx"),
                ViewCatalogGroupLogUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/ViewCatalogGroupLog.aspx"),
                BankTransferPopupUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/IncomingTransfersPopup.aspx"),
                BankEditPopupUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/BankEditPopup.aspx"),
                ExportBookCatalogsPopupUrl = ResolveClientUrl("~/Secure/EditorPopups/ExportBookCatalogs.aspx"),
                ShowEditBookActiveStatusUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/EditBookActiveStatus.aspx"),
                InsertNewCatalogPopupUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/InsertNewCatalogPopup.aspx"),
                EditCatalogPopupUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/EditCatalogPopup.aspx"),
                SendToYDEUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/SendToYDE.aspx"),
                ReturnFromYDEUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/ReturnFromYDE.aspx"),
                ChangePhasePopupUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/ChangePhasePopup.aspx"),
                ApproveGroupUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/ApproveGroup.aspx"),
                RevertApprovalUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/RevertApproval.aspx"),
                ExportOfficeSlipPopupUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/ExportOfficeSlipPopup.aspx"),
                ExportCatalogInvoicePopupUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/ExportCatalogInvoicePopup.aspx"),
                AddCatalogUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/AddCatalog.aspx"),
                AddBankUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/AddBank.aspx"),
                EditBankUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/EditBank.aspx"), 
                AddMinistryUserUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/AddMinistryUser.aspx"),
                EditMinistryUserUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/EditMinistryUser.aspx"),
                SelectPhasePopupUrl = ResolveClientUrl("~/Secure/Ministry/EditorPopups/SelectPhasePopup.aspx"),
                ManageFilesUrl = ResolveClientUrl("~/Secure/EditorPopups/ManageFiles.aspx"),

            };

            Page.ClientScript.RegisterStartupScript(
                GetType(),
                "ministryUrls",
                string.Format("var ministryUrls = {0};", new JavaScriptSerializer().Serialize(urls)),
                true);

            loginBar.ChangePasswordButton.Visible = false;
            loginBar.UserDetails.Visible = false;

            //var phase = CacheManager.Phases.Get(SelectedPhaseID);

            //selectedPhaseArea.Visible = true;
            //ltSelectedPhase.Text = string.Format("Τρέχουσα Περίοδος Πληρωμών: <b>{0:dd/MM/yyyy}</b> με <b>{1}</b>.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a runat='server' href='SelectPhase.aspx?pID={2}'>Επιλογή Περιόδου Πληρωμών</a>", phase.StartDate, phase.EndDate.HasValue ? string.Format("{0:dd/MM/yyyy}", phase.EndDate) : "σήμερα", ((Ministry)Page.Master).SelectedPhaseID);
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

        protected string GetUrl(object url)
        {
            return url.ToString();
            //if (url == null)
            //    return string.Empty;

            //return string.Format("{0}?pID={1}", url, SelectedPhaseID);
        }
    }
}