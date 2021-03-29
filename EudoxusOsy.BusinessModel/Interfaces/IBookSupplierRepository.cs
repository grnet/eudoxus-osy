using System.Collections.Generic;

namespace EudoxusOsy.BusinessModel
{
    public interface IBookSupplierRepository : IBaseRepository<BookSupplier, int>
    {
        List<BookSupplier> FindBySupplierIDAndBookIDAndYear(int supplierID, int bookID, int currentYear);
    }
}