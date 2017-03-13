using System;
using System.Collections.Generic;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public class WelfareRecordEntryRepository : DomainRepository<DBEntities, WelfareRecordEntry, int>
    {
        #region [ Base .ctors ]

        public WelfareRecordEntryRepository() : base() { }

        public WelfareRecordEntryRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public WelfareRecordEntry FindByID(int id)
        {
            return BaseQuery
                    .FirstOrDefault(x => x.ID == id);
        }

        public WelfareRecordEntry FindByWelfareRecordID(int welfareRecordID)
        {
            return BaseQuery
                    .FirstOrDefault(x => x.WelfareRecordID == welfareRecordID);
        }

        public List<WelfareRecordEntry> FindByBookPriceRequestID(int bookPriceRequestID)
        {
            return BaseQuery
                    .Where(x => x.BookPriceRequestID == bookPriceRequestID).ToList();
        }

    }
}
