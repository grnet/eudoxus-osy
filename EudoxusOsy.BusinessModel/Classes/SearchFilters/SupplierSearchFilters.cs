using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;

namespace EudoxusOsy.BusinessModel
{
    public class SupplierSearchFilters : BaseSearchFilters<Supplier>
    {
        public int? SupplierID { get; set; }
        public int? SupplierKpsID { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAFM { get; set; }
        public enSupplierType? SupplierType { get; set; }
        public enSupplierStatus? SupplierStatus { get; set; }

        public override Imis.Domain.EF.Search.Criteria<Supplier> GetExpression()
        {
            var expression = Imis.Domain.EF.Search.Criteria<Supplier>.Empty;

            if (SupplierKpsID.HasValue)
                expression = expression.Where(x => x.SupplierKpsID, SupplierKpsID);

            if (SupplierID.HasValue)
                expression = expression.Where(x => x.ID, SupplierID);

            if (!string.IsNullOrEmpty(SupplierName))
                expression = expression.Where(x => x.Name, SupplierName, Imis.Domain.EF.Search.enCriteriaOperator.Like);

            if (!string.IsNullOrEmpty(SupplierAFM))
                expression = expression.Where(x => x.AFM, SupplierAFM);

            if (SupplierType.HasValue)
                expression = expression.Where(x => x.SupplierType, SupplierType);
            
            if (SupplierStatus.HasValue)
                expression = expression.Where(x => x.Status, SupplierStatus);

            return string.IsNullOrEmpty(expression.CommandText) ? null : expression;
        }
    }
}