namespace EudoxusOsy.BusinessModel
{
    public interface IBookPricesGridVRepository : IBaseRepository<BookPricesGridV, int>
    {
        BookPricesGridV FindByBookPriceID(int bookPriceId);
    }
}