using Imis.Domain.EF;
using System.Linq;

namespace EudoxusOsy.BusinessModel
{
    public class DiscountRepository : DomainRepository<DBEntities, Discount, int>, IDiscountRepository
    {
        #region [ Base .ctors ]

        public DiscountRepository() : base() { }

        public DiscountRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public Discount FindByID(int id)
        {
            return BaseQuery
                    .FirstOrDefault(x => x.ID == id);
        }

        public Discount FindGeneralForPhase(int phaseID)
        {
            return BaseQuery
                    .FirstOrDefault(x => x.PhaseID == phaseID
                    && !x.BookID.HasValue);
        }
    }
}
