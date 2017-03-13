using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;

namespace EudoxusOsy.BusinessModel
{
    public class CatalogSearchFilters : BaseSearchFilters<Catalog>
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
        public int? State { get; set; }
        public int? GroupState { get; set; }
        public int? InstitutionID { get; set; }
        public int? DepartmentID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public enCatalogType? CatalogType { get; set; }
        public enCatalogStatus? CatalogStatus { get; set; }
        public int? IsForLibrary { get; set; }

        public override Imis.Domain.EF.Search.Criteria<Catalog> GetExpression()
        {
            var expression = Imis.Domain.EF.Search.Criteria<Catalog>.Empty;

            if (ID.HasValue)
                expression = expression.Where(x => x.ID, ID);

            if (GroupID.HasValue)
                expression = expression.Where(x => x.GroupID, GroupID);

            if (PhaseID.HasValue)            
                expression = expression.Where(x => x.PhaseID, PhaseID);            

            if (BookKpsID.HasValue)
                expression = expression.Where(x => x.Book.BookKpsID, BookKpsID);            

            if (SupplierKpsID.HasValue)            
                expression = expression.Where(x => x.Supplier.SupplierKpsID, SupplierKpsID);

            if (SecretaryKpsID.HasValue)
            {
                var orExpression = Imis.Domain.EF.Search.Criteria<Catalog>.Empty;
                orExpression = orExpression.Where(x => x.Department.SecretaryKpsID, SecretaryKpsID).Or(x => x.Department.LibraryKpsID, SecretaryKpsID);
                expression = expression.And(orExpression);
            }

            if (CreatedAt.HasValue)
            {
                expression = expression.Where(x => x.CreatedAt, CreatedAt, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThanEquals)
                                        .Where(x => x.CreatedAt, CreatedAt.Value.AddDays(1), Imis.Domain.EF.Search.enCriteriaOperator.LessThan);
            }

            if (DiscountPercentage.HasValue)            
                expression = expression.Where(x => x.Discount.DiscountPercentage, 1 - (DiscountPercentage * 0.01m));

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

            if (State.HasValue)            
                expression = expression.Where(x => x.StateInt, (int)State);            

            if (GroupState.HasValue)            
                expression = expression.Where(x => x.CatalogGroup.StateInt, (int)GroupState);

            if (InstitutionID.HasValue)
                expression = expression.Where(x => x.Department.InstitutionID, InstitutionID);

            if (DepartmentID.HasValue)
                expression = expression.Where(x => x.DepartmentID, DepartmentID);

            if (!string.IsNullOrEmpty(Title))
                expression = expression.Where(x => x.Book.Title, Title, Imis.Domain.EF.Search.enCriteriaOperator.Like);
            
            if (!string.IsNullOrEmpty(Author))            
                expression = expression.Where(x => x.Book.Author, Author, Imis.Domain.EF.Search.enCriteriaOperator.Like);

            if(CatalogType.HasValue)
            {
                expression = expression.Where(x => x.CatalogTypeInt, (int)CatalogType);
            }

            if(CatalogStatus.HasValue)
            {
                expression = expression.Where(x => x.StatusInt, (int)CatalogStatus);
            }

            if(IsForLibrary.HasValue)
            {
                if(IsForLibrary == 1)
                {
                    expression = expression.IsNotNull(x => x.Department.LibraryKpsID);
                }
                else if (IsForLibrary == 2)
                {
                    expression = expression.IsNotNull(x => x.Department.SecretaryKpsID);
                }
            }

            return string.IsNullOrEmpty(expression.CommandText) ? null : expression;
        }
    }
}