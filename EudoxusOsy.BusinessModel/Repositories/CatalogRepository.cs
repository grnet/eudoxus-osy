using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EudoxusOsy.BusinessModel
{
    public class CatalogRepository : DomainRepository<DBEntities, Catalog, int>
    {
        #region [ Base .ctors ]

        public CatalogRepository() : base() { }

        public CatalogRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public Catalog FindByID(int id)
        {
            return BaseQuery
                    .FirstOrDefault(x => x.ID == id);
        }

        public List<Catalog> FindByBookID(int bookID)
        {
            return BaseQuery
                .Include(x => x.Book)
                .Include(x => x.CatalogGroup)
                .Include(x => x.BookPrice)
                .Include(x => x.Phase)
                .Include(x => x.CatalogGroup.PaymentOrders)
                    .Where(x => x.BookID == bookID && x.StatusInt >= (int)enCatalogStatus.Active).ToList();
        }

        public List<Catalog> FindByPhaseID(int phaseID)
        {
            return BaseQuery
                    .Where(x => x.PhaseID == phaseID).ToList();
        }

        public List<Catalog> FindByGroupID(int groupID)
        {
            return BaseQuery
                    .Where(x => x.GroupID == groupID).ToList();
        }

        public List<Catalog> FindByGroupIDWithBooks(int groupID)
        {
            return BaseQuery
                .Include(x => x.Book)
                .Include(x => x.BookPrice)
                    .Where(x => x.GroupID == groupID).ToList();
        }

        public List<Catalog> FindConnectedCatalogsByGroupID(int groupID)
        {
            return BaseQuery
                    .Include(x => x.Book)
                    .Include(x => x.Department)
                    .Where(x => x.GroupID == groupID)
                    .Where(x => x.StatusInt == (int)enCatalogStatus.Active)
                    .Where(x => (x.StateInt == (int)enCatalogState.Normal || x.StateInt == (int)enCatalogState.FromMove))
                    .ToList();
        }

        public List<Catalog> FindCatalogsToDeActivate(int bookID)
        {
            return BaseQuery
                    .Where(x => x.BookID == bookID)
                    .Where(x => x.StatusInt == (int)enCatalogStatus.Active)
                    .Where(x => (x.StateInt == (int)enCatalogState.Normal || x.StateInt == (int)enCatalogState.FromMove))
                    .ToList();
        }

        public List<Catalog> FindBySupplierID(int supplierID)
        {
            return BaseQuery
                    .Where(x => x.SupplierID == supplierID).ToList();
        }

        public List<Catalog> FindByDepartmentID(int departmentID)
        {
            return BaseQuery
                    .Where(x => x.DepartmentID == departmentID).ToList();
        }

        public List<Catalog> FindByDiscountID(int discountID)
        {
            return BaseQuery
                    .Where(x => x.DiscountID == discountID).ToList();
        }

        public List<Catalog> FindByBookPriceID(int bookPriceID)
        {
            return BaseQuery
                    .Where(x => x.BookPriceID == bookPriceID).ToList();
        }

        public List<Catalog> FindByBookOldCatalogID(int oldCatalogID)
        {
            return BaseQuery
                    .Where(x => x.OldCatalogID == oldCatalogID).ToList();
        }

        public List<Catalog> FindByNewCatalogID(int newCatalogID)
        {
            return BaseQuery
                    .Where(x => x.NewCatalogID == newCatalogID).ToList();
        }

        public IList<Catalog> GetUngroupedCatalogsBySupplierAndPhase(int supplierID, int phaseID)
        {
            var myQuery = BaseQuery
                .Include(x => x.Book)
                .Include(x => x.Department.Institution);


            var query = myQuery
                        .Where(x => x.SupplierID == supplierID
                        && x.PhaseID == phaseID
                        && !x.GroupID.HasValue
                        && x.BookPriceID != null
                        && (x.StateInt == (int)enCatalogState.FromMove || x.StateInt == (int)enCatalogState.Normal)
                        && x.StatusInt == (int)enCatalogStatus.Active
                        && (x.PhaseID.Value < 13 || (!x.HasPendingPriceVerification && !x.HasUnexpectedPriceChange))
                        && x.IsBookActive);

            return query
                    .OrderBy(x => x.ID)
                    .ToArray<Catalog>();
        }

        /// <summary>
        /// Get the catalogs that will be recalculated when a price change from KPS happens
        /// also catalogs without price will be calculated
        /// All this is valid for phase 13 and beyond
        /// </summary>
        /// <param name="bookID"></param>
        /// <param name="phaseID"></param>
        /// <returns></returns>
        public IList<Catalog> FindCatalogsToRecalculate(int bookID, int phaseID)
        {
            var myQuery = BaseQuery
                .Include(x => x.Book)
                .Include(x => x.BookPrice)
                .Include(x => x.Discount)
                .Include(x => x.CatalogGroup)
                .Include(x => x.Department.Institution);



            var query = myQuery
                        .Where(x => x.PhaseID == phaseID
                        && x.BookID == bookID
                        && x.PhaseID == phaseID
                        && x.PhaseID >= 13
                        && (!x.GroupID.HasValue || (x.CatalogGroup.StateInt == (int)enCatalogGroupState.New))
                        && (x.StateInt == (int)enCatalogState.FromMove || x.StateInt == (int)enCatalogState.Normal)
                        && x.StatusInt == (int)enCatalogStatus.Active
                        && x.IsBookActive);

            return query
                    .OrderBy(x => x.ID)
                    .ToArray<Catalog>();
        }

        //find the catalogs to create price difference catalogs for
        public IList<Catalog> FindCatalogsForPriceDifferenceReversal(int bookID, int phaseID)
        {
            var myQuery = BaseQuery
                .Include(x => x.Book)
                .Include(x => x.BookPrice)
                .Include(x => x.Discount)
                .Include(x => x.CatalogGroup)
                .Include(x => x.Department.Institution);


            var query = myQuery
                        .Where(x => x.PhaseID == phaseID
                        && x.BookID == bookID
                        && x.PhaseID == phaseID
                        && x.PhaseID >= 13
                        && (x.GroupID.HasValue && (x.CatalogGroup.StateInt > (int)enCatalogGroupState.New))
                        && (x.StateInt == (int)enCatalogState.FromMove || x.StateInt == (int)enCatalogState.Normal)
                        && x.StatusInt == (int)enCatalogStatus.Active
                        && x.IsBookActive);

            return query
                    .OrderBy(x => x.ID)
                    .ToArray<Catalog>();
        }

        /// <summary>
        /// Get the catalog groups to ungroup. Catalog groups that were reverted by the ministry to the previous state are not automatically ungrouped
        /// </summary>
        /// <param name="supplierID"></param>
        /// <param name="phaseID"></param>
        /// <returns></returns>
        public IList<Catalog> GetCatalogsToUngroupBySupplierAndPhase(int supplierID, int phaseID)
        {
            var myQuery = BaseQuery
                            .Include(x => x.CatalogLogs)
                            .Include(x => x.CatalogGroup.LockedCatalogGroups)
                            .Include(x => x.CatalogGroup.Invoices);

            var query = myQuery
                        .Where(x => x.SupplierID == supplierID
                        && x.PhaseID == phaseID
                        && x.GroupID.HasValue
                        && x.CatalogGroup.StateInt == (int)enCatalogGroupState.New
                        && !x.CatalogGroup.LockedCatalogGroups.Any()
                        && (!x.CatalogGroup.ReversalCount.HasValue || x.CatalogGroup.ReversalCount == 0));

            return query
                    .OrderBy(x => x.ID)
                    .ToArray<Catalog>();
        }

        public IList<Catalog> GetPossibleReversedCatalogs(int supplierID, int bookID, int departmentID, decimal amount)
        {
            var myQuery = BaseQuery.Include(x => x.CatalogGroup);

            var query = myQuery
                        .Where(x => x.SupplierID == supplierID
                        && x.BookID == bookID
                        && x.DepartmentID == departmentID
                        && x.StatusInt == (int)enCatalogStatus.Active
                        && (Math.Abs(x.Amount.Value) > (Math.Abs(amount) - 0.01m) && Math.Abs(x.Amount.Value) < (Math.Abs(amount) + 0.01m))
                        && x.Amount.Value < 0);

            return query
                    .OrderBy(x => x.ID)
                    .ToArray<Catalog>();
        }

        /// <summary>
        /// Finds the debt (for active normal and 'from move' cataloga for a given supplier and phase)
        /// </summary>
        /// <param name="supplierID"></param>
        /// <param name="phaseID"></param>
        /// <returns></returns>
        public decimal? GetSupplierMoneyDebt(int supplierID, int phaseID)
        {
            decimal? amount = 0m;

            amount = BaseQuery.Where(x => x.SupplierID == supplierID
            && x.PhaseID == phaseID
            && (x.StateInt == (int)enCatalogState.Normal || x.StateInt == (int)enCatalogState.FromMove)
            && x.StatusInt == (int)enCatalogStatus.Active)
            .Sum(x => x.Amount);

            return amount;
        }

        /// <summary>
        /// Calculate for SENT groups to get the amount paid to the supplier
        /// </summary>
        /// <param name="supplierID"></param>
        /// <param name="phaseID"></param>
        /// <returns></returns>
        public decimal? GetSupplierPaidMoney(int supplierID, int phaseID, int groupID)
        {
            decimal? amount = 0m;

            amount = BaseQuery.Where(x => x.SupplierID == supplierID
            && x.PhaseID == phaseID
            && x.GroupID == groupID
            && (x.StateInt == (int)enCatalogState.Normal || x.StateInt == (int)enCatalogState.FromMove)
            && x.StatusInt == (int)enCatalogStatus.Active)
            .Sum(x => x.Amount);

            return amount;
        }

        /// <summary>
        /// calculate for ALL groups OTHER THAN 'NEW' to get the remaining Amount
        /// </summary>
        /// <param name="supplierID"></param>
        /// <param name="phaseID"></param>
        /// <returns></returns>
        public decimal? GetSupplierRemainingMoney(int supplierID, int phaseID, int groupID)
        {
            decimal? amount = 0m;

            amount = BaseQuery.Where(x => x.SupplierID == supplierID
            && x.StatusInt == (int)enCatalogStatus.Active
            && x.GroupID == groupID
            && x.PhaseID == phaseID
            && (x.StateInt == (int)enCatalogState.Normal || x.StateInt == (int)enCatalogState.FromMove))
            .Sum(x => x.Amount);

            return amount;
        }

        /// <summary>
        /// Creates the catalogs from the individual receipts for the given phase
        /// </summary>
        /// <param name="phaseID"></param>
        public void CreateCatalogsForPhase(int phaseID)
        {
            var ctx = GetCurrentObjectContext();
            var timeout = ctx.CommandTimeout;
            if (timeout == null || timeout < 600)
            {
                ctx.CommandTimeout = 600;
            }
            ctx.CreateCatalogsForPhase(phaseID);
        }
    }
}
