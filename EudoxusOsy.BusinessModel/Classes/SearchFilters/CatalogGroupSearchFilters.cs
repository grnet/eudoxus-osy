using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;

namespace EudoxusOsy.BusinessModel
{
    public class CatalogGroupSearchFilters : BaseSearchFilters<CatalogGroup>
    {
        public int? GroupID { get; set; }

        public override Imis.Domain.EF.Search.Criteria<CatalogGroup> GetExpression()
        {
            var expression = Imis.Domain.EF.Search.Criteria<CatalogGroup>.Empty;

            if (GroupID.HasValue)
                expression = expression.Where(x => x.ID, GroupID);            

            return string.IsNullOrEmpty(expression.CommandText) ? null : expression;
        }
    }
}