using Imis.Domain.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public interface ICatalogRepository : IBaseRepository<Catalog, int>
    {
        Catalog FindByID(int id);
        List<Catalog> FindByBookID(int bookID);
        List<Catalog> FindByPhaseID(int phaseID);
        int CountByPhaseID(int phaseID);
        List<Catalog> FindByGroupID(int groupID);
        List<Catalog> FindByGroupIDWithBooks(int groupID);
        List<Catalog> FindConnectedCatalogsByGroupID(int groupID);
        List<Catalog> FindCatalogsToDeActivate(int bookID);
        List<Catalog> FindBySupplierID(int supplierID);
        List<Catalog> FindByDepartmentID(int departmentID);
        List<Catalog> FindByDiscountID(int discountID);
        List<Catalog> FindByBookPriceID(int bookPriceID);
        List<Catalog> FindByBookOldCatalogID(int oldCatalogID);
        List<Catalog> FindByNewCatalogID(int newCatalogID);
        IList<Catalog> GetUngroupedCatalogsBySupplierAndPhase(int supplierID, int phaseID);

        /// <summary>
        /// Get the catalogs that will be recalculated when a price change from KPS happens
        /// also catalogs without price will be calculated
        /// All this is valid for phase 13 and beyond
        /// </summary>
        /// <param name="bookID"></param>
        /// <param name="phaseID"></param>
        /// <returns></returns>
        IList<Catalog> FindCatalogsToRecalculate(int bookID, int phaseID);

        IList<Catalog> FindCatalogsForPriceDifferenceReversal(int bookID, int phaseID);

        /// <summary>
        /// Get the catalog groups to ungroup. Catalog groups that were reverted by the ministry to the previous state are not automatically ungrouped
        /// </summary>
        /// <param name="supplierID"></param>
        /// <param name="phaseID"></param>
        /// <returns></returns>
        IList<Catalog> GetCatalogsToUngroupBySupplierAndPhase(int supplierID, int phaseID);

        IList<Catalog> GetPossibleReversedCatalogs(int supplierID, int bookID, int departmentID, decimal amount);

        /// <summary>
        /// Find if there are any special or reversal catalogs created for the book in the given phase
        /// for phase 13 and beyond
        /// </summary>
        /// <param name="supplierID"></param>
        /// <param name="bookID"></param>
        /// <param name="departmentID"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        IList<Catalog> GetSpecialOrReversalCatalogsForBook(int bookID, int phaseID);

        /// <summary>
        /// Finds the total debt (for active catalogs for a given supplier and phase)
        /// </summary>
        /// <param name="supplierID"></param>
        /// <param name="phaseID"></param>
        /// <returns></returns>
        decimal? GetTotalDebtBySupplierAndPhase(int supplierID, int phaseID);

        Dictionary<int, decimal?> GetSupplierAmountPerPhase(int phaseID);
        /// <summary>
        /// Finds the debt (for active normal and 'from move' catalogs for a given supplier and phase)
        /// </summary>
        /// <param name="supplierID"></param>
        /// <param name="phaseID"></param>
        /// <returns></returns>
        decimal? GetSupplierMoneyDebt(int supplierID, int phaseID);

        /// <summary>
        /// Calculate for SENT groups to get the amount paid to the supplier
        /// </summary>
        /// <param name="supplierID"></param>
        /// <param name="phaseID"></param>
        /// <returns></returns>
        decimal? GetSupplierPaidMoney(int supplierID, int phaseID, int groupID);

        /// <summary>
        /// calculate for ALL groups OTHER THAN 'NEW' to get the remaining Amount
        /// </summary>
        /// <param name="supplierID"></param>
        /// <param name="phaseID"></param>
        /// <returns></returns>
        decimal? GetSupplierRemainingMoney(int supplierID, int phaseID, int groupID);

        /// <summary>
        /// Creates the catalogs from the individual receipts for the given phase
        /// </summary>
        /// <param name="phaseID"></param>
        void CreateCatalogsForPhase(int phaseID);

        List<int> GetBooksWithCatalogsInYear(IEnumerable<int> bookIDs, int year);        
        EntityKey CreateEntityKey(object value);
        EntityKey CreateEntityKey(object value, string idName);
        Catalog Load(int ID);
        Catalog Load(int ID, IEnumerable<string> includes);
        Catalog Load(int ID, params Expression<Func<Catalog, object>>[] includeExpressions);
        ObjectQuery<Catalog> LoadMany(IEnumerable<int> IDs);
        ObjectQuery<Catalog> LoadAll();
        void Delete(Catalog domainEntity);
        void Save(Catalog domainEntity);
        void Refresh(Catalog domainEntity);
        ObjectQuery<Catalog> LoadMany(IEnumerable<int> IDs, params string[] includeProperties);
        ObjectQuery<Catalog> LoadMany(IEnumerable<int> IDs, params Expression<Func<Catalog, object>>[] includeExpressions);
        ObjectQuery<Catalog> LoadAll(params string[] includeProperties);
        ObjectQuery<Catalog> LoadAll(params Expression<Func<Catalog, object>>[] includeExpressions);
        List<Catalog> FindWithCriteria(DomainCriteria<Catalog> criteria, out int totalRecordCount);
        void Attach(Catalog domainEntity);
        void Insert(Catalog domainObject);
        int ContextSaveChanges(DBEntities context);
    }
}