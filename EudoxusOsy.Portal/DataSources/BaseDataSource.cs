using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using EudoxusOsy.BusinessModel;
using Imis.Domain;
using Imis.Domain.EF;

namespace EudoxusOsy.Portal.DataSources
{
    public class BaseDataSource<TEntity> where TEntity : DomainEntity<DBEntities>
    {
        protected int _RecordCount = 0;

        public int CountWithCriteria(Criteria<TEntity> criteria)
        {
            return _RecordCount;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<TEntity> FindWithCriteria(Criteria<TEntity> criteria, int startRowIndex, int maximumRows, string sortExpression)
        {
            int recordCount;

            criteria.StartRowIndex = startRowIndex;
            criteria.MaximumRows = maximumRows;

            if (!string.IsNullOrEmpty(sortExpression))
            {
                criteria.Sort.Expression = sortExpression;
            }

            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                var results = new BaseRepository<TEntity>(uow).FindWithCriteria(criteria, out recordCount);
                _RecordCount = recordCount;

                if (!criteria.UsePaging && criteria.MaximumRows > 0)
                {
                    return results
                            .Skip(startRowIndex)
                            .Take(maximumRows)
                            .ToList();
                }

                return results;
            }
        }
    }
}