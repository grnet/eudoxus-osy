using System;
using System.Collections.Generic;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public class AuditReceiptRepository : DomainRepository<DBEntities, AuditReceipt, int>
    {
        #region [ Base .ctors ]

        public AuditReceiptRepository() : base() { }

        public AuditReceiptRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        public List<AuditReceipt> FindAuditsToUpdate(Criteria<AuditReceipt> criteria, out int totalAuditRecords)
        {
            var orExpression1 = Imis.Domain.EF.Search.Criteria<AuditReceipt>.Empty;
            orExpression1 = criteria.Expression.IsNull(x => x.StatusInt);
            var orExpression2 = Imis.Domain.EF.Search.Criteria<AuditReceipt>.Empty;
            orExpression2 = criteria.Expression.Where(x => x.StatusInt, (int)enAuditReceiptStatus.Failed);
            var orExpression = orExpression1.Or(orExpression2);
            criteria.Expression = criteria.Expression.And(orExpression);

            int totalCount = 0;
            var results = FindWithCriteria(criteria, out totalCount);
            totalAuditRecords = totalCount;

            return results.ToList();
        }

        public List<ReceiptsXmlComparisonResult> GetKpsXmlOnly(int? phaseID = null)
        {
            var ctx = GetCurrentObjectContext();
            return ctx.CompareReceipts(phaseID, (int)enCompareReceipts.XmlOnly).ToList();
        }

        public List<ReceiptsXmlComparisonResult> GetOsyReceiptsOnly(int? phaseID = null)
        {
            var ctx = GetCurrentObjectContext();
            return ctx.CompareReceipts(phaseID, (int)enCompareReceipts.ReceiptsOnly).ToList();
        }

        public void InsertCorrectedFiles(string path, string inputDataFileName, int? mode)
        {
            var ctx = GetCurrentObjectContext();
            ctx.InsertCorrectedFiles(path, inputDataFileName, mode);
        }

        public void ComplementReceipts(int? currentPhaseID)
        {
            var ctx = GetCurrentObjectContext();
            ctx.ComplementReceipts(currentPhaseID);
        }

        #endregion
    }
}
