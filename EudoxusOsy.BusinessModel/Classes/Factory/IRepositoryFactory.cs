using Imis.Domain;
using Imis.Domain.EF;
using System.Data.Objects.DataClasses;

namespace EudoxusOsy.BusinessModel
{
    public interface IRepositoryFactory
    {
        TRepository GetRepositoryInstance<T, TRepository>(IUnitOfWork uow) 
            where TRepository: IBaseRepository<T, int>
            where T : DomainEntity<DBEntities>;
    }
}