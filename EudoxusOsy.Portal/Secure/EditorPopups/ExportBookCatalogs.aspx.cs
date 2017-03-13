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
    public partial class ExportBookCatalogs : BaseEntityPortalPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmitHidden_Click(object sender, EventArgs e)
        {
            var bookID = int.Parse(txtKpsID.Text);
            var phaseID = int.Parse(txtPhaseID.Text);

            var book = new BookRepository(UnitOfWork).FindByBookKpsID(bookID).FirstOrDefault();
            if (book != null)
            {
                var bookGroupsToExport = book.GetCatalogsOfBook(phaseID, UnitOfWork);
                gvExportBookCatalogs.Export(bookGroupsToExport, "bookGroups_" + bookID);
            }

        }
    }
}