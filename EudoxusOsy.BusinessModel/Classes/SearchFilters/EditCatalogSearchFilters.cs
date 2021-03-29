using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public class EditCatalogSearchFilters : BaseSearchFilters<EditCatalogsGridV>
    {
        public int? ID { get; set; }
        public int? GroupID { get; set; }
        public int? PhaseID { get; set; }
        public int? BookKpsID { get; set; }
        public int? SupplierKpsID { get; set; }
        public int? SecretaryKpsID { get; set; }
        public DateTime? CreatedAt { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public int? BookCount { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? Amount { get; set; }
        public bool? IsInGroup { get; set; }
        public bool? IsLocked { get; set; }
        public int? State { get; set; }
        public int? GroupState { get; set; }
        public int? InstitutionID { get; set; }
        public int? DepartmentID { get; set; }        
        public enCatalogType? CatalogType { get; set; }
        public enCatalogStatus? CatalogStatus { get; set; }
        public int? IsForLibrary { get; set; }

        public override Imis.Domain.EF.Search.Criteria<EditCatalogsGridV> GetExpression()
        {
            var expression = Imis.Domain.EF.Search.Criteria<EditCatalogsGridV>.Empty;

            if (ID.HasValue)
                expression = expression.Where(x => x.ID, ID);

            if (GroupID.HasValue)
                expression = expression.Where(x => x.GroupID, GroupID);

            if (PhaseID.HasValue)
                expression = expression.Where(x => x.PhaseID, PhaseID);

            if (BookKpsID.HasValue)
                expression = expression.Where(x => x.BookKpsID, BookKpsID);

            if (SupplierKpsID.HasValue)
                expression = expression.Where(x => x.SupplierKpsID, SupplierKpsID);

            if (SecretaryKpsID.HasValue)
            {
                var orExpression = Imis.Domain.EF.Search.Criteria<EditCatalogsGridV>.Empty;
                orExpression = orExpression.Where(x => x.SecretaryKpsID, SecretaryKpsID).Or(x => x.LibraryKpsID, SecretaryKpsID);
                expression = expression.And(orExpression);
            }

            if (CreatedAt.HasValue)
            {
                expression = expression.Where(x => x.CreatedAt, CreatedAt, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThanEquals)
                                        .Where(x => x.CreatedAt, CreatedAt.Value.AddDays(1), Imis.Domain.EF.Search.enCriteriaOperator.LessThan);
            }

            if (DiscountPercentage.HasValue)
                expression = expression.Where(x => x.DiscountPercentage, 1 - (DiscountPercentage * 0.01m));

            if (BookCount.HasValue)
                expression = expression.Where(x => x.BookCount, BookCount);

            if (Percentage.HasValue)
                expression = expression.Where(x => x.Percentage, Percentage);

            if (Amount.HasValue)
                expression = expression.Where(x => x.Amount, Amount);

            if (IsInGroup.HasValue)
            {
                if (IsInGroup == true)
                {
                    expression = expression.IsNotNull(x => x.GroupID);
                }
                else
                {
                    expression = expression.IsNull(x => x.GroupID);
                }
            }
            
            if (IsLocked.HasValue)
            {
                if (IsLocked.Value)
                {
                    var orExpression = Imis.Domain.EF.Search.Criteria<EditCatalogsGridV>.Empty;
                    orExpression = orExpression.Where(x => x.HasPendingPriceVerification, IsLocked).Or(x => x.HasUnexpectedPriceChange, IsLocked);
                    expression = expression.And(orExpression);                
                }
                else
                {
                    expression = expression.Where(x => x.HasPendingPriceVerification, false);
                    expression = expression.Where(x => x.HasUnexpectedPriceChange, false);
                }
                
            }

            if (State.HasValue)
                expression = expression.Where(x => x.State, (int)State);

            if (GroupState.HasValue)
                expression = expression.Where(x => x.GroupState, (int)GroupState);
                     
            if (CatalogType.HasValue)
            {
                expression = expression.Where(x => x.CatalogType, (int)CatalogType);
            }

            if (CatalogStatus.HasValue)
            {
                expression = expression.Where(x => x.Status, (int)CatalogStatus);
            }

            if (IsForLibrary.HasValue)
            {
                if (IsForLibrary == 1)
                {
                    expression = expression.IsNotNull(x => x.LibraryKpsID);
                }
                else if (IsForLibrary == 2)
                {
                    expression = expression.IsNotNull(x => x.SecretaryKpsID);
                }
            }

            return string.IsNullOrEmpty(expression.CommandText) ? null : expression;
        }
    }
}
