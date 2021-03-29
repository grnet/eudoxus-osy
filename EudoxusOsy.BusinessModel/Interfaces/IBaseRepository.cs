using Imis.Domain.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Objects;

namespace EudoxusOsy.BusinessModel
{
    public interface IBaseRepository<TEntity, TKey> where TEntity : DomainEntity<DBEntities>
    {
        List<TEntity> FindWithCriteria(DomainCriteria<TEntity> criteria, out int totalRecordCount);
        TEntity Load(TKey ID);
        TEntity Load(TKey ID, IEnumerable<string> includes);
        TEntity Load(TKey ID, params Expression<Func<TEntity, object>>[] includeExpressions);
        ObjectQuery<TEntity> LoadAll();
        ObjectQuery<TEntity> LoadAll(params Expression<Func<TEntity, object>>[] includeExpressions);
        ObjectQuery<TEntity> LoadAll(params string[] includeProperties);
        ObjectQuery<TEntity> LoadMany(IEnumerable<TKey> IDs);
        ObjectQuery<TEntity> LoadMany(IEnumerable<TKey> IDs, params Expression<Func<TEntity, object>>[] includeExpressions);
        ObjectQuery<TEntity> LoadMany(IEnumerable<TKey> IDs, params string[] includeProperties);
    }
}
