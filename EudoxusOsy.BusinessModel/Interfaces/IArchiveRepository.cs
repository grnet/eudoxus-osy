using System.Collections.Generic;

namespace EudoxusOsy.BusinessModel
{
    public interface IArchiveRepository : IBaseRepository<Archive, int>
    {
        List<Archive> GetActive();

    }
}