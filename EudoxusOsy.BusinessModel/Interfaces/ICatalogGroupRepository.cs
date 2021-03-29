using System.Collections.Generic;

namespace EudoxusOsy.BusinessModel
{
    public interface ICatalogGroupRepository : IBaseRepository<CatalogGroup, int>
    {
        CatalogGroup FindByID(int id);
        List<CatalogGroup> FindByPhaseID(int phaseID);
        List<CatalogGroup> FindBySupplierID(int supplierID);
        CatalogGroup FindByIDandSupplierID(int id, int supplierID);
        List<CatalogGroup> FindBySupplierIDAndPhaseIDWithCatalogs(int supplierID, int phaseID);
        List<CatalogGroup> FindBySupplierIBANID(int supplierIBANID);
        List<CatalogGroup> FindByInstitutiooID(int institutionID);
        List<CatalogGroup> FindByBankID(int bankID);
        CatalogGroupInfo GetByID(int groupID);
        IList<CatalogGroupInfo> GetBySupplierAndPhase(int supplierID, int phaseID, int? groupID,
           int startRowIndex, int maximumRows, string sortExpression, out int recordCount);
        bool BankExistsInCatalogGroup(int bankID);
        Deduction FindActiveDeductionForSupplier(int supplierID);
    }
}