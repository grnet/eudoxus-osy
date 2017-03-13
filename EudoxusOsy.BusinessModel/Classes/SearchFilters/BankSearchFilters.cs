using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;

namespace EudoxusOsy.BusinessModel
{
    public class BankSearchFilters : BaseSearchFilters<Bank>
    {
        public string Name { get; set; }
        public bool? IsBank { get; set; }
        public bool? IsActive { get; set; }

        public override Imis.Domain.EF.Search.Criteria<Bank> GetExpression()
        {
            var expression = Imis.Domain.EF.Search.Criteria<Bank>.Empty;

            if (!string.IsNullOrEmpty(Name))
                expression = expression.Where(x => x.Name, Name, Imis.Domain.EF.Search.enCriteriaOperator.Like);

            if (IsBank.HasValue)            
                expression = expression.Where(x => x.IsBank, IsBank.Value);

            if (IsActive.HasValue)
                expression = expression.Where(x => x.IsActive, IsActive.Value);

            return string.IsNullOrEmpty(expression.CommandText) ? null : expression;
        }
    }
}