using System;
using System.Collections.Generic;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Objects.SqlClient;

namespace EudoxusOsy.BusinessModel
{
    public class CatalogGroupLogRepository : DomainRepository<DBEntities, CatalogGroupLog, int>
    {
        #region [ Base .ctors ]

        public CatalogGroupLogRepository() : base() { }

        public CatalogGroupLogRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public CatalogGroupLog FindByGroupID(int groupID)
        {
            return BaseQuery
                    .FirstOrDefault(x => x.GroupID == groupID);
        }

        public int FindMaxOfficeSlipNumber(int year)
        {
            var stringYear = year.ToString();
            var maxValue = BaseQuery.Where(x => SqlFunctions.StringConvert((double)x.OfficeSlipNumber.Value).StartsWith(stringYear)).Max(x=> x.OfficeSlipNumber).ToString();
            if (string.IsNullOrEmpty(maxValue) || maxValue.Substring(0, 4) != year.ToString())
            {
                maxValue = "0";
            }
            else
            {
                maxValue = maxValue.Substring(4, maxValue.Length - 4);
            }
            return Convert.ToInt32(maxValue);

        }
    }
}
