using EudoxusOsy.BusinessModel.Interfaces;
using Imis.Domain.EF;

namespace EudoxusOsy.BusinessModel
{
    public class EditCatalogsGridVRepository : DomainRepository<DBEntities, EditCatalogsGridV, int>, IEditCatalogsGridVRepository
    {
        #region [ Base .ctors ]

        public EditCatalogsGridVRepository() : base() { }

        public EditCatalogsGridVRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion  
    }
}