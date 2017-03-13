using Imis.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EudoxusOsy.BusinessModel
{
    public static class PhaseHelper
    {
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


            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                /** 
                    Get all the active Suppliers for the Phase
                */
                var suppliers = new SupplierRepository(uow).GetAllActive();
                var supplierIDs = suppliers.Select(x => x.ID);

                List<SupplierInfo> SuppliersInfo = new List<SupplierInfo>();
                List<SupplierInfo> ZeroSuppliersInfo = new List<SupplierInfo>();


                foreach (var supplier in suppliers)
                {
                    /**
                        calculate the debt to the supplier
                    */
                    var totalDebtFromSupplierCatalogs = supplier.GetSupplierMoneyDebt(selectedPhaseID);
                    var supplierGroups = new CatalogGroupRepository().FindBySupplierIDAndPhaseIDWithCatalogs(supplier.ID, selectedPhaseID);

                    decimal supplierAssignedMoney = 0m;

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
                        Groups = supplierGroups
                    };

                    var currentSupplierPhase = new SupplierPhaseRepository(uow).GetSupplierPhase(supplier.ID, selectedPhaseID);

                    if (currentSupplierPhase == null)
                    {
                        supplierAssignedMoney = 0m;
                        supplierInfo.AssignedMoney = 0m;
                    }
                    else
                    {
                        supplierInfo.AssignedMoney = (decimal)currentSupplierPhase.TotalDebt;
                        supplierAssignedMoney = (decimal)currentSupplierPhase.TotalDebt;
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

                    if(supplierInfo.OwedMoney > 0)
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
                        if(insertedMoney < 0)
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
                            TotalDebt = Math.Round((double)supplierInfo.AssignedMoney, 2),
                            CreatedAt = DateTime.Today,
                            CreatedBy = username,
                            IsActive = true
                        };

                        newSupplierPhases.Add(newSupplierPhase);
                        uow.MarkAsNew(newSupplierPhase);
                    }
                    else
                    {
                        supplierPhase.TotalDebt = Math.Round((double)supplierInfo.AssignedMoney, 2);
                        supplierPhase.UpdatedBy = username;
                        supplierPhase.UpdatedAt = DateTime.Today;
                        newSupplierPhases.Add(supplierPhase);
                    }

                    if (supplierInfo.SupplierType == enSupplierType.SelfPublisher && (!supplierInfo.HasLogisticsBooks.HasValue || supplierInfo.HasLogisticsBooks == false))
                    {

                        CatalogGroupHelper.GroupCatalogsByInstitution(supplierInfo.ID, selectedPhaseID, uow);
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
                currentPhase.PhaseAmount = Math.Round((double)newSupplierPhases.Sum(x => x.TotalDebt), 2);
                currentPhase.UpdatedAt = DateTime.Today;
                currentPhase.UpdatedBy = username;
                uow.Commit();
                EudoxusOsyCacheManager<Phase>.Current.Refresh();
            }
            return true;
        }

        public static List<SupplierPhaseStatistics> GetSupplierPhaseStatistics(int supplierID, int? phaseID = null)
        {
            var statistics = new List<SupplierPhaseStatistics>();

            var phases = new PhaseRepository().LoadAll().OrderBy(x => x.ID);

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

                foreach (var group in supplierGroups)
                {
                    phaseStatistics.OwedAmount += (decimal)group.Catalogs.Sum(x => x.Amount);

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
    }

    public class SupplierInfo
    {
        public int ID { get; set; }
        public int KpsID { get; set; }
        public decimal? TotalDebtFromSupplierCatalogs { get; set; }
        public enSupplierType SupplierType { get; set; }
        public bool? HasLogisticsBooks { get; set; }
        public decimal AssignedMoney { get; set; }
        public decimal PaidMoney { get; set; }
        public decimal OwedMoney { get; set; }
        public double Percent { get; set; }
        public decimal Difference { get; set; }
        public decimal RemainingMoney { get; set; }
        public IList<CatalogGroup> Groups { get; set; }
    }

    public class SupplierPhaseStatistics
    {
        public int SupplierID { get; set; }
        public Phase Phase { get; set; }
        public decimal OwedAmount { get; set; }
        public decimal AllocatedAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public decimal PaidAmount { get; set; }
    }
}
