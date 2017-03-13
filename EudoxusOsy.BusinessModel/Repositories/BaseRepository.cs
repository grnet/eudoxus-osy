using Imis.Domain.EF;

namespace EudoxusOsy.BusinessModel
{
    public class BaseRepository<TEntity> : DomainRepository<DBEntities, TEntity, int>
        where TEntity : DomainEntity<DBEntities>
    {
        public BaseRepository() : base() { }

        public BaseRepository(Imis.Domain.IUnitOfWork uow) : base(uow) {
            this.GetCurrentObjectContext().CommandTimeout = 0;
        }
    }
}
