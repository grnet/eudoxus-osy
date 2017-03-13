using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EudoxusOsy.Portal.Secure.Ministry.EditorPopups
{
    public partial class InsertNewCatalogPopup : BaseSecureEntityPortalPage
    {
        protected override bool Authenticate()
        {
            return User.IsInRole(RoleNames.MinistryPayments)
                    || User.IsInRole(RoleNames.SystemAdministrator);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsSecure)
            {

            }
       }

        protected void btnSubmitHidden_Click(object sender, EventArgs e)
        {
            Catalog newCatalog = new Catalog();

            int bookKpsID = Convert.ToInt32(txtBookID.Text);
            var book = new BookRepository(UnitOfWork).FindByBookKpsID(bookKpsID).FirstOrDefault();

            if (book == null)
            {
                ClientScript.RegisterStartupScript(GetType(), "alertError", "window.parent.showAlertBox('Δεν βρέθηκε βιβλίο με το συγκεκριμένο κωδικό')", true);
            }
            else
            {

                newCatalog.BookID = book.ID;

                var department = EudoxusOsyCacheManager<Department>.Current.GetItems().FirstOrDefault(x => x.SecretaryKpsID == Convert.ToInt32(txtSecretary.Text));
                if (department != null)
                {
                    newCatalog.DepartmentID = department.ID;


                    int supplierKpsID = Convert.ToInt32(txtSupplier.Text);
                    var supplier = new SupplierRepository(UnitOfWork).FindByKpsID(supplierKpsID);

                    int phaseID = Convert.ToInt32(txtPhase.Text);
                    var phase = new PhaseRepository(UnitOfWork).Load(phaseID);
                    BookPrice bookPrice = new BookPriceRepository(UnitOfWork).FindByBookIDAndYear(book.ID, phase.Year);

                    if (bookPrice == null)
                    {
                        ClientScript.RegisterStartupScript(GetType(), "alertError", "window.parent.showAlertBox('Το βιβλίο δεν έχει κοστολογηθεί για την περίοδο'" + phaseID + ". )", true);
                    }
                    else
                    {

                        var currentYear = phase.Year;
                        var bookSupplier = new BookSupplierRepository(UnitOfWork).FindBySupplierIDAndBookIDAndYear(supplier.ID, book.ID, currentYear);
                        var discount = new DiscountRepository(UnitOfWork).FindGeneralForPhase(phase.ID);
                        if (bookSupplier != null && bookSupplier.Count > 0)
                        {
                            newCatalog.PhaseID = phaseID;
                            newCatalog.OriginalPhaseID = phaseID;
                            newCatalog.SupplierID = supplier.ID;
                            newCatalog.BookCount = Convert.ToInt32(txtBookCount.Text);
                            newCatalog.CreatedBy = User.Identity.Name;
                            newCatalog.CreatedAt = DateTime.Today;
                            newCatalog.CatalogType = enCatalogType.Manual;
                            newCatalog.Status = enCatalogStatus.Active;
                            newCatalog.BookPriceID = bookPrice.ID;
                            newCatalog.Amount = bookPrice.Price * newCatalog.BookCount * book.GetBookDiscount(currentYear);
                            if (discount != null)
                            {
                                newCatalog.DiscountID = discount.ID;
                            }
                            newCatalog.Percentage = bookSupplier[0].Percentage;

                            UnitOfWork.MarkAsNew(newCatalog);
                            UnitOfWork.Commit();

                            /**
                                Prepare the log entry
                            */
                            var catalogLog = newCatalog.CreateCatalogLog(enCatalogLogAction.Create,User.Identity.Name, User.Identity.ReporterID);
                            UnitOfWork.MarkAsNew(catalogLog);

                            UnitOfWork.Commit();
                            ClientScript.RegisterStartupScript(GetType(), "closePopup", "window.parent.popUp.hide();", true);
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(GetType(), "alertError", "window.parent.showAlertBox('Δεν υπάρχει σύνδεση μεταξύ του επιλεγμένου βιβλίου και του επιλεγμένου εκδότη')", true);
                        }
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "alertError", "window.parent.showAlertBox('Δεν βρέθηκε Γραμματεία με τον κωδικό που δώσατε')", true);
                }
            }
        }
    }
}