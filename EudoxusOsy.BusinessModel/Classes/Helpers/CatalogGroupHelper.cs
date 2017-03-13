using Imis.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EudoxusOsy.BusinessModel
{
    public static class CatalogGroupHelper
    {
        public static void GroupCatalogsByInstitution(int supplierID, int phaseID, IUnitOfWork uow)
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

            bool condition = catalog.BookPriceID.HasValue && catalog.IsBookActive;
            condition = PendingPriceVerificationCondition(catalog, condition);

            return condition;
        }

        public static bool CanMovePhase(Catalog catalog)
        {
            if (catalog == null)
                return false;

            bool condition = catalog.Amount <= 0m;
            condition = PendingPriceVerificationCondition(catalog, condition);

            return condition;
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
        public static void RecalculateCatalogsForBook(int bookID, int phaseID, decimal newPrice, int newBookPriceID,  IUnitOfWork uow)
        {
            /** step 1. Find the ungrouped catalogs or the catalogs that are in groups of state NEW
                these are the catalogs to recalculate.
            */
            var catalogsToRecalculate = new CatalogRepository(uow).FindCatalogsToRecalculate(bookID, phaseID).ToList();

            catalogsToRecalculate.ForEach(x =>
            {
                x.Amount = x.BookCount * Math.Round(x.Discount.DiscountPercentage * newPrice * (x.Percentage.Value / 100) * x.Book.FirstRegistrationYearDiscountPercentage(x.PhaseID.Value), 2);
                x.BookPriceID = newBookPriceID;
            });

            uow.Commit();
        }


        // create new catalogs with the price difference for books that have already processed groups
        public static void CreatePriceDifferenceCatalogsForBook(int bookID, int phaseID, decimal newPrice, int newBookPriceID, IUnitOfWork uow)
        {
            var currentPhase = EudoxusOsyCacheManager<Phase>.Current.Get(phaseID);
            var newCatalogs = new List<Catalog>();

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
                newCatalogs.Add(newCatalog);
            });

            uow.Commit();
        }

    }
}
