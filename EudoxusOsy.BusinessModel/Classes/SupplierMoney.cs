using Imis.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EudoxusOsy.BusinessModel
{
    public class SupplierMoney
    {
        private IUnitOfWork _uow;
        private ICatalogRepository _catalogRepository;
        private ISupplierRepository _supplierRepository;
        private ICatalogGroupRepository _catalogGroupRepository;
        private ISupplierPhaseRepository _supplierPhaseRepository;
        private ISupplierAmountDiffRepository _supplierAmountDiffRepository;

        public SupplierMoney(IUnitOfWork uow, ICatalogRepository catalogRepository,
            ISupplierRepository supplierRepository, ICatalogGroupRepository catalogGroupRepository,
            ISupplierPhaseRepository supplierPhaseRepository, ISupplierAmountDiffRepository supplierAmountDiffRepository)
        {
            _uow = uow;
            _catalogRepository = catalogRepository;
            _supplierRepository = supplierRepository;
            _catalogGroupRepository = catalogGroupRepository;
            _supplierPhaseRepository = supplierPhaseRepository;
            _supplierAmountDiffRepository = supplierAmountDiffRepository;
        }

        /// <summary>
        /// Set the money for the selected Phase
        /// </summary>
        /// <param name="parameters"></param>        
        public bool SetMoney(SetMoneyParameters parameters)
        {
            decimal totalInsertedMoney = 0m;
            decimal insertedMoney = parameters.PhaseAmount;
            decimal insertedMoneyLimit = parameters.AmountLimit;
            decimal totalOwedMoney = 0m;

            /** 
                Get all the active Suppliers with catalogs, for the Phase
            */
            var suppliers = _supplierRepository.GetAllActive(x => x.SupplierIBANs);

            List<SupplierInfo> remaingSuppliers = new List<SupplierInfo>();
            List<SupplierInfo> clearedOffSuppliersInfo = new List<SupplierInfo>();
            List<TempAmountDiff> supplierAmountDiffs = new List<TempAmountDiff>();

            /**
             * Fix all the Amount Diffs from previous money distribution
             */
            #region [ Fix all the Amount Diffs from previous money distribution ]

            // Use extra logic only if EnableMoneyDiffRecalculation app setting is enabled
            if (Config.EnableMoneyDiffRecalculation == true)
            {
                _supplierAmountDiffRepository.RefreshAmountDiffs(parameters.SelectedPhaseID);
                supplierAmountDiffs = _supplierAmountDiffRepository.LoadAll().ToList();
                var totalDiffAmount = supplierAmountDiffs.Sum(x => x.AmountDiff);
                insertedMoney = insertedMoney + totalDiffAmount.Value;
            }

            #endregion


            CalculateDebtToTheSupplier(parameters.SelectedPhaseID, suppliers,
                remaingSuppliers, clearedOffSuppliersInfo, supplierAmountDiffs);

            // totalDebt is the sum of the owedMoney to all suppliers
            if (!CalculateTotalOwedMoney(remaingSuppliers, insertedMoneyLimit, ref totalOwedMoney,
                ref insertedMoney, ref totalInsertedMoney))
            {
                return false;
            }

            // If all the suppliers are under the money limit then the totalDebt will evaluate to zero, but we
            // still need to update the suppliers_phases table. However we do not need to run the algorithm.
            if (totalOwedMoney != 0)
            {
                DistributeRemainingTotalOwedMoney(totalOwedMoney, remaingSuppliers, insertedMoney);
            }

            var newSupplierPhases = CreateSupplierPhasesForSuppliers(parameters.SelectedPhaseID,
                parameters.Username, remaingSuppliers.Concat(clearedOffSuppliersInfo).ToList());

            UpdatePhaseAmount(parameters, Math.Round((double)newSupplierPhases.Sum(x => x.TotalDebt), 4));
            _uow.Commit();
            EudoxusOsyCacheManager<Phase>.Current.Refresh();

            return true;
        }

        public void CalculateDebtToTheSupplier(int selectedPhaseId, IList<Supplier> suppliers,
            List<SupplierInfo> remainingSuppliersInfo, List<SupplierInfo> clearedOffSuppliersInfo, List<TempAmountDiff> supplierAmountDiffs)
        {
            foreach (var supplier in suppliers)
            {
                /**
                        calculate the debt to the supplier
                    */
                var totalDebtFromSupplierCatalogs = supplier.GetSupplierMoneyDebt(selectedPhaseId);
                var supplierGroups = _catalogGroupRepository.FindBySupplierIDAndPhaseIDWithCatalogs(supplier.ID, selectedPhaseId);

                decimal supplierAssignedMoney = 0m;

                // We have to subtract the diff amount when calculating the 
                var currentSupplierAmount = supplierAmountDiffs.SingleOrDefault(x => x.SupplierID == supplier.ID);

                decimal supplierDiff = 0;
                if (currentSupplierAmount != null)
                {
                    supplierDiff = currentSupplierAmount.AmountDiff.Value;
                }

                if (totalDebtFromSupplierCatalogs < 0)
                {
                    totalDebtFromSupplierCatalogs = 0;
                }

                var supplierInfo = new SupplierInfo()
                {
                    ID = supplier.ID,
                    KpsID = supplier.SupplierKpsID,
                    TotalDebtFromSupplierCatalogs = totalDebtFromSupplierCatalogs ?? 0m,
                    SupplierType = supplier.SupplierType,
                    HasLogisticsBooks = supplier.HasLogisticBooks,
                    Groups = supplierGroups,
                    IBANID = supplier.GetLatestIBANID()
                };

                var currentSupplierPhase = _supplierPhaseRepository.GetSupplierPhase(supplier.ID, selectedPhaseId);

                if (currentSupplierPhase == null)
                {
                    supplierAssignedMoney = 0m;
                    supplierInfo.AssignedMoney = 0m;
                }
                else
                {
                    supplierInfo.AssignedMoney = (decimal)currentSupplierPhase.TotalDebt - supplierDiff;
                    supplierAssignedMoney = (decimal)currentSupplierPhase.TotalDebt - supplierDiff;
                }


                ComputeSupplierPaidMoneyAndTheRemainingMoney(supplierAssignedMoney, supplierInfo, remainingSuppliersInfo,
                    clearedOffSuppliersInfo);
            }
        }

        public List<SupplierPhase> CreateSupplierPhasesForSuppliers(int selectedPhaseId, string username,
            List<SupplierInfo> suppliersInfo)
        {
            List<SupplierPhase> newSupplierPhases = new List<SupplierPhase>();

            foreach (var supplierInfo in suppliersInfo)
            {
                var supplierPhase = _supplierPhaseRepository.GetSupplierPhase(supplierInfo.ID, selectedPhaseId);
                if (supplierPhase == null)
                {
                    var newSupplierPhase = new SupplierPhase()
                    {
                        PhaseID = selectedPhaseId,
                        SupplierID = supplierInfo.ID,
                        TotalDebt = Math.Round((double)supplierInfo.AssignedMoney, 4),
                        CreatedAt = DateTime.Today,
                        CreatedBy = username,
                        IsActive = true
                    };

                    newSupplierPhases.Add(newSupplierPhase);
                    _uow.MarkAsNew(newSupplierPhase);
                }
                else
                {
                    supplierPhase.TotalDebt = Math.Round((double)supplierInfo.AssignedMoney, 4);
                    supplierPhase.UpdatedBy = username;
                    supplierPhase.UpdatedAt = DateTime.Today;
                    newSupplierPhases.Add(supplierPhase);
                }

                if (supplierInfo.SupplierType == enSupplierType.SelfPublisher &&
                    (!supplierInfo.HasLogisticsBooks.HasValue || supplierInfo.HasLogisticsBooks == false))
                {
                    SelfPublisherOrNoLogisticBooks(selectedPhaseId, username, supplierInfo);
                }
            }
            return newSupplierPhases;
        }

        public void SelfPublisherOrNoLogisticBooks(int selectedPhaseID, string username, SupplierInfo supplierInfo)
        {
            CatalogGroupHelper.GroupCatalogsByInstitution(supplierInfo.ID, supplierInfo.IBANID, selectedPhaseID, _uow);
            var remaining = supplierInfo.AssignedMoney - supplierInfo.RemainingMoney;

            if (remaining < 0)
            {
                return;
            }

            foreach (var group in supplierInfo.Groups)
            {
                if (group.State == enCatalogGroupState.New)
                {
                    var groupTotal = group.Catalogs.Sum(x => x.Amount);
                    if (groupTotal < remaining)
                    {
                        remaining -= (decimal)groupTotal;
                        group.State = enCatalogGroupState.Approved;
                        group.UpdatedAt = DateTime.Today;
                        group.UpdatedBy = username;
                        group.Comments = "Αυτόματη δέσμευση";
                    }
                    if (remaining < 0)
                    {
                        break;
                    }
                }
            }
        }

        public void DistributeRemainingTotalOwedMoney(decimal remainingOwedMoney, List<SupplierInfo> suppliersInfo, decimal availableMoney)
        {
            foreach (SupplierInfo si in suppliersInfo)
            {
                si.Percent = (double)si.OwedMoney / (double)remainingOwedMoney;
                si.Difference = (decimal)si.Percent * (decimal)availableMoney;

                if (si.Difference > si.OwedMoney)
                {
                    si.Difference = si.OwedMoney;
                }
                si.AssignedMoney += si.Difference;
            }
        }

        public bool CalculateTotalOwedMoney(List<SupplierInfo> suppliersInfo, decimal insertedMoneyLimit, ref decimal totalOwedMoney,
            ref decimal insertedMoney, ref decimal totalInsertedMoney)
        {
            totalOwedMoney = 0;

            foreach (SupplierInfo si in suppliersInfo)
            {
                if (si.OwedMoney <= insertedMoneyLimit)
                {
                    si.AssignedMoney += si.OwedMoney;
                    insertedMoney -= si.OwedMoney;
                    if (insertedMoney < 0)
                    {
                        return false;
                    }
                    si.OwedMoney = 0;
                    totalInsertedMoney += si.OwedMoney;
                }
                else
                {
                    si.AssignedMoney += insertedMoneyLimit;
                    insertedMoney -= insertedMoneyLimit;
                    if (insertedMoney < 0)
                    {
                        return false;
                    }
                    si.OwedMoney -= insertedMoneyLimit;
                    totalInsertedMoney += insertedMoneyLimit;
                }
                totalOwedMoney += si.OwedMoney;
            }
            return true;
        }

        public void ComputeSupplierPaidMoneyAndTheRemainingMoney(decimal supplierAssignedMoney,
            SupplierInfo supplierInfo, List<SupplierInfo> suppliersInfo, List<SupplierInfo> zeroSuppliersInfo)
        {
            decimal supplierPaidCatalogsMoney = 0m;
            decimal supplierRemainingCatalogsMoney = 0m;

            //Compute the supplier paid money and the remaining money

            foreach (var group in supplierInfo.Groups)
            {
                if (group == null) continue;
                if (group.State == enCatalogGroupState.Sent && group.IsActive)
                {
                    supplierPaidCatalogsMoney += (decimal)group.Catalogs
                        .Where(x => (x.State == enCatalogState.FromMove || x.State == enCatalogState.Normal))
                        .Sum(x => x.Amount);
                }
                else if (group.State != enCatalogGroupState.New && group.IsActive)
                {
                    supplierRemainingCatalogsMoney += (decimal)group.Catalogs
                        .Where(x => x.Status == enCatalogStatus.Active &&
                                    (x.State == enCatalogState.FromMove || x.State == enCatalogState.Normal))
                        .Sum(x => x.Amount);
                }
            }

            /** if the money that was paid to the supplier, is less than the money assigned the algorithm must take into account
                     the money that was assigned. Just because a supplier didn't use his assigned money doesn't mean that he
                     should receive more money, next time tha algorithm runs.
                     If we want the algorithm not to take into account the money that have already been paid and always calculate
                     using the assigned money we have to remove the following if
                    */
            //if (supplierPaidMoney < supplierTotalDebt)
            supplierPaidCatalogsMoney = supplierAssignedMoney;

            if (supplierInfo != null)
            {
                supplierInfo.PaidMoney = supplierPaidCatalogsMoney;
                supplierInfo.RemainingMoney = supplierRemainingCatalogsMoney;

                if (supplierInfo.TotalDebtFromSupplierCatalogs.Value - supplierPaidCatalogsMoney < 0)
                {
                    supplierInfo.OwedMoney = 0;
                }
                else
                {
                    supplierInfo.OwedMoney =
                        supplierInfo.TotalDebtFromSupplierCatalogs.Value -
                        supplierPaidCatalogsMoney; // OwedMoney το ποσό που απομένει και δεν έχει ακόμη ανατεθεί στον εκδότη
                }

                if (supplierInfo.OwedMoney > 0)
                {
                    suppliersInfo.Add(supplierInfo);
                }
                else
                {
                    zeroSuppliersInfo.Add(supplierInfo);
                }
            }
        }

        public void UpdatePhaseAmount(SetMoneyParameters parameters, double phaseAmount)
        {
            var currentPhase = new PhaseRepository(_uow).Load(parameters.SelectedPhaseID);
            currentPhase.PhaseAmount = phaseAmount;
            currentPhase.UpdatedAt = DateTime.Today;
            currentPhase.UpdatedBy = parameters.Username;
        }

    }
}
