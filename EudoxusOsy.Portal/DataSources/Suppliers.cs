using EudoxusOsy.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EudoxusOsy.Portal.DataSources
{
    public class Suppliers : BaseDataSource<Supplier>
    {

        public List<SupplierFullStatistics> GetSuppliersStats(int? supplierKpsID, string afm, string name, int? supplierType, int phaseID, int startRowIndex, int maximumRows, string sortExpression)
        {
            maximumRows = 20;
            int recordCount = 0;
            using (Imis.Domain.IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                var result = new SupplierRepository(uow).GetCurrentPhaseStatistics(supplierKpsID, afm, name, supplierType, phaseID, out recordCount);
                _RecordCount = recordCount;

                if (string.IsNullOrEmpty(sortExpression))
                {
                    sortExpression = "supplier_kpsid ASC";
                }

                return result.OrderBy(sortExpression).Skip(startRowIndex).Take(maximumRows).ToList();
            }
        }


        public int CountResult(int? supplierKpsID, string afm, string name, int? supplierType, int phaseID)
        {
            return _RecordCount;
        }
    }
}