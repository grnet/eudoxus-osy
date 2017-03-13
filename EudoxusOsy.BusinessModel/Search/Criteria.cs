using System.Data.Objects.DataClasses;

namespace EudoxusOsy.BusinessModel
{
    public class Criteria<T> : Imis.Domain.EF.DomainCriteria<T> where T : EntityObject
    {
    }
}