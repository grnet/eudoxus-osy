using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imis.Domain.EF;

namespace EudoxusOsy.BusinessModel
{
    public class BookPriceChangeRepository : DomainRepository<DBEntities, BookPriceChange, int>
    {
        #region [ Base .ctors ]

        public BookPriceChangeRepository() : base() { }

        public BookPriceChangeRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public bool AlreadyExists(int bookID, int year, decimal? price, decimal suggestedPrice, bool priceChecked)
        {
            return BaseQuery.Any(x => x.BookID == bookID && x.Year == year && x.Price == price && x.SuggestedPrice == suggestedPrice && x.PriceChecked == priceChecked);
        }
    }
}
