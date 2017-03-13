using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public class ReporterRepository : DomainRepository<DBEntities, Reporter, int>
    {
        #region [ Base .ctors ]

        public ReporterRepository() : base() { }

        public ReporterRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        //public List<Reporter> FindReportersWithCriteria(ReporterCriteria criteria, out int totalRecordCount)
        //{
        //    var query = BaseQuery;

        //    if (criteria.Includes != null)
        //    {
        //        criteria.Includes.ForEach(x => query = query.Include(x));
        //    }

        //    if (!string.IsNullOrEmpty(criteria.Expression.CommandText))
        //    {
        //        query = query.Where(criteria.Expression.CommandText, criteria.Expression.Parameters);
        //    }

        //    if (string.IsNullOrEmpty(criteria.Sort.Expression))
        //        criteria.Sort.Expression = "it.CreatedAt DESC";
        //    else
        //        criteria.Sort.Expression = "it." + criteria.Sort.Expression.Replace(",", ",it.");

        //    if (criteria.UsePaging)
        //    {
        //        totalRecordCount = query.Count();

        //        return query
        //                .OrderBy(criteria.Sort.Expression)
        //                .Skip(criteria.StartRowIndex)
        //                .Take(criteria.MaximumRows)
        //                .ToList();
        //    }

        //    var retValue = query.ToList();
        //    totalRecordCount = retValue.Count;

        //    return retValue;
        //}

        public Reporter FindByID<T>(int id)
            where T : Reporter
        {
            return BaseQuery
                    .OfType<T>()
                    .FirstOrDefault(x => x.ID == id);
        }

        public enReporterType FindReporterTypeByUsername(string username)
        {
            return (enReporterType)BaseQuery
                    .Where(x => x.Username == username)
                    .Select(x => x.ReporterTypeInt)
                    .FirstOrDefault();
        }

        public Reporter FindByUsername(string username, params Expression<Func<Reporter, object>>[] includeExpressions)
        {
            var query = BaseQuery;

            if (includeExpressions.Length > 0)
            {
                foreach (var item in includeExpressions)
                    query = query.Include(item);
            }

            return query
                    .Where(x => x.Username == username)
                    .FirstOrDefault();
        }

        public Reporter FindByEmail(string email)
        {
            return BaseQuery
                    .Where(x => x.Email == email)
                    .FirstOrDefault();
        }

        //public Reporter FindByEmailVerificationCode(string emailVerificationCode)
        //{
        //    return BaseQuery
        //            .Where(x => x.EmailVerificationCode == emailVerificationCode)
        //            .FirstOrDefault();
        //}

        //public bool MobilePhoneExists(int reporterID, string mobilePhone)
        //{
        //    return BaseQuery
        //            .Where(x => x.ContactMobilePhone == mobilePhone)
        //            .Where(x => x.IsMobilePhoneVerified == true)
        //            .Any(x => x.ID != reporterID);
        //}
    }
}