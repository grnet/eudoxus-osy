using EudoxusOsy.BusinessModel;
using Imis.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EudoxusOsy.Portal.DataSources
{
    public class CatalogGroups
    {
        protected int _RecordCount = 0;

        public int CountWithCriteria(int supplierID, int phaseID, int? groupID)
        {
            return _RecordCount;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IList<CatalogGroupInfo> FindWithSupplierPhaseAndGroup(int supplierID, int phaseID, int? groupID, int startRowIndex, int maximumRows, string sortExpression)
        {
            int recordCount;

            if (groupID == 0)
            {
                groupID = null;
            }

            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                var results = new CatalogGroupRepository(uow).GetBySupplierAndPhase(supplierID, phaseID, groupID, startRowIndex, maximumRows, sortExpression, out recordCount);
                _RecordCount = recordCount;

                return results;                        
            }
        }
   }
}