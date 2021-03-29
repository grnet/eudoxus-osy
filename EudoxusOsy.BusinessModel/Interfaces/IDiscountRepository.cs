namespace EudoxusOsy.BusinessModel
{
    public interface IDiscountRepository : IBaseRepository<Discount, int>
    {
        Discount FindGeneralForPhase(int phaseID);
    }
}