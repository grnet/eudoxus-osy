using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;

namespace EudoxusOsy.Portal.Secure.Suppliers
{
    public partial class Default : BaseEntityPortalPage<Supplier>
    {
        #region [ Entity Fill ]

        protected override void Fill()
        {
            Entity = new SupplierRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name,
                                                                        x => x.Reporter,
                                                                        x => x.SupplierDetail);
            Entity.SaveToCurrentContext();
        }

        #endregion

        #region [ Page Inits ]

        protected void Page_Init(object sender, EventArgs e)
        {
            ucSupplierView.Entity = Entity;
            ucSupplierView.Bind();
        }

        #endregion

    }
}