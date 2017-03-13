using EudoxusOsy.BusinessModel;
using Imis.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EudoxusOsy.Portal.DataSources
{
    public class BankTransfers
    {
        protected int _RecordCount = 0;

        public int CountWithCriteria(Criteria<BankTransfer> criteria)
        {
            return _RecordCount;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IList<BankTransfer> FindBySupplierIDAndPhaseID(Criteria<BankTransfer> criteria, int startRowIndex, int maximumRows, string sortExpression)
        {
            int recordCount;

            criteria.UsePaging = true;
            criteria.StartRowIndex = startRowIndex;
            criteria.MaximumRows = maximumRows;
            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                var results = new BankTransferRepository(uow).FindBySupplierIDAndPhaseID(criteria, out recordCount);
                _RecordCount = recordCount;

                return results;                        
            }
        }
    }
}