using System.Collections.Generic;

namespace EudoxusOsy.BusinessModel
{
    public interface IPhaseRepository: IBaseRepository<Phase, int>
    {
        List<Phase> GetAllActive();
        Phase GetCurrentCatalogsPhase();
        Phase GetCurrentPhase();
        bool IsActive(int id);
    }
}