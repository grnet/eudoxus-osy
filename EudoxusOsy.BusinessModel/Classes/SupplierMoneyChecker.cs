using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imis.Domain;

namespace EudoxusOsy.BusinessModel
{
    public class SupplierMoneyChecker
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositoryFactory _repFactory;

        private ICatalogRepository _catalogRepository;
        private ISupplierRepository _supplierRepository;
        private ICatalogGroupRepository _catalogGroupRepository;
        private ISupplierPhaseRepository _supplierPhaseRepository;
        private IPhaseRepository _phaseRepository;

        public SupplierMoneyChecker(IUnitOfWork unitOfWork, IRepositoryFactory repFactory = null)
        {
            _unitOfWork = unitOfWork;
            _repFactory = repFactory;

            InitializeRepositories();
        }

        private void InitializeRepositories()
        {
            if (_repFactory != null)
            {
                _catalogRepository = _repFactory.GetRepositoryInstance<Catalog, ICatalogRepository>(_unitOfWork);
                _supplierRepository = _repFactory.GetRepositoryInstance<Supplier, ISupplierRepository>(_unitOfWork);
                _catalogGroupRepository =
                    _repFactory.GetRepositoryInstance<CatalogGroup, ICatalogGroupRepository>(_unitOfWork);
                _supplierPhaseRepository =
                    _repFactory.GetRepositoryInstance<SupplierPhase, ISupplierPhaseRepository>(_unitOfWork);
            }
            else
            {
                _catalogRepository = new CatalogRepository(_unitOfWork);
                _supplierRepository = new SupplierRepository(_unitOfWork);
                _catalogGroupRepository = new CatalogGroupRepository(_unitOfWork);
                _supplierPhaseRepository = new SupplierPhaseRepository(_unitOfWork);
            }
        }

        public bool SetMoney(SetMoneyParameters parameters, bool saveToFile)
        {
            decimal availableAmount = parameters.PhaseAmount;
            decimal amountLimit = parameters.AmountLimit;
            
            bool ok = false;

            decimal remainingMoney = availableAmount;
            decimal remainingDebt = 0;

            List<Supplier> suppliers = _supplierRepository.GetAllActive().ToList();            
            List<SupplierInfo> suppliersInfo = FillUpSuppliersInfo(suppliers, parameters.SelectedPhaseID);

            ok = DistributeUntilLimit(suppliersInfo, amountLimit, ref remainingMoney, ref remainingDebt);

            if (ok && remainingMoney > 0 && remainingDebt > 0)
            {
                // suppliers -> osoi den xrostane
                ok = DistributeByPercentage(suppliersInfo, remainingMoney, remainingDebt);
            }

            if (ok && saveToFile)
            {
                WriteToFile(parameters.SelectedPhaseID, parameters.Username, suppliersInfo);                
            }
            else if (ok)
            {
                var newSupplierPhases = CreateSupplierPhasesForSuppliers(parameters.SelectedPhaseID,
                    parameters.Username, suppliersInfo);
               
                var currentPhase = _phaseRepository.Load(parameters.SelectedPhaseID);
                currentPhase.PhaseAmount = Math.Round((double)newSupplierPhases.Sum(x => x.TotalDebt), 4);
                currentPhase.UpdatedAt = DateTime.Today;
                currentPhase.UpdatedBy = parameters.Username;

                _unitOfWork.Commit();
            }

            return ok;
        }

        public List<SupplierInfo> FillUpSuppliersInfo(List<Supplier> suppliers, int phaseId)
        {
            List<SupplierInfo> suppliersInfo = new List<SupplierInfo>();

            foreach (Supplier supplier in suppliers)
            {
                decimal? debt = _catalogRepository.GetSupplierMoneyDebt(supplier.ID, phaseId);

                if (!debt.HasValue || debt.Value <= 0)
                {
                    continue;
                }

                SupplierPhase supplierPhase = _supplierPhaseRepository.GetSupplierPhase(supplier.ID, phaseId);
                decimal assignedMoney = 0;

                if (supplierPhase != null)
                {
                    assignedMoney = (decimal) supplierPhase.TotalDebt;
                }                

                var supplierInfo = new SupplierInfo()
                {
                    ID = supplier.ID,
                    KpsID = supplier.SupplierKpsID,
                    TotalDebtFromSupplierCatalogs = debt,
                    OwedMoney = debt.Value - assignedMoney,
                    AssignedMoney = assignedMoney,
                    SupplierType = supplier.SupplierType,
                    HasLogisticsBooks = supplier.HasLogisticBooks                    
                };

                suppliersInfo.Add(supplierInfo);
            }

            return suppliersInfo;
        }

        public bool DistributeUntilLimit(List<SupplierInfo> suppliers, decimal amountLimit, 
            ref decimal remainingMoney, ref decimal remainingDebt)
        {
            bool ok = true;
            
            foreach (SupplierInfo supplier in suppliers)
            {
                if (remainingMoney < amountLimit)
                {
                    ok = false;
                    break;
                }

                if (!CalculateUntilLimit(supplier, amountLimit, ref remainingMoney))
                {                   
                    break;
                }

                remainingDebt += supplier.OwedMoney;
            }
            return ok;
        }

        private bool CalculateUntilLimit(SupplierInfo supplier, decimal amountLimit, ref decimal remainingMoney)
        {
            bool ok = false;            

            if (supplier.OwedMoney < 0)
            {
                ok = false;
            }
            else if (supplier.OwedMoney == 0)
            {
                ok = true;                
            }
            else
            {
                decimal moneyToAssign = 0;

                moneyToAssign = amountLimit >= supplier.OwedMoney ? supplier.OwedMoney : amountLimit;

                remainingMoney -= moneyToAssign;

                supplier.AssignedMoney += moneyToAssign;
                supplier.OwedMoney -= moneyToAssign;

                ok = true;
            }

            return ok;            
        }

        public bool DistributeByPercentage(List<SupplierInfo> suppliers, decimal remainingMoney, decimal remainingDebt)
        {
            bool ok = false;

            foreach (SupplierInfo supplier in suppliers)
            {
                if (supplier.OwedMoney == 0)
                {
                    continue;
                }
                
                if (!CalculateByPercentage(supplier, remainingMoney, remainingDebt))
                {
                    break;
                }
            }
            return ok;
        }

        private bool CalculateByPercentage(SupplierInfo si,  decimal remainingMoney, decimal remainingDebt)
        {
            if (remainingDebt == 0)
            {
                return false;
            }

            si.Percent = (double) si.OwedMoney / (double) remainingDebt;
            si.Difference = (decimal) si.Percent * remainingMoney;

            if (si.Difference > si.OwedMoney)
            {
                si.Difference = si.OwedMoney;
            }

            si.AssignedMoney += si.Difference;
            si.OwedMoney -= si.Difference;            

            return true;
        }

        public void WriteToFile(int selectedPhaseId, string username,
            List<SupplierInfo> suppliersInfo)
        {
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(Config.SetMoneyCheckFile + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt"))
            {

                foreach (var supplierInfo in suppliersInfo)
                {
                    file.WriteLine("{0}\t{1}", supplierInfo.ID, Math.Round((double) supplierInfo.AssignedMoney, 4));
                }
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
                    _unitOfWork.MarkAsNew(newSupplierPhase);
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
                    //SelfPublisherOrNoLogisticBooks(selectedPhaseId, username, supplierInfo);
                }
            }
            return newSupplierPhases;
        }
    }
}
