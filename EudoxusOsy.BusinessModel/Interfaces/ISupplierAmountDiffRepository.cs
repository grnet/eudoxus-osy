using System.Collections.Generic;

namespace EudoxusOsy.BusinessModel
{
    public interface ISupplierAmountDiffRepository : IBaseRepository<TempAmountDiff, int>
    {
        void RefreshAmountDiffs(int phaseID);
    }
}