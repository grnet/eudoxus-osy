using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EudoxusOsy.Portal.Secure.EditorPopups
{
    public partial class InvoiceCollectionEditPopup : BaseSecureEntityPortalPage<CatalogGroup>
    {
        protected CatalogGroupInfo CatalogGroupInfo { get; private set; }

        protected int? CatalogGroupID
        {
            get
            {
                if (Request.QueryString["id"] != null)
                {
                    return Convert.ToInt32(Request.QueryString["id"]);
                }
                return null;
            }
        }

        protected override void Fill()
        {
            if (CatalogGroupID.HasValue)
            {
                Entity = new CatalogGroupRepository(UnitOfWork).Load(CatalogGroupID.Value, x => x.Supplier);
                Entity.Invoices.EnsureLoad();

                CatalogGroupInfo = new CatalogGroupRepository(UnitOfWork).GetByID(CatalogGroupID.Value);
            }
            else
            {
                Response.Redirect("~/Default.aspx");
            }            
        }


        protected override bool Authorize()
        {
            return (EudoxusOsyRoleProvider.IsAuthorizedEditorUser()
                || Entity.Supplier.ReporterID == Entity.Supplier.ReporterID)
                && CatalogGroupHelper.CanEditGroup(CatalogGroupInfo);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsAuthorized)
            {
                ClientScript.RegisterStartupScript(GetType(), "closePopup", "window.parent.popUp.hide();", true);
                return;
            }

            Entity.Invoices.EnsureLoad();

            var banks = EudoxusOsyCacheManager<Bank>.Current.GetItems();
            cmbSelectBank.DataSource = banks.Where(x=> x.IsBank).OrderBy(x=> x.Name);
            cmbSelectBank.DataBind();

            InvoiceItemsEdit.CatalogGroupInfo = CatalogGroupInfo;
            InvoiceItemsEdit.Bind(Entity.GetInvoicesToCollection());
            if (Entity.BankID.HasValue)
            {
                chkTransferToBank.Checked = Entity.IsTransfered;
                cmbSelectBank.SelectedItem = cmbSelectBank.Items.FindByValue(Entity.BankID);
            }
            else
            {
                cmbSelectBank.ClientVisible = false;
            }

            if(Entity.State >  enCatalogGroupState.New && User.IsInRole(RoleNames.Supplier))
            {
                InvoiceItemsEdit.ReadOnly = true;
                chkTransferToBank.ClientEnabled = false;
                cmbSelectBank.ClientEnabled = false;
            }
        }

        protected void btnSubmitHidden_Click(object sender, EventArgs e)
        {
            if (IsAuthorized)
            {
                var invoicesCollection = InvoiceItemsEdit.ExtractValue();
                var newInvoiceIDs = invoicesCollection.Select(x => x.InvoiceID);

                var existingInvoices = Entity.Invoices.ToList();

                for (int i = existingInvoices.Count - 1; i >= 0; i--)
                {
                    if (!newInvoiceIDs.Contains(existingInvoices[i].ID))
                    {
                        UnitOfWork.MarkAsDeleted(existingInvoices[i]);
                    }
                }


                foreach (var item in invoicesCollection)
                {
                    if (!item.InvoiceID.HasValue)
                    {
                        var newInvoice = new Invoice()
                        {
                            GroupID = Entity.ID,
                            InvoiceNumber = item.InvoiceNumber,
                            InvoiceDate = item.Date,
                            IsActive = true,
                            InvoiceValue = item.Amount
                        };
                        UnitOfWork.MarkAsNew(newInvoice);
                    }
                }

                Entity.IsTransfered = hfIsTransferedToBank.Value == "true" ? true : false;
                Entity.BankID = Entity.IsTransfered ? cmbSelectBank.GetSelectedInteger() : null;

                UnitOfWork.Commit();
                ClientScript.RegisterStartupScript(GetType(), "closePopup", "window.parent.refresh();window.parent.popUp.hide();", true);
            }
        }
    }
}