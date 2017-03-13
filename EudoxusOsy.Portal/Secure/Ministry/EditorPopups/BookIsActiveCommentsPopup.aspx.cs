using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.Portal.Controls;
using EudoxusOsy.BusinessModel;
using System.Web.Security;
using DevExpress.Web;
using EudoxusOsy.Portal.Utils;

namespace EudoxusOsy.Portal.Secure.Ministry.EditorPopups
{
    public partial class BookIsActiveCommentsPopup : BaseEntityPortalPage<Book>
    {
        #region [ Entity Fill ]

        protected override void Fill()
        {
            int bookID;
            if (int.TryParse(Request.QueryString["id"], out bookID) && bookID > 0)
            {
                Entity = new BookRepository(UnitOfWork).Load(bookID);
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "hidePopup", "window.parent.popUp.hide();", true);
            }
        }

        #endregion

        #region [ Button Handlers ]

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Entity.Comments = txtComments.Text;
            Entity.IsActive = !Entity.IsActive;

            var catalogsToDeActivate = new CatalogRepository(UnitOfWork).FindCatalogsToDeActivate(Entity.ID);

            foreach (var catalog in catalogsToDeActivate)
            {
                catalog.IsBookActive = Entity.IsActive;
            }

            UnitOfWork.Commit();
            ClientScript.RegisterStartupScript(GetType(), "closePopup", "window.parent.cmdRefresh();window.parent.popUp.hide();", true);
        }

        #endregion
    }
}
