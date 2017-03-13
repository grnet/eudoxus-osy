using DevExpress.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Linq;

namespace EudoxusOsy.Portal.Secure.Ministry.EditorPopups
{
    public partial class AddCatalog : BaseEntityPortalPage<NewCatalogInfo>
    {
        #region [ Entity Fill ]

        protected override void Fill()
        {
            Entity = new NewCatalogInfo();
        }

        #endregion

        #region [ Button Handlers ]

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ASPxEdit.ValidateEditorsInContainer(Page, "vgCatalog"))
                return;

            ucCatalogInput.Fill(Entity);

            var book = new BookRepository(UnitOfWork).FindByBookKpsID(Entity.BookKpsID).FirstOrDefault();

            if (book == null)
            {
                lblErrors.Text = "Δεν βρέθηκε βιβλίο με τον κωδικό που εισάγατε";
                return;
            }

            var department = EudoxusOsyCacheManager<Department>.Current.GetItems().FirstOrDefault(x => x.SecretaryKpsID == Entity.SecretaryKpsID);

            if (department == null)
            {
                lblErrors.Text = "Δεν βρέθηκε Γραμματεία με τον κωδικό που εισάγατε";
                return;
            }

            var phase = new PhaseRepository(UnitOfWork).Load(Entity.PhaseID);
            var bookPrice = new BookPriceRepository(UnitOfWork).FindByBookIDAndYear(book.ID, phase.Year);

            if (bookPrice == null)
            {
                lblErrors.Text = string.Format("Το βιβλίο δεν έχει κοστολογηθεί για την περίοδο {0}", phase.AcademicYearString);
                return;
            }

            var supplier = new SupplierRepository(UnitOfWork).FindByKpsID(Entity.SupplierKpsID);
            var currentYear = phase.Year;

            if (supplier == null)
            {
                lblErrors.Text = "Δεν βρέθηκε εκδότης με τον κωδικό που εισάγατε";
                return;
            }

            var bookSupplier = new BookSupplierRepository(UnitOfWork).FindBySupplierIDAndBookIDAndYear(supplier.ID, book.ID, currentYear);
            var discount = new DiscountRepository(UnitOfWork).FindGeneralForPhase(phase.ID);

            if (bookSupplier == null || bookSupplier.Count == 0)
            {
                lblErrors.Text = "Δεν υπάρχει σύνδεση μεταξύ του επιλεγμένου βιβλίου και του επιλεγμένου εκδότη";
                return;
            }

            var newCatalog = new Catalog()
            {
                CatalogType = enCatalogType.Manual,
                PhaseID = Entity.PhaseID,
                OriginalPhaseID = Entity.PhaseID,
                BookID = book.ID,
                SupplierID = supplier.ID,
                DepartmentID = department.ID,
                BookCount = Entity.BookCount,
                Percentage = bookSupplier[0].Percentage,
                BookPriceID = bookPrice.ID,
                Amount = bookPrice.Price * Entity.BookCount * book.GetBookDiscount(currentYear),
                Status = enCatalogStatus.Active,
                CreatedBy = User.Identity.Name,
                CreatedAt = DateTime.Today,
                IsBookActive = book.IsActive
            };

            if (discount != null)
            {
                newCatalog.DiscountID = discount.ID;
            }

            UnitOfWork.MarkAsNew(newCatalog);
            UnitOfWork.Commit();

            var catalogLog = newCatalog.CreateCatalogLog(enCatalogLogAction.Create, User.Identity.Name, User.Identity.ReporterID);
            UnitOfWork.MarkAsNew(catalogLog);

            UnitOfWork.Commit();

            var message = string.Format("Δημιουργήθηκε νεά διανομή με ID {0} και η ενέργεια καταγράφηκε με κωδικό {1}. Η τιμή του βιβλίου για το έτος {2} έχει οριστεί στα € {3}.", newCatalog.ID, catalogLog.ID, currentYear, bookPrice.Price);

            ClientScript.RegisterStartupScript(GetType(), "closePopup", "window.parent.showAlertBox('" + message + "');window.parent.popUp.hide();", true);
        }

        #endregion
    }
}