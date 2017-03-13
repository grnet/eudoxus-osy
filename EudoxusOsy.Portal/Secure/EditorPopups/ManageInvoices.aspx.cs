using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using Imis.Domain;
using System.Drawing;
using DevExpress.Web;
using EudoxusOsy.Utils;
using System.Web.Security;

namespace EudoxusOsy.Portal.Secure.EditorPopups
{
    public partial class ManageInvoices : BaseEntityPortalPage<CatalogGroup>
    {
        #region [ Entity Fill ]

        protected int CurrentGroupID;

        protected override void Fill()
        {
            if (int.TryParse(Request.QueryString["id"], out CurrentGroupID) && CurrentGroupID > 0)
            {
                Entity = new CatalogGroupRepository(UnitOfWork).Load(CurrentGroupID,
                                                                        x => x.Catalogs,
                                                                        x => x.Invoices);

                if (Entity == null)
                {
                    ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
                }
            }
        }

        #endregion

        #region [ Page Inits ]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Entity.IsLocked
                || Entity.State == enCatalogGroupState.Sent
                || (Roles.IsUserInRole(RoleNames.Supplier) && Entity.StateInt > (int)enCatalogGroupState.New))
            {
                btnAddInvoice.Visible = false;
                gvInvoices.Columns.Where(x => x.Name == "Actions").First().Visible = false;
            }
            else
            {
                BindButtons();
                btnAddInvoice.ClientSideEvents.Click = string.Format("function (s,e) {{ showAddInvoicePopup({0}); }}", CurrentGroupID);
            }
        }

        private void BindButtons()
        {
            if (Entity.Invoices.Count > 0)
            {
                var invoicesSum = Entity.Invoices.Where(x => x.IsActive).Sum(x => x.InvoiceValue);

                divWarning.Visible = invoicesSum < Entity.TotalAmount;
                divError.Visible = invoicesSum > Entity.TotalAmount;
            }
            else
            {
                divWarning.Visible = false;
                divError.Visible = false;
            }
        }

        #endregion

        #region [ Button Handlers ]

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            BindButtons();
            gvInvoices.DataBind();
        }

        protected void btnDeleteInvoice_Click(object sender, EventArgs e)
        {
            var invoiceID = int.Parse(hfInvoiceID.Value);
            var invoice = new InvoiceRepository(UnitOfWork).Load(invoiceID);

            UnitOfWork.MarkAsDeleted(invoice);
            UnitOfWork.Commit();

            BindButtons();
            gvInvoices.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }

        #endregion

        #region [ DataSource Events ]

        protected void odsInvoices_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<Invoice> criteria = new Criteria<Invoice>();

            criteria.Expression =
                criteria.Expression.Where(x => x.GroupID, Entity.ID)
                                    .Where(x => x.IsActive, true);

            e.InputParameters["criteria"] = criteria;
        }

        #endregion

        #region [ GridView Events ]

        protected void gvInvoices_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();

            if (action == "refresh")
            {
                gvInvoices.DataBind();
                return;
            }
        }

        #endregion
    }
}
