namespace EudoxusOsy.BusinessModel
{
    public interface ISupplierPhaseRepository : IBaseRepository<SupplierPhase, int>
    {
        double? GetSupplierPhaseMoney<T>(int supplierID, int phaseID) where T : SupplierPhase;
        SupplierPhase GetSupplierPhase(int supplierID, int phaseID);
        double? GetPhaseMoney(int phaseID);
    }
}