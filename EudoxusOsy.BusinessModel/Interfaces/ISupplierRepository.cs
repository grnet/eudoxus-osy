using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public interface ISupplierRepository : IBaseRepository<Supplier, int>
    {
        Supplier FindByKpsID(int id, params Expression<Func<Supplier, object>>[] includeExpressions);
        List<Supplier> FindManyByKpsID(List<int> kpsIDs,
            params Expression<Func<Supplier, object>>[] includeExpressions);
        Supplier FindByUsername(string username, params Expression<Func<Supplier, object>>[] includeExpressions);
        Supplier FindByEmail(string email);
        IList<Supplier> GetAllActive(params Expression<Func<Supplier, object>>[] includeExpressions);
        List<CoAuthors> GetCoAuthors(int phaseID);
        List<SuppliersNoLogisticBooks> GetSuppliersNoLogisticBooks(int phaseID);
        List<Commitments> GetCommitments(int phaseID);
        IQueryable<SupplierFullStatistics> GetCurrentPhaseStatistics(int? supplierKpsID, string afm, string name,
            int? supplierType, int phaseID, out int recordCount);
        IQueryable<SuppliersStatsForExport> GetSupplierStatsForExport(int phaseID);
        List<SyncPublisherDto> GetManuallyInsertedSuppliers();

    }
}