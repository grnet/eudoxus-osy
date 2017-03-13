using System.Data.Objects.DataClasses;

namespace EudoxusOsy.BusinessModel
{
    public abstract class BaseSearchFilters<TEntity> where TEntity : EntityObject
    {
        public virtual Imis.Domain.EF.Search.Criteria<TEntity> GetExpression()
        {
            return Imis.Domain.EF.Search.Criteria<TEntity>.Empty;
        }
    }
}