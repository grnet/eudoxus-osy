using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imis.Domain.EF;

namespace EudoxusOsy.BusinessModel
{
    public class BookPricesGridVRepository : DomainRepository<DBEntities, BookPricesGridV, int>, IBookPricesGridVRepository
    {
        #region [ Base .ctors ]

        public BookPricesGridVRepository() : base() { }

        public BookPricesGridVRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public BookPricesGridV FindByBookPriceID(int bookPriceId)
        {
            return BaseQuery.FirstOrDefault(x => x.BookPriceID == bookPriceId);
        }

        public BookPricesGridV AdditionalApprovalPending(int year, int bookId)
        {
            return BaseQuery.FirstOrDefault(x => x.BookID == bookId && x.ChangeYear != year);
        }
    }
}
