using EudoxusOsy.BusinessModel;
using EudoxusOsy.BusinessModel.Flow;
using Imis.Domain;
using System;
using System.Linq;

namespace EudoxusOsy.Portal.Utils
{
    public class PaymentOrdersUserManagement
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositoryFactory _repFactory;
        private ICatalogGroupRepository _catalogGroupRepository;

        public PaymentOrdersUserManagement(IUnitOfWork unitOfWork, IRepositoryFactory repFactory = null)
        {
            _unitOfWork = unitOfWork;
            _repFactory = repFactory;

            InitializeRepositories();
        }

        private void InitializeRepositories()
        {
            if (_repFactory != null)
            {
                this._catalogGroupRepository =
                    _repFactory.GetRepositoryInstance<CatalogGroup, ICatalogGroupRepository>(_unitOfWork);
            }
            else
            {
                this._catalogGroupRepository = new CatalogGroupRepository(_unitOfWork);
            }
        }

        public bool ManageActions(string username, bool isMinistry, string action, string[] parameters,
            out string error, out string jsProperty)
        {
            bool ok = false;
            error = string.Empty;
            jsProperty = "cpError";

            var id = int.Parse(parameters[1]);

            if (action == "include")
            {
                int previousStateInt = int.Parse(parameters[2]);
                var currentGroup = _catalogGroupRepository.Load(id, x => x.Catalogs, x => x.Invoices);

                if (isMinistry || !currentGroup.IsLocked || CheckLockedByMinistry(out error, currentGroup))
                    ok = Include(username, previousStateInt, ref error, ref jsProperty, currentGroup);
                else
                    return false;
            }
            else if (action == "removebanktransfer")
            {
                var currentGroup = _catalogGroupRepository.Load(id);
                RemoveBankTransfer(currentGroup);
                ok = true;
            }
            else if (action == "delete")
            {
                var currentGroup = _catalogGroupRepository.Load(id, x => x.Catalogs, y => y.Invoices, z => z.CatalogGroupLogs);
                ok = Delete(isMinistry, ref error, currentGroup);
            }
            else if (isMinistry && (action == "lock" || action == "unlock"))
            {
                var currentGroup = _catalogGroupRepository.Load(id, x => x.Catalogs);
                LockUnlock(username, action == "lock", currentGroup);
                ok = true;
            }

            return ok;
        }

        #region Actions
        public bool Include(string username, int previousStateInt,
            ref string error, ref string jsProperty, CatalogGroup currentGroup)
        {
            bool ok = false;

            var cgStateMachine = new CatalogGroupStateMachine(currentGroup, _repFactory);

            if (previousStateInt == currentGroup.StateInt)
            {
                if (cgStateMachine.CanFire(enCatalogGroupTriggers.Select))
                {
                    var triggerParams = new CatalogGroupTriggerParams()
                    {
                        UnitOfWork = _unitOfWork,
                        Username = username
                    };

                    cgStateMachine.Select(triggerParams);

                    _unitOfWork.Commit();
                    ok = true;
                }
                else if (cgStateMachine.CanFire(enCatalogGroupTriggers.RevertSelection))
                {
                    var triggerParams = new CatalogGroupTriggerParams()
                    {
                        UnitOfWork = _unitOfWork,
                        Username = username
                    };

                    cgStateMachine.RevertSelection(triggerParams);

                    _unitOfWork.Commit();
                    ok = true;
                }
                else if (currentGroup.ContainsInActiveBooks.Value)
                {
                    error = "Η κατάσταση δεν μπορεί να επιλεχθεί για πληρωμή, γιατί περιέχει διανομές βιβλίων που δεν είναι δυνατόν να τιμολογηθούν και να αποζημιωθούν. Για περισσότερες διευκρινίσεις επικοινωνήστε με το Γραφείο Αρωγής Χρηστών.";
                }
                //do not allow group selection if it contains books that have price verification from the committee pending and phase >= 13
                else if (currentGroup.ContainsCommitteePendingPrices.HasValue && currentGroup.ContainsCommitteePendingPrices.Value)
                {
                    error = "Η κατάσταση δεν μπορεί να επιλεχθεί για πληρωμή, γιατί περιέχει διανομές βιβλίων των οποίων η τιμή εξετάζεται από την επιτροπή κοστολόγησης. Για περισσότερες διευκρινίσεις επικοινωνήστε με το Γραφείο Αρωγής Χρηστών.";
                }
                //do not allow group selection if it contains books that have unexpected price change and phase >= 13                        
                else if (currentGroup.ContainsUnexpectedPendingPrices.HasValue && currentGroup.ContainsUnexpectedPendingPrices.Value)
                {
                    error = "Η κατάσταση δεν μπορεί να επιλεχθεί για πληρωμή, γιατί περιέχει διανομές βιβλίων με μη αναμενόμενη αλλαγή στην τιμή τους. Για περισσότερες διευκρινίσεις επικοινωνήστε με το Γραφείο Αρωγής Χρηστών.";
                }
                else
                {
                    error = currentGroup.StateInt >= enCatalogGroupState.Approved.GetValue()
                        ? "Δεν επιτρέπεται η ενέργεια αναίρεσης της \"Επιλογής για πληρωμή\" καθώς η κατάσταση έχει εγκριθεί για πληρωμή."
                        : "Δεν μπορείτε να επιλέξετε τη συγκεκριμένη κατάσταση για πληρωμή.";
                }
            }
            else
            {
                jsProperty = "cpMessage";
                error = "Το στάδιο της κατάστασης είχε αλλάξει από την τελευταία φορά που φορτώσατε τη σελίδα. Έγινε συγχρονισμός της σελίδας με τα πραγματικά δεδομένα.";
            }
            return ok;
        }

        public bool Delete(bool isMinistry, ref string error, CatalogGroup currentGroup)
        {
            bool ok = false;

            if (CatalogGroupHelper.CanDeleteGroup(currentGroup.ToCatalogGroupInfo()))
            {
                if ((currentGroup.ReversalCount.HasValue && currentGroup.ReversalCount.Value > 0) ||
                    (!isMinistry && currentGroup.LockedCatalogGroups.Any()))
                {
                    error = "Δεν μπορείτε να διαγράψετε τη συγκεκριμένη κατάσταση γιατί έχει ήδη ελεγχθεί από το Υπουργείο.";
                }
                else
                {
                    currentGroup.Catalogs.ToList().ForEach(x => { x.GroupID = null; });
                    currentGroup.Invoices.ToList().ForEach(_unitOfWork.MarkAsDeleted);
                    currentGroup.CatalogGroupLogs.ToList().ForEach(_unitOfWork.MarkAsDeleted);

                    _unitOfWork.MarkAsDeleted(currentGroup);
                    _unitOfWork.Commit();
                    ok = true;
                }
            }
            else
            {
                error = "Η κατάσταση δεν μπορεί να διαγραφεί.";
            }
            return ok;
        }

        public void RemoveBankTransfer(CatalogGroup currentGroup)
        {
            currentGroup.IsTransfered = false;
            currentGroup.BankID = null;

            _unitOfWork.Commit();
        }

        public void LockUnlock(string username, bool actionLock, CatalogGroup currentGroup)
        {
            currentGroup.IsLocked = actionLock;

            var log = CreateLog(username, currentGroup, actionLock);

            _unitOfWork.MarkAsNew(log);
            _unitOfWork.Commit();
        }

        public bool MoveToState(enCatalogGroupTriggers trigger, CatalogGroup catalogGroup, 
            string username, string comments, DateTime? sentToYDEDate = null)
        {
            bool ok = false;
            var stateMachine = new CatalogGroupStateMachine(catalogGroup, _repFactory);

            if (stateMachine.CanFire(trigger))
            {
                var triggerParams = new CatalogGroupTriggerParams()
                {
                    Username = username,
                    Comments = comments,
                    UnitOfWork = _unitOfWork
                };

                switch (trigger)
                {
                    case enCatalogGroupTriggers.Approve:
                        stateMachine.Approve(triggerParams);
                        break;
                    case enCatalogGroupTriggers.RevertApproval:
                        stateMachine.RevertApproval(triggerParams);
                        break;
                    case enCatalogGroupTriggers.SendToYDE:
                        triggerParams.SentToYDEDate = sentToYDEDate;
                        stateMachine.SendToYDE(triggerParams);
                        break;
                    case enCatalogGroupTriggers.ReturnFromYDE:
                        stateMachine.ReturnFromYDE(triggerParams);
                        break;
                }                                
                _unitOfWork.Commit();
                ok = true;
            }

            return ok;
        }
        #endregion

        private static CatalogGroupLog CreateLog(string username, CatalogGroup currentGroup, bool actionLock)
        {
            var log = new CatalogGroupLog
            {
                GroupID = currentGroup.ID,
                CreatedAt = DateTime.Now,
                CreatedBy = username,
                Comments = actionLock
                    ? enCatalogGroupLogAction.Lock.GetLabel()
                    : enCatalogGroupLogAction.Unlock.GetLabel(),
                Amount = (double?)currentGroup.TotalAmount
            };

            log.OldState = log.NewState = currentGroup.State;
            log.SetOldValues(new CatalogGroupChangeValues() { IsLocked = !actionLock });
            log.SetNewValues(new CatalogGroupChangeValues() { IsLocked = actionLock });
            return log;
        }

        private static bool CheckLockedByMinistry(out string error, CatalogGroup currentGroup)
        {
            if (currentGroup.State == enCatalogGroupState.New)
            {
                error = "Η κατάσταση δεν μπορεί να επιλεχθεί για πληρωμή, γιατί είναι κλειδωμένη από το Υπουργείο.";
                return false;
            }
            if (currentGroup.State >= enCatalogGroupState.Selected)
            {
                error = "Η κατάσταση δεν μπορεί να απο-επιλεγεί για πληρωμή, γιατί βρίσκεται σε έλεγχο από το Υπουργείο.";
                return false;
            }
            error = string.Empty;
            return true;
        }
    }
}
