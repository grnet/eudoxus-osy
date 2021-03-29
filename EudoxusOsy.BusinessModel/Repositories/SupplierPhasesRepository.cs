using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace EudoxusOsy.BusinessModel
{
    public class SupplierPhaseRepository : DomainRepository<DBEntities, SupplierPhase, int>, ISupplierPhaseRepository
    {
        #region [ Base .ctors ]

        public SupplierPhaseRepository() : base() { }

        public SupplierPhaseRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public SupplierPhase FindByID<T>(int id)
           where T : SupplierPhase
        {
            return BaseQuery
                    .OfType<T>()
                    .FirstOrDefault(x => x.ID == id);
        }

        /// <summary>
        /// Get the Total debt for the given supplier and phase
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="supplierID"></param>
        /// <param name="phaseID"></param>
        /// <returns></returns>
        public double? GetSupplierPhaseMoney<T>(int supplierID, int phaseID)
           where T : SupplierPhase
        {
            var supplierPhase = BaseQuery
                    .OfType<T>()
                    .FirstOrDefault(x => x.SupplierID == supplierID
                        && x.PhaseID == phaseID
                        && x.IsActive);
            if (supplierPhase != null)
            {
                return supplierPhase.TotalDebt;
            }
            else
            {
                return 0;
            }
        }

        public SupplierPhase GetSupplierPhase(int supplierID, int phaseID)
        {
            return BaseQuery
                    .FirstOrDefault(x => x.SupplierID == supplierID
                        && x.PhaseID == phaseID
                        && x.IsActive);
        }

        public IList<SupplierPhase> GetAllActive(int phaseID)
        {
            return BaseQuery.Where(x => x.IsActive && x.PhaseID == phaseID).ToList();
        }

        public double? GetPhaseMoney(int phaseID)
        {            
            return BaseQuery.Where(x => x.IsActive && x.PhaseID == phaseID).Sum(x => x.TotalDebt);
        }

    }
}
