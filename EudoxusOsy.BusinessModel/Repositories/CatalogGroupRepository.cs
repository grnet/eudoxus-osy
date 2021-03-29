using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace EudoxusOsy.BusinessModel
{
    public class CatalogGroupRepository : DomainRepository<DBEntities, CatalogGroup, int>, ICatalogGroupRepository
    {
        #region [ Base .ctors ]

        public CatalogGroupRepository() : base() { }

        public CatalogGroupRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public CatalogGroup FindByID(int id)
        {
            return BaseQuery
                    .FirstOrDefault(x => x.ID == id);
        }

        public List<CatalogGroup> FindByPhaseID(int phaseID)
        {
            return BaseQuery
                    .Where(x => x.PhaseID == phaseID).ToList();
        }

        public List<CatalogGroup> FindBySupplierID(int supplierID)
        {
            return BaseQuery
                    .Where(x => x.SupplierID == supplierID).ToList();
        }

        public CatalogGroup FindByIDandSupplierID(int id, int supplierID)
        {
            return BaseQuery
                    .FirstOrDefault(x => x.ID == id && x.SupplierID == supplierID);
        }

        public List<CatalogGroup> FindBySupplierIDAndPhaseIDWithCatalogs(int supplierID, int phaseID)
        {

            var query = BaseQuery.Include(x => x.Catalogs);
            return query
                    .Where(x => x.SupplierID == supplierID && x.PhaseID == phaseID && x.IsActive).ToList();
        }

        public List<CatalogGroup> FindBySupplierIBANID(int supplierIBANID)
        {
            return BaseQuery
                    .Where(x => x.SupplierIBANID == supplierIBANID).ToList();
        }

        public List<CatalogGroup> FindByInstitutiooID(int institutionID)
        {
            return BaseQuery
                    .Where(x => x.InstitutionID == institutionID).ToList();
        }

        public List<CatalogGroup> FindByBankID(int bankID)
        {
            return BaseQuery
                    .Where(x => x.BankID == bankID).ToList();
        }

        public CatalogGroupInfo GetByID(int groupID)
        {
            var query = BaseQuery
                        .Include(x => x.Deduction)
                        .Where(x => x.ID == groupID && x.IsActive);

            return query.Select(x => new CatalogGroupInfo()
            {
                ID = x.ID,
                SupplierID = x.SupplierID,
                InstitutionID = x.InstitutionID,
                GroupStateInt = x.StateInt,
                ContainsInActiveBooks = x.Catalogs.Any(y => !y.IsBookActive),
                HasPendingPriceVerification = x.PhaseID >= 13 && x.Catalogs.Any(c => c.HasPendingPriceVerification),
                HasUnexpectedPriceChange = x.PhaseID >= 13 && x.Catalogs.Any(c => c.HasUnexpectedPriceChange),
                IsLocked = x.IsLocked,
                CatalogCount = x.Catalogs.Where(y => y.StateInt == (int)enCatalogState.Normal || y.StateInt == (int)enCatalogState.FromMove).Count(),
                TotalAmount = x.Catalogs.Where(y => y.StateInt == (int)enCatalogState.Normal || y.StateInt == (int)enCatalogState.FromMove).Sum(y => y.Amount),
                InvoiceCount = x.Invoices.Where(y => y.IsActive).Count(),
                InvoiceSum = x.Invoices.Where(y => y.IsActive).Sum(y => y.InvoiceValue),
                Deduction = x.Deduction,
                Vat = x.Vat,
                IsTransfered = x.IsTransfered,
                TransferedBankID = x.BankID
            })
            .FirstOrDefault();
        }

        public IList<CatalogGroupInfo> GetBySupplierAndPhase(int supplierID, int phaseID, int? groupID, int startRowIndex, int maximumRows, string sortExpression, out int recordCount)
        {
            var query = groupID.HasValue
                            ? BaseQuery.Include(x => x.Deduction).Include(x => x.CatalogGroupLogs).Where(x => x.SupplierID == supplierID && x.PhaseID == phaseID && x.ID == groupID && x.IsActive)
                            : BaseQuery.Include(x => x.Deduction).Include(x => x.CatalogGroupLogs).Where(x => x.SupplierID == supplierID && x.PhaseID == phaseID && x.IsActive);

            recordCount = query.Count();

            return query.Select(x => new CatalogGroupInfo()
            {
                ID = x.ID,
                InstitutionID = x.InstitutionID,
                GroupStateInt = x.StateInt,
                ContainsInActiveBooks = x.Catalogs.Any(y => !y.IsBookActive),
                HasPendingPriceVerification = x.PhaseID >= 13 && x.Catalogs.Any(c => c.HasPendingPriceVerification),
                HasUnexpectedPriceChange = x.PhaseID >= 13 && x.Catalogs.Any(c => c.HasUnexpectedPriceChange),
                IsLocked = x.IsLocked,
                CatalogCount = x.Catalogs.Where(y => y.StateInt == (int)enCatalogState.Normal || y.StateInt == (int)enCatalogState.FromMove).Count(),
                TotalAmount = x.Catalogs.Where(y => y.StateInt == (int)enCatalogState.Normal || y.StateInt == (int)enCatalogState.FromMove).Sum(y => y.Amount),
                InvoiceCount = x.Invoices.Where(y => y.IsActive).Count(),
                InvoiceSum = x.Invoices.Where(y => y.IsActive).Sum(y => y.InvoiceValue),
                OfficeSlipDate = x.PaymentOrders.FirstOrDefault(y => y.IsActive.Value).OfficeSlipDate,
                Deduction = x.Deduction,
                DeductionVatType = x.Deduction != null ? (enDeductionVatType?)x.Deduction.VatTypeInt : null,
                Vat = x.Vat,
                IsTransfered = x.IsTransfered,
                TransferedBankID = x.BankID//,
                //IsCatalogGroupInvoicePDFAvailable = x.CatalogGroupLogs
            })
                    .OrderBy(sortExpression)
                    .Skip(startRowIndex)
                    .Take(maximumRows)
                    .ToArray<CatalogGroupInfo>();
        }

        public bool BankExistsInCatalogGroup(int bankID)
        {
            return BaseQuery.Any(x => x.BankID == bankID);
        }

        public Deduction FindActiveDeductionForSupplier(int supplierID)
        {
            var deduction = BaseQuery
                            .Where(x => x.SupplierID == supplierID && x.IsActive && x.DeductionID != null)
                            .OrderByDescending(x => x.ID)
                            .Select(x => x.Deduction)
                            .FirstOrDefault();

            return deduction;
        }
    }
}
