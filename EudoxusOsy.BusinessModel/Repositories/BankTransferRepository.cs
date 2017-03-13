using System;
using System.Collections.Generic;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public class BankTransferRepository : DomainRepository<DBEntities, BankTransfer, int>
    {
        #region [ Base .ctors ]

        public BankTransferRepository() : base() { }

        public BankTransferRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public BankTransfer FindByID(int id)
        {
            return BaseQuery
                    .FirstOrDefault(x => x.ID == id);
        }

        public List<BankTransfer> FindBySupplierIDAndPhaseID(Criteria<BankTransfer> criteria, out int recordCount)
        {
            return FindWithCriteria(criteria, out recordCount);
        }

        public bool BankExistsInTransfer(int bankID)
        {
            return BaseQuery.Any(x => x.BankID == bankID);
        }
    }
}
