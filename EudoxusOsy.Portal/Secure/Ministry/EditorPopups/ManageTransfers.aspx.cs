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

namespace EudoxusOsy.Portal.Secure.Ministry.EditorPopups
{
    public partial class ManageTransfers : BaseEntityPortalPage<Supplier>
    {
        #region [ Entity Fill ]

        protected int CurrentSupplierID;

        protected override void Fill()
        {
            if (int.TryParse(Request.QueryString["id"], out CurrentSupplierID) && CurrentSupplierID > 0)
            {
                Entity = new SupplierRepository(UnitOfWork).Load(CurrentSupplierID);
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
            }
        }

        #endregion

        #region [ Page Inits ]

        protected void Page_Load(object sender, EventArgs e)
        {
            btnAddTransfer.ClientSideEvents.Click = string.Format("function (s,e) {{ showAddTransferPopup({0}); }}", CurrentSupplierID);
        }

        #endregion

        #region [ DataSource Events ]

        protected void odsTransfers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<BankTransfer> criteria = new Criteria<BankTransfer>();

            criteria.Include(x => x.Bank)
                    .Include(x => x.Phase)
                    .Include(x => x.Supplier);

            criteria.Sort.OrderByDescending(x => x.ID);

            criteria.Expression = criteria.Expression.Where(x => x.SupplierID, CurrentSupplierID)
                                                     .Where(x => x.IsActive, true);

            e.InputParameters["criteria"] = criteria;
        }

        #endregion

        #region [ GridView Events ]

        protected void gvTransfers_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();

            if (action == "refresh")
            {
                gvTransfers.DataBind();
                return;
            }

            if (action == "delete" && EudoxusOsyRoleProvider.IsAuthorizedEditorUser())
            {
                var id = int.Parse(parameters[1]);

                var bankTransfer = new BankTransferRepository(UnitOfWork).Load(id);

                UnitOfWork.MarkAsDeleted(bankTransfer);
                UnitOfWork.Commit();

                gvTransfers.DataBind();
            }
        }

        #endregion
    }
}
