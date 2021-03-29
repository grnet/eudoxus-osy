namespace EudoxusOsy.BusinessModel
{
    public interface IBookPriceRepository : IBaseRepository<BookPrice, int>
    {
        BookPrice FindByBookIDAndYear(int bookID, int phaseID);
    }
}