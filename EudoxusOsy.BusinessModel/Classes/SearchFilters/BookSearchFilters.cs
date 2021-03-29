using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;

namespace EudoxusOsy.BusinessModel
{
    public class BookSearchFilters : BaseSearchFilters<Book>
    {
        public int? BookKpsID { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public bool? IsActive { get; set; }
        public bool? HasBookPriceChanges { get; set; }

        public override Imis.Domain.EF.Search.Criteria<Book> GetExpression()
        {
            var expression = Imis.Domain.EF.Search.Criteria<Book>.Empty;

            if (BookKpsID.HasValue)
                expression = expression.Where(x => x.BookKpsID, BookKpsID);

            if (!string.IsNullOrEmpty(Author))
                expression = expression.Where(x => x.Author, Author, Imis.Domain.EF.Search.enCriteriaOperator.Like);

            if (!string.IsNullOrEmpty(Publisher))
                expression = expression.Where(x => x.Publisher, Publisher, Imis.Domain.EF.Search.enCriteriaOperator.Like);

            if (!string.IsNullOrEmpty(ISBN))
                expression = expression.Where(x => x.ISBN, ISBN);

            if (!string.IsNullOrEmpty(Title))
                expression = expression.Where(x => x.Title, Title, Imis.Domain.EF.Search.enCriteriaOperator.Like);

            if (IsActive.HasValue)
            {
                expression = expression.Where(x => x.IsActive, IsActive);
            }

            if (HasBookPriceChanges == true)
            {
                expression = expression.Where("it.ID in (Select VALUE DISTINCT it2.BookID from BookPriceChanges as it2 where it2.BookID = it.ID)");
            }
            else if (HasBookPriceChanges == false)
            {
                expression = expression.Where("it.ID not in (Select VALUE DISTINCT it2.BookID from BookPriceChanges as it2 where it2.BookID = it.ID)");
            }

            return string.IsNullOrEmpty(expression.CommandText) ? null : expression;
        }
    }
}