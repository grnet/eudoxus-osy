using Imis.Domain.EF;

namespace EudoxusOsy.BusinessModel
{
    public class EudoxusOsyCacheManager<TEntity> : DomainCacheManager<DBEntities, TEntity, int>
        where TEntity : DomainEntity<DBEntities>
    {
        protected EudoxusOsyCacheManager()
        {
            if (s_CacheStorage.Values.Count == 0)
                Fill();
        }

        #region Thread-safe, lazy Singleton

        public static EudoxusOsyCacheManager<TEntity> Current
        {
            get { return Nested._cacheManager; }
        }

        /// <summary>
        /// Assists with ensuring thread-safe, lazy singleton
        /// </summary>
        private sealed class Nested
        {
            static Nested() { }
            internal static readonly EudoxusOsyCacheManager<TEntity> _cacheManager = new EudoxusOsyCacheManager<TEntity>();
        }

        #endregion
    }
}
