using DevExpress.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using Imis.Domain;
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

            BookPrice bookPrice;
            int currentYear;
            Catalog newCatalog;
            CatalogLog catalogLog;
            if (AddNewCatalog(Entity, UnitOfWork, null, out bookPrice, out currentYear, out newCatalog, out catalogLog)) return;

            var message = string.Format("Δημιουργήθηκε νεά διανομή με ID {0} και η ενέργεια καταγράφηκε με κωδικό {1}. Η τιμή του βιβλίου για το έτος {2} έχει οριστεί στα € {3}.", newCatalog.ID, catalogLog.ID, currentYear, bookPrice.Price);

            ClientScript.RegisterStartupScript(GetType(), "closePopup", "window.parent.showAlertBox('" + message + "');window.parent.popUp.hide();", true);
        }

        public bool AddNewCatalog(NewCatalogInfo entity, IUnitOfWork uow, IRepositoryFactory repFactory, out BookPrice bookPrice, out int currentYear, out Catalog newCatalog,
            out CatalogLog catalogLog)
        {

            bookPrice = null;
            currentYear = 0;
            newCatalog = null;
            catalogLog = null;

            #region Setup Repositories
            IBookRepository bookRepository;
            IPhaseRepository phaseRepository;
            IBookPriceRepository bookPriceRepository;
            ISupplierRepository supplierRepository;
            IBookSupplierRepository bookSupplierRepository;
            IDiscountRepository discountRepository;

            if (repFactory == null)
            {
                bookRepository = new BookRepository(uow);
                phaseRepository = new PhaseRepository(uow);
                bookPriceRepository = new BookPriceRepository(uow);
                supplierRepository = new SupplierRepository(uow);
                bookSupplierRepository = new BookSupplierRepository(uow);
                discountRepository = new DiscountRepository(uow);

            }
            else
            {
                bookRepository = repFactory.GetRepositoryInstance<Book, IBookRepository>(uow);
                phaseRepository = repFactory.GetRepositoryInstance<Phase, IPhaseRepository>(uow);
                bookPriceRepository = repFactory.GetRepositoryInstance<BookPrice, IBookPriceRepository>(uow);
                supplierRepository = repFactory.GetRepositoryInstance<Supplier, ISupplierRepository>(uow);
                bookSupplierRepository = repFactory.GetRepositoryInstance<BookSupplier, IBookSupplierRepository>(uow);
                discountRepository = repFactory.GetRepositoryInstance<Discount, IDiscountRepository>(uow);

            }
            #endregion

            var book = bookRepository.FindByBookKpsID(entity.BookKpsID).FirstOrDefault();

            if (book == null)
            {
                lblErrors.Text = "Δεν βρέθηκε βιβλίο με τον κωδικό που εισάγατε";
                return true;
            }

            var department = EudoxusOsyCacheManager<Department>.Current.GetItems()
                .FirstOrDefault(
                x => x.SecretaryKpsID == entity.SecretaryKpsID
                || x.LibraryKpsID == entity.SecretaryKpsID);

            if (department == null)
            {
                lblErrors.Text = "Δεν βρέθηκε Γραμματεία με τον κωδικό που εισάγατε";
                return true;
            }

            var phase = phaseRepository.Load(entity.PhaseID);
            bookPrice = bookPriceRepository.FindByBookIDAndYear(book.ID, phase.Year);

            if (bookPrice == null)
            {
                lblErrors.Text = string.Format("Το βιβλίο δεν έχει κοστολογηθεί για την περίοδο {0}", phase.AcademicYearString);
                return true;
            }

            var supplier = supplierRepository.FindByKpsID(entity.SupplierKpsID);
            currentYear = phase.Year;

            if (supplier == null)
            {
                lblErrors.Text = "Δεν βρέθηκε εκδότης με τον κωδικό που εισάγατε";
                return true;
            }

            var bookSupplier = bookSupplierRepository.FindBySupplierIDAndBookIDAndYear(supplier.ID, book.ID, currentYear);
            var discount = discountRepository.FindGeneralForPhase(phase.ID);

            if (bookSupplier == null || bookSupplier.Count == 0)
            {
                lblErrors.Text = "Δεν υπάρχει σύνδεση μεταξύ του επιλεγμένου βιβλίου και του επιλεγμένου εκδότη";
                return true;
            }

            newCatalog = new Catalog()
            {
                CatalogType = enCatalogType.Manual,
                PhaseID = entity.PhaseID,
                OriginalPhaseID = entity.PhaseID,
                BookID = book.ID,
                SupplierID = supplier.ID,
                DepartmentID = department.ID,
                BookCount = entity.BookCount,
                Percentage = bookSupplier[0].Percentage,
                BookPriceID = bookPrice.ID,
                Amount = bookPrice.Price * entity.BookCount * book.GetBookDiscount(currentYear),
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

            catalogLog = newCatalog.CreateCatalogLog(enCatalogLogAction.Create, User.Identity.Name, User.Identity.ReporterID);
            UnitOfWork.MarkAsNew(catalogLog);

            UnitOfWork.Commit();
            return false;
        }

        #endregion
    }
}