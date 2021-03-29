using Imis.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EudoxusOsy.BusinessModel
{
    public static class PhaseHelper
    {
        public static List<Phase> GetPhasesOfYear(int year)
        {
            return EudoxusOsyCacheManager<Phase>.Current.GetItems().Where(x => x.Year == year).ToList();
        }
        public static int MaxYear()
        {
            return EudoxusOsyCacheManager<Phase>.Current.GetItems().Where(x => x.IsActive).Max(x => x.Year);
        }
        /// <summary>
        /// Set the money for the selected Phase
        /// </summary>
        /// <param name="phaseAmount"></param>
        /// <param name="amountLimit"></param>
        public static bool SetMoney(decimal phaseAmount, decimal amountLimit, int selectedPhaseID, string username = "sysadmin")
        {
            decimal totalInsertedMoney = 0m;
            decimal insertedMoney = phaseAmount;
            decimal insertedMoneyLimit = amountLimit;
            decimal totalOwedMoney = 0m;
            decimal? totalDiffAmount = 0m;


            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                /** 
                    Get all the active Suppliers for the Phase
                */
                var suppliers = new SupplierRepository(uow).LoadAll(x => x.SupplierIBANs);
                SupplierAmountDiffRepository _supplierAmountDiffRepository = new SupplierAmountDiffRepository();

                List<SupplierInfo> SuppliersInfo = new List<SupplierInfo>();
                List<SupplierInfo> ZeroSuppliersInfo = new List<SupplierInfo>();

                List<TempAmountDiff> supplierAmountDiffs = new List<TempAmountDiff>();

                /**
                 * Fix all the Amount Diffs from previous money distribution
                 */
                #region [ Fix all the Amount Diffs from previous money distribution ]

                // Use extra logic only if EnableMoneyDiffRecalculation app setting is enabled
                if (Config.EnableMoneyDiffRecalculation == true)
                {
                    try
                    {
                        _supplierAmountDiffRepository.RefreshAmountDiffs(selectedPhaseID);
                    }
                    catch
                    {
                        return false;
                    }
                    supplierAmountDiffs = _supplierAmountDiffRepository.LoadAll().ToList();
                    totalDiffAmount = supplierAmountDiffs.Sum(x => x.AmountDiff);
                    insertedMoney = insertedMoney + totalDiffAmount.Value;
                }

                #endregion


                foreach (var supplier in suppliers)
                {                    
                    /**
                        calculate the debt to the supplier
                    */
                    var totalDebtFromSupplierCatalogs = supplier.GetSupplierMoneyDebt(selectedPhaseID);
                    var supplierGroups = new CatalogGroupRepository().FindBySupplierIDAndPhaseIDWithCatalogs(supplier.ID, selectedPhaseID);

                    if (totalDebtFromSupplierCatalogs == 0 && supplier.Status != enSupplierStatus.Active)
                    {
                        continue;
                    }

                    decimal supplierAssignedMoney = 0m;

                    if (totalDebtFromSupplierCatalogs < 0)
                    {
                        totalDebtFromSupplierCatalogs = 0;
                    }

                    // We have to subtract the diff amount when calculating the Supplier phase money (only if the app setting is enabled will it have an impact [supplierAmountDiffs != empty list])
                    var currentSupplierAmount = supplierAmountDiffs.SingleOrDefault(x => x.SupplierID == supplier.ID);

                    decimal supplierDiff = 0;
                    if (currentSupplierAmount != null)
                    {
                        supplierDiff = currentSupplierAmount.AmountDiff.Value;
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

                    var currentSupplierPhase = new SupplierPhaseRepository(uow).GetSupplierPhase(supplier.ID, selectedPhaseID);

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


                    decimal supplierPaidCatalogsMoney = 0m;
                    decimal supplierRemainingCatalogsMoney = 0m;
                    /** 
                        Compute the supplier paid money and the remaining money
                    */
                    foreach (var group in supplierGroups)
                    {
                        if (group.State == enCatalogGroupState.Sent && group.IsActive)
                        {
                            supplierPaidCatalogsMoney += (decimal)group.Catalogs.Where(x => (x.State == enCatalogState.FromMove || x.State == enCatalogState.Normal)).Sum(x => x.Amount);
                        }
                        else if (group.State != enCatalogGroupState.New && group.IsActive)
                        {
                            supplierRemainingCatalogsMoney += (decimal)group.Catalogs.Where(x => x.Status == enCatalogStatus.Active && (x.State == enCatalogState.FromMove || x.State == enCatalogState.Normal)).Sum(x => x.Amount);
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
                    supplierInfo.PaidMoney = supplierPaidCatalogsMoney;
                    supplierInfo.RemainingMoney = supplierRemainingCatalogsMoney;

                    if (supplierInfo.TotalDebtFromSupplierCatalogs.Value - supplierPaidCatalogsMoney < 0)
                    {
                        supplierInfo.OwedMoney = 0;
                    }
                    else
                    {
                        supplierInfo.OwedMoney = supplierInfo.TotalDebtFromSupplierCatalogs.Value - supplierPaidCatalogsMoney; // OwedMoney το ποσό που απομένει και δεν έχει ακόμη ανατεθεί στον εκδότη
                    }

                    if (supplierInfo.OwedMoney > 0)
                    {
                        SuppliersInfo.Add(supplierInfo);
                    }
                    else
                    {
                        ZeroSuppliersInfo.Add(supplierInfo);
                    }
                }

                // totalDebt is the sum of the owedMoney to all suppliers
                totalOwedMoney = 0;

                for (int i = 0; i < SuppliersInfo.Count; i++)
                {
                    if (SuppliersInfo[i].OwedMoney <= insertedMoneyLimit)
                    {
                        SuppliersInfo[i].AssignedMoney += SuppliersInfo[i].OwedMoney;
                        insertedMoney -= SuppliersInfo[i].OwedMoney;
                        if (insertedMoney < 0)
                        {
                            return false;
                        }
                        SuppliersInfo[i].OwedMoney = 0;
                        totalInsertedMoney += SuppliersInfo[i].OwedMoney;
                    }
                    else
                    {
                        SuppliersInfo[i].AssignedMoney += insertedMoneyLimit;
                        insertedMoney -= insertedMoneyLimit;
                        if (insertedMoney < 0)
                        {
                            return false;
                        }
                        SuppliersInfo[i].OwedMoney -= insertedMoneyLimit;
                        totalInsertedMoney += insertedMoneyLimit;
                    }
                    totalOwedMoney += SuppliersInfo[i].OwedMoney;
                }

                // If all the suppliers are under the money limit then the totalDebt will evaluate to zero, but we
                // still need to update the suppliers_phases table. However we do not need to run the algorithm.

                if (totalOwedMoney != 0)
                {
                    for (int i = 0; i < SuppliersInfo.Count; i++)
                    {
                        SuppliersInfo[i].Percent = (double)SuppliersInfo[i].OwedMoney / (double)totalOwedMoney;
                        SuppliersInfo[i].Difference = (decimal)SuppliersInfo[i].Percent * (decimal)insertedMoney;
                        if (SuppliersInfo[i].Difference > SuppliersInfo[i].OwedMoney)
                        {
                            SuppliersInfo[i].Difference = SuppliersInfo[i].OwedMoney;
                        }
                        SuppliersInfo[i].AssignedMoney += SuppliersInfo[i].Difference;
                        totalInsertedMoney += SuppliersInfo[i].Difference;
                    }
                }

                List<SupplierPhase> newSupplierPhases = new List<SupplierPhase>();
                SuppliersInfo.AddRange(ZeroSuppliersInfo);
                foreach (var supplierInfo in SuppliersInfo)
                {
                    var supplierPhase = new SupplierPhaseRepository(uow).GetSupplierPhase(supplierInfo.ID, selectedPhaseID);
                    if (supplierPhase == null)
                    {
                        var newSupplierPhase = new SupplierPhase()
                        {
                            PhaseID = selectedPhaseID,
                            SupplierID = supplierInfo.ID,
                            TotalDebt = Math.Round((double)supplierInfo.AssignedMoney, 4),
                            CreatedAt = DateTime.Today,
                            CreatedBy = username,
                            IsActive = true
                        };

                        newSupplierPhases.Add(newSupplierPhase);
                        uow.MarkAsNew(newSupplierPhase);
                    }
                    else
                    {
                        supplierPhase.TotalDebt = Math.Round((double)supplierInfo.AssignedMoney, 4);
                        supplierPhase.UpdatedBy = username;
                        supplierPhase.UpdatedAt = DateTime.Today;
                        newSupplierPhases.Add(supplierPhase);
                    }

                    if (supplierInfo.SupplierType == enSupplierType.SelfPublisher && (!supplierInfo.HasLogisticsBooks.HasValue || supplierInfo.HasLogisticsBooks == false))
                    {

                        CatalogGroupHelper.GroupCatalogsByInstitution(supplierInfo.ID, supplierInfo.IBANID, selectedPhaseID, uow);
                        var remaining = supplierInfo.AssignedMoney - supplierInfo.RemainingMoney;

                        if (remaining < 0)
                        {
                            continue;
                        }

                        foreach (var group in supplierInfo.Groups)
                        {
                            if (group.State == enCatalogGroupState.New)
                            {
                                var groupTotal = group.Catalogs.Sum(x => x.Amount);
                                if (groupTotal < remaining)
                                {
                                    remaining -= (decimal)groupTotal;
                                    group.DeductionID = null;
                                    group.Vat = null;
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
                }
                //var supplierPhases = new SupplierPhaseRepository(uow).GetAllActive(selectedPhaseID);
                var currentPhase = new PhaseRepository(uow).Load(selectedPhaseID);
                // fix the Total amount if EnableMoneyDiffRecalculation app setting is true
                currentPhase.PhaseAmount = Math.Round((double)newSupplierPhases.Sum(x => x.TotalDebt), 4);
                currentPhase.UpdatedAt = DateTime.Today;
                currentPhase.UpdatedBy = username;
                uow.Commit();
                EudoxusOsyCacheManager<Phase>.Current.Refresh();
            }
            return true;
        }

        #region Supplier Statistics
        public static List<SupplierPhaseStatistics> GetSupplierPhaseStatistics(int supplierID, int? phaseID = null)
        {
            var statistics = new List<SupplierPhaseStatistics>();

            var phases = new PhaseRepository().GetAllActive().OrderBy(x => x.ID);

            foreach (var phase in phases)
            {
                //Skip phase if requesting statistics for a specific phase
                if (phaseID.HasValue && phase.ID != phaseID)
                    continue;

                var phaseStatistics = new SupplierPhaseStatistics()
                {
                    SupplierID = supplierID,
                    Phase = phase,
                    OwedAmount = 0,
                    AllocatedAmount = 0,
                    RemainingAmount = 0,
                    PaidAmount = 0
                };

                phaseStatistics.AllocatedAmount = Convert.ToDecimal(new SupplierPhaseRepository().GetSupplierPhaseMoney<SupplierPhase>(supplierID, phase.ID));

                var supplierGroups = new CatalogGroupRepository().FindBySupplierIDAndPhaseIDWithCatalogs(supplierID, phase.ID);

                decimal selectedForPaymentAmount = 0;

                var totalDebtBySupplierAndPhase = new CatalogRepository().GetTotalDebtBySupplierAndPhase(supplierID, phase.ID);

                if (totalDebtBySupplierAndPhase != null)
                    phaseStatistics.OwedAmount = (decimal)totalDebtBySupplierAndPhase;

                foreach (var group in supplierGroups)
                {
                    if (group.State == enCatalogGroupState.Sent)
                    {
                        phaseStatistics.PaidAmount += (decimal)group.Catalogs.Where(x => x.Status == enCatalogStatus.Active && (x.State == enCatalogState.FromMove || x.State == enCatalogState.Normal)).Sum(x => x.Amount);
                        selectedForPaymentAmount += (decimal)group.Catalogs.Where(x => x.Status == enCatalogStatus.Active && (x.State == enCatalogState.FromMove || x.State == enCatalogState.Normal)).Sum(x => x.Amount);
                    }
                    else if (group.State != enCatalogGroupState.New)
                    {
                        selectedForPaymentAmount += (decimal)group.Catalogs.Where(x => x.Status == enCatalogStatus.Active && (x.State == enCatalogState.FromMove || x.State == enCatalogState.Normal)).Sum(x => x.Amount);
                    }
                }

                phaseStatistics.RemainingAmount = (phaseStatistics.AllocatedAmount - selectedForPaymentAmount);

                statistics.Add(phaseStatistics);
            }

            return statistics;
        }

        public static SupplierPhaseStatistics GetSupplierSpecificPhaseStatistics(int supplierID, int phaseID)
        {
            return GetSupplierPhaseStatistics(supplierID, phaseID)[0];
        }
        #endregion
    }
}
