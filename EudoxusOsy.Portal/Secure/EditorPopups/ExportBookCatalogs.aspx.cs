using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Linq;

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

            var book = new BookRepository(UnitOfWork).FindByBookKpsID(bookID, x => x.BookSuppliers).FirstOrDefault();
            var currentSupplier = new SupplierRepository(UnitOfWork).FindByUsername(User.Identity.Name);


            if (book != null
                && (currentSupplier != null && book.BookSuppliers.Select(x => x.SupplierID).Contains(currentSupplier.ID)
                || EudoxusOsyRoleProvider.IsAuthorizedEditorUser()))
            {
                var bookGroupsToExport = book.GetCatalogsOfBook(phaseID, UnitOfWork);
                gvExportBookCatalogs.Export(bookGroupsToExport, "bookGroups_" + bookID);
            }

        }
    }
}