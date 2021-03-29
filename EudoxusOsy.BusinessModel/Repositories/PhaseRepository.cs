using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace EudoxusOsy.BusinessModel
{
    public class PhaseRepository : DomainRepository<DBEntities, Phase, int>, IPhaseRepository
    {
        #region [ Base .ctors ]

        public PhaseRepository() : base() { }

        public PhaseRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public Phase GetCurrentPhase()
        {
            return BaseQuery.Where(x => x.IsActive)
                    .OrderByDescending(x => x.ID)
                    .FirstOrDefault();
        }

        /// <summary>
        /// Returns the phase that is current for catalogs calculations
        /// and book price changes updates, the 'financial' current phase
        /// should be currentPhase - 1
        /// </summary>
        /// <returns></returns>
        public Phase GetCurrentCatalogsPhase()
        {
            return BaseQuery.Where(x => x.IsActive)
                    .Where(x=> x.CatalogsCreated)
                    .OrderByDescending(x => x.ID)
                    .FirstOrDefault();
        }

        public List<Phase> GetAllActive()
        {
            return BaseQuery.Where(x => x.IsActive).ToList();
        }

        public bool IsActive(int id)
        {
            return BaseQuery.Any(x => x.ID == id && x.IsActive);
        }

    }
}
