using System.Collections.Generic;
using Imis.Domain.EF;

namespace EudoxusOsy.BusinessModel
{
    public interface IEditCatalogsGridVRepository : IBaseRepository<EditCatalogsGridV, int>
    {
        List<EditCatalogsGridV> FindWithCriteria(DomainCriteria<EditCatalogsGridV> criteria, out int totalRecordCount);
    }
}