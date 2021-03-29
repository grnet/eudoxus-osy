namespace EudoxusOsy.BusinessModel.Interfaces
{
    public interface IPaymentOrderRepository : IBaseRepository<PaymentOrder, int>
    {
        PaymentOrder FindByGroupID(int groupID);
        int FindMaxOfficeSlipNumber(int year);
    }
}
