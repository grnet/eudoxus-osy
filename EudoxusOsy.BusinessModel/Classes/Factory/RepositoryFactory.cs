using EudoxusOsy.BusinessModel.Interfaces;
using Imis.Domain;
using Imis.Domain.EF;
using System;

namespace EudoxusOsy.BusinessModel
{
    public class BMRepositoryFactory : IRepositoryFactory
    {
        public TRepository GetRepositoryInstance<T, TRepository>(IUnitOfWork uow)
          where TRepository : IBaseRepository<T, int>
          where T : DomainEntity<DBEntities>

        {
            if (typeof(TRepository).IsInterface)
            {
                if (typeof(TRepository) is IBookRepository)
                    return (TRepository)Activator.CreateInstance(typeof(BookRepository), new object[] { uow });
                if (typeof(TRepository) is ICatalogRepository)
                    return (TRepository)Activator.CreateInstance(typeof(CatalogRepository), new object[] { uow });
                if (typeof(TRepository) is IBookPriceRepository)
                    return (TRepository)Activator.CreateInstance(typeof(BookPriceRepository), new object[] { uow });
                if (typeof(TRepository) is ISupplierRepository)
                    return (TRepository)Activator.CreateInstance(typeof(SupplierRepository), new object[] { uow });
                if (typeof(TRepository) is IBookSupplierRepository)
                    return (TRepository)Activator.CreateInstance(typeof(BookSupplierRepository), new object[] { uow });
                if (typeof(TRepository) is IDiscountRepository)
                    return (TRepository)Activator.CreateInstance(typeof(DiscountRepository), new object[] { uow });
                if (typeof(TRepository) is ICatalogGroupRepository)
                    return (TRepository)Activator.CreateInstance(typeof(CatalogRepository), new object[] { uow });
                if (typeof(TRepository) is IPaymentOrderRepository)
                    return (TRepository)Activator.CreateInstance(typeof(PaymentOrderRepository), new object[] { uow });
            }

            return (TRepository)Activator.CreateInstance(typeof(TRepository), new object[] { uow });
        }
    }
}
