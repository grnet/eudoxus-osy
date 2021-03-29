using Imis.Domain;
using System;
using System.Linq;
using System.Web;

namespace EudoxusOsy.BusinessModel
{
    public static class CatalogGroupHelper
    {
        public static void GroupCatalogsByInstitution(int supplierID, int? supplierIBANID, int phaseID, IUnitOfWork uow)
        {
            var ungroupedCatalogs = new CatalogRepository(uow).GetUngroupedCatalogsBySupplierAndPhase(supplierID, phaseID);
            var groupedCatalogs = ungroupedCatalogs.GroupBy(x => x.Department.InstitutionID);

            var deduction = FindActiveDeductionForSupplier(supplierID);

            foreach (IGrouping<int, Catalog> item in groupedCatalogs)
            {
                var group = new CatalogGroup()
                {
                    PhaseID = phaseID,
                    SupplierID = supplierID,
                    SupplierIBANID = supplierIBANID,
                    InstitutionID = item.Key,
                    State = enCatalogGroupState.New,
                    IsActive = true
                };

                if (deduction != null)
                {
                    group.DeductionID = deduction.ID;
                }                

                foreach (Catalog catalog in item)
                {
                    catalog.CatalogGroup = group;
                }

                uow.Commit();
            }
        }

        public static void UngroupCatalogs(int supplierID, int phaseID, IUnitOfWork uow)
        {
            var groupedCatalogs = new CatalogRepository(uow).GetCatalogsToUngroupBySupplierAndPhase(supplierID, phaseID).ToList();
            var groups = groupedCatalogs.Select(x => x.CatalogGroup).Distinct().ToList();
            groupedCatalogs.ForEach(x => { x.GroupID = null; });
            groups.ForEach(x =>
            {
                x.Invoices.ToList().ForEach(y => { uow.MarkAsDeleted(y); });
                x.CatalogGroupLogs.ToList().ForEach(y => uow.MarkAsDeleted(y));
                uow.MarkAsDeleted(x);
            });

            uow.Commit();
        }

        public static Deduction FindDeductionFromDeductionVatType(enDeductionVatType vatType)
        {
            return EudoxusOsyCacheManager<Deduction>.Current.GetItems()
                .Where(x => x.IsActive && x.VatTypeInt == (int)vatType)
                .OrderByDescending(x => x.ID)
                .FirstOrDefault();
        }

        public static Deduction FindActiveDeductionFromOldDeduction(Deduction oldDeduction)
        {
            return EudoxusOsyCacheManager<Deduction>.Current.GetItems()
                .Where(x => x.IsActive && x.VatTypeInt == oldDeduction.VatTypeInt)
                .OrderByDescending(x => x.ID)
                .FirstOrDefault();
        }

        public static Deduction FindActiveDeductionForSupplier(int supplierID)
        {
            //Check for zero vat eligibility
            Supplier supplier = new SupplierRepository().Load(supplierID);

            if (supplier.ZeroVatEligible)
            {
                return null;
            }

            //---------------------------
            var deduction = new CatalogGroupRepository().FindActiveDeductionForSupplier(supplierID);

            if (deduction == null)
            {
                return FindDeductionFromDeductionVatType(enDeductionVatType.Small);
            }

            if (!deduction.IsActive)
            {
                deduction = FindActiveDeductionFromOldDeduction(deduction);
            }

            return deduction;
        }

        /// <summary>
        /// Checks if a catalog can be added to a group. 
        /// 1. The catalog should have a bookPrice set.
        /// 2. The book of the catalog should be active
        /// 3. If phase >= 13, the price should be verified by the committee
        /// </summary>
        /// <param name="catalog"></param>
        /// <returns></returns>
        public static bool CanAddToGroup(Catalog catalog)
        {
            if (catalog == null)
                return false;

            bool condition = catalog.BookPriceID.HasValue && catalog.IsBookActive && catalog.GroupID == null;
            condition = PendingPriceVerificationCondition(catalog, condition);

            return condition;
        }

        public static bool CanApproveGroup(CatalogGroupInfo group)
        {
            if (group == null)
                return false;

            return group.CanBePaid
                    && group.GroupStateInt == enCatalogGroupState.Selected.GetValue();
        }

        public static bool CanMovePhase(Catalog catalog)
        {
            if (catalog == null)
                return false;

            bool condition = catalog.Amount <= 0m;
            condition = PendingPriceVerificationCondition(catalog, condition);

            return condition;
        }

        public static bool CanRevertApproval(CatalogGroupInfo group)
        {
            if (group == null)
                return false;

            return !group.IsLocked
                    && group.GroupStateInt == enCatalogGroupState.Approved.GetValue();
        }

        public static bool CanSendToYDE(CatalogGroupInfo group)
        {
            if (group == null)
                return false;

            return group.CanBePaid
                    && (group.GroupStateInt == enCatalogGroupState.Approved.GetValue()
                        || group.GroupStateInt == enCatalogGroupState.Returned.GetValue());
        }

        public static bool CanReturnFromYDE(CatalogGroupInfo group)
        {
            if (group == null)
                return false;

            return !group.IsLocked
                    && group.GroupStateInt == enCatalogGroupState.Sent.GetValue();
        }

        public static bool CanEditGroup(CatalogGroupInfo group)
        {
            if (group == null)
                return false;

            return !group.IsLocked
                    && (group.GroupStateInt == enCatalogGroupState.New.GetValue()
                    || group.GroupStateInt == enCatalogGroupState.Selected.GetValue()
                    || group.GroupStateInt == enCatalogGroupState.Returned.GetValue());
        }

        public static bool CanDeleteGroup(CatalogGroupInfo group)
        {
            if (group == null)
                return false;

            return !group.IsLocked
                    && group.GroupStateInt == enCatalogGroupState.New.GetValue();
        }

        public static bool CanAddInvoice(CatalogGroup group, EudoxusOsyPrincipal user)
        {
            return !(group.IsLocked
                || group.State == enCatalogGroupState.Sent
                || (user.IsInRole(RoleNames.Supplier) && group.StateInt > (int)enCatalogGroupState.New));
        }


        private static bool PendingPriceVerificationCondition(Catalog catalog, bool condition)
        {
            bool checkPendingPriceVerification = catalog.PhaseID >= 13;

            if (checkPendingPriceVerification)
            {
                condition = condition && !catalog.HasPendingPriceVerification && !catalog.HasUnexpectedPriceChange;
            }
            return condition;
        }

        /// <summary>
        /// A function to update the bookprice price for books that have no grouped catalogs
        /// </summary>
        /// <param name="bookID"></param>
        /// <param name="phaseID"></param>
        /// <param name="uow"></param>
        public static void RecalculateCatalogsForBook(int bookID, int phaseID, decimal newPrice, int newBookPriceID, IUnitOfWork uow)
        {
            /** step 1. Find the ungrouped catalogs or the catalogs that are in groups of state NEW
                these are the catalogs to recalculate.
            */
            var catalogsToRecalculate = new CatalogRepository(uow).FindCatalogsToRecalculate(bookID, phaseID).ToList();

            foreach (var catalog in catalogsToRecalculate)
            {
                catalog.Amount = catalog.RecalculateAmount(newPrice);
                catalog.BookPriceID = newBookPriceID;
            }

            uow.Commit();
        }


        // create new catalogs with the price difference for books that have already processed groups
        public static void CreatePriceDifferenceCatalogsForBook(int bookID, int phaseID, decimal newPrice, int newBookPriceID, IUnitOfWork uow)
        {
            var catalogsForPriceDifference = new CatalogRepository(uow).FindCatalogsForPriceDifferenceReversal(bookID, phaseID).ToList();
            catalogsForPriceDifference.ForEach(x =>
            {
                var priceDifference = newPrice - x.BookPrice.Price;

                var newCatalog = new Catalog()
                {
                    OriginalPhaseID = x.PhaseID,
                    PhaseID = x.PhaseID,
                    BookID = x.BookID,
                    Amount = x.RecalculateAmount(priceDifference),
                    Percentage = x.Percentage,
                    CatalogType = priceDifference > 0 ? enCatalogType.Special : enCatalogType.Reversal,
                    BookCount = x.BookCount,
                    DepartmentID = x.DepartmentID,
                    DiscountID = x.DiscountID,
                    IsBookActive = x.IsBookActive,
                    State = enCatalogState.Normal,
                    Status = enCatalogStatus.Active,
                    SupplierID = x.SupplierID,
                    CreatedAt = DateTime.Today,
                    CreatedBy = HttpContext.Current.User.Identity.Name,
                    BookPriceID = newBookPriceID,
                    OldCatalogID = x.ID
                };

                uow.MarkAsNew(newCatalog);
                uow.Commit();
            });
        }

        public static string InabilityToCreateGroup(Catalog catalog)
        {
            if (!catalog.IsBookActive)
            {
                return "Το βιβλίο είναι ανενεργό.";
            }
            else if (catalog.HasPendingPriceVerification)
            {
                return "Η τιμή του βιβλίου ελέγχεται από την επιτροπή κοστολόγησης.";
            }
            else if (catalog.HasUnexpectedPriceChange)
            {
                return "Το βιβλίο έχει μη αναμενόμενη αλλαγή τιμής.";
            }

            return "";
        }

        public static enPaymentOrderState MapCatalogGroupStateToPaymentOrderState(enCatalogGroupState catalogGroupState)
        {
            switch (catalogGroupState)
            {
                case enCatalogGroupState.New:
                    return enPaymentOrderState.New;
                case enCatalogGroupState.Approved:
                    return enPaymentOrderState.Approved;
                case enCatalogGroupState.Returned:
                    return enPaymentOrderState.Returned;
                case enCatalogGroupState.Selected:
                    return enPaymentOrderState.Selected;
                case enCatalogGroupState.Sent:
                    return enPaymentOrderState.Sent;
            }

            return enPaymentOrderState.New;
        }

        public static CatalogGroupInfo ToCatalogGroupInfo(this CatalogGroup group)
        {
            return new CatalogGroupInfo()
            {
                ID = group.ID,
                SupplierID = group.SupplierID,
                InstitutionID = group.InstitutionID,
                GroupStateInt = group.StateInt,
                ContainsInActiveBooks = group.Catalogs.Any(y => !y.IsBookActive),
                HasPendingPriceVerification = group.PhaseID >= 13 && group.Catalogs.Any(c => c.HasPendingPriceVerification),
                HasUnexpectedPriceChange = group.PhaseID >= 13 && group.Catalogs.Any(c => c.HasUnexpectedPriceChange),
                IsLocked = group.IsLocked,
                CatalogCount = group.Catalogs.Where(y => y.StateInt == (int)enCatalogState.Normal || y.StateInt == (int)enCatalogState.FromMove).Count(),
                TotalAmount = group.Catalogs.Where(y => y.StateInt == (int)enCatalogState.Normal || y.StateInt == (int)enCatalogState.FromMove).Sum(y => y.Amount),
                InvoiceCount = group.Invoices.Where(y => y.IsActive).Count(),
                InvoiceSum = group.Invoices.Where(y => y.IsActive).Sum(y => y.InvoiceValue),
                Deduction = group.Deduction,
                Vat = group.Vat,
                IsTransfered = group.IsTransfered,
                TransferedBankID = group.BankID
            };
        }

    }
}
