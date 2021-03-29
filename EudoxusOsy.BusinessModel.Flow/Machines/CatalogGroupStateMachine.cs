using EudoxusOsy.BusinessModel.Interfaces;
using EudoxusOsy.Utils;
using Imis.Domain;
using Stateless;
using System;
using System.Collections.Generic;

namespace EudoxusOsy.BusinessModel.Flow
{
    public class CatalogGroupStateMachine : StateMachine<enCatalogGroupState, enCatalogGroupTriggers>
    {
        private readonly IRepositoryFactory _repFactory;

        #region [ Helpers ]

        #region [ Triggers ]

        Dictionary<enCatalogGroupTriggers, TriggerWithParameters<CatalogGroupTriggerParams>> _triggers =
            new Dictionary<enCatalogGroupTriggers, TriggerWithParameters<CatalogGroupTriggerParams>>();

        public TriggerWithParameters<CatalogGroupTriggerParams> TriggerFor(enCatalogGroupTriggers trigger)
        {
            if (!_triggers.ContainsKey(trigger))
            {
                _triggers.Add(trigger, SetTriggerParameters<CatalogGroupTriggerParams>(trigger));
            }
            return _triggers[trigger];
        }

        #endregion

        private CatalogGroupLog GetLog(CatalogGroupTriggerParams triggerParams, Transition transition)
        {
            return new CatalogGroupLog()
            {
                OldState = transition.Source,
                NewState = transition.Destination,
                CreatedAt = DateTime.Now,
                CreatedBy = triggerParams.Username,
                GroupID = CatalogGroup.ID,
                Amount = (double?)CatalogGroup.TotalAmount,
                Comments = triggerParams.Comments
            };
        }
        #endregion

        public CatalogGroupStateMachine(CatalogGroup group, IRepositoryFactory repFactory = null)
            : base(group.State)
        {
            CatalogGroup = group;
            this._repFactory = repFactory;
            ConfigureInitial();
        }

        protected CatalogGroup CatalogGroup { get; set; }

        #region [ Configuration Methods ]

        private void ConfigureInitial()
        {
            Configure(enCatalogGroupState.New)
                .PermitIf(enCatalogGroupTriggers.Select, enCatalogGroupState.Selected, () => (
                    !CatalogGroup.ContainsInActiveBooks.Value &&
                    CatalogGroup.DoesNotHavePendingPrices &&
                    (CatalogGroup.TotalAmount <= 0 || (
                        !CatalogGroup.InvoiceSum.HasValue || CatalogGroup.InvoiceSum <= CatalogGroup.TotalAmount)
                    )
                    ))
                .OnEntryFrom(TriggerFor(enCatalogGroupTriggers.Delete),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;
                        uow.MarkAsNew(GetLog(triggerParams, transition));

                        var paymentOrder = GetPaymentOrderRep(_repFactory, uow).FindByGroupID(CatalogGroup.ID);
                        if (paymentOrder != null)
                        {
                            enCatalogGroupState catalogGroupState = (enCatalogGroupState)Enum.Parse(typeof(enCatalogGroupState), transition.Destination.GetValue().ToString());
                            paymentOrder.State = CatalogGroupHelper.MapCatalogGroupStateToPaymentOrderState(catalogGroupState);
                        }
                        CatalogGroup.State = transition.Destination;
                    })
                .OnEntryFrom(TriggerFor(enCatalogGroupTriggers.RevertSelection),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;
                        uow.MarkAsNew(GetLog(triggerParams, transition));
                        var paymentOrder = GetPaymentOrderRep(_repFactory, uow).FindByGroupID(CatalogGroup.ID);
                        if (paymentOrder != null)
                        {
                            enCatalogGroupState catalogGroupState = (enCatalogGroupState)Enum.Parse(typeof(enCatalogGroupState), transition.Destination.GetValue().ToString());
                            paymentOrder.State = CatalogGroupHelper.MapCatalogGroupStateToPaymentOrderState(catalogGroupState);
                        }

                        CatalogGroup.State = transition.Destination;
                    })
                .OnEntryFrom(TriggerFor(enCatalogGroupTriggers.RevertSelectionByMinistry),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;
                        uow.MarkAsNew(GetLog(triggerParams, transition));
                        var paymentOrder = GetPaymentOrderRep(_repFactory, uow).FindByGroupID(CatalogGroup.ID);
                        if (paymentOrder != null)
                        {
                            enCatalogGroupState catalogGroupState = (enCatalogGroupState)Enum.Parse(typeof(enCatalogGroupState), transition.Destination.GetValue().ToString());
                            paymentOrder.State = CatalogGroupHelper.MapCatalogGroupStateToPaymentOrderState(catalogGroupState);
                        }

                        CatalogGroup.State = transition.Destination;
                        CatalogGroup.ReversalCount++;
                    });

            Configure(enCatalogGroupState.Selected)
                .Permit(enCatalogGroupTriggers.Approve, enCatalogGroupState.Approved)
                .PermitIf(enCatalogGroupTriggers.RevertSelection, enCatalogGroupState.New, () => !CatalogGroup.ReversalCount.HasValue || CatalogGroup.ReversalCount == 0)
                .OnEntryFrom(TriggerFor(enCatalogGroupTriggers.Select),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;
                        uow.MarkAsNew(GetLog(triggerParams, transition));
                        var paymentOrder = GetPaymentOrderRep(_repFactory, uow).FindByGroupID(CatalogGroup.ID);
                        if (paymentOrder != null)
                        {
                            enCatalogGroupState catalogGroupState = (enCatalogGroupState)Enum.Parse(typeof(enCatalogGroupState), transition.Destination.GetValue().ToString());
                            paymentOrder.State = CatalogGroupHelper.MapCatalogGroupStateToPaymentOrderState(catalogGroupState);
                        }

                        CatalogGroup.State = transition.Destination;
                    })
                    .OnEntryFrom(TriggerFor(enCatalogGroupTriggers.RevertApproval),
                    (triggerParams, transition) => { ManageApproval(triggerParams, transition); });

            Configure(enCatalogGroupState.Approved)
                .Permit(enCatalogGroupTriggers.SendToYDE, enCatalogGroupState.Sent)
                .Permit(enCatalogGroupTriggers.RevertApproval, enCatalogGroupState.Selected)
                .OnEntryFrom(TriggerFor(enCatalogGroupTriggers.Approve),
                    (triggerParams, transition) =>
                    {
                        ManageApproval(triggerParams, transition);
                    });

            Configure(enCatalogGroupState.Sent)
                .Permit(enCatalogGroupTriggers.ReturnFromYDE, enCatalogGroupState.Returned)
                .OnEntryFrom(TriggerFor(enCatalogGroupTriggers.SendToYDE),
                    (triggerParams, transition) =>
                    {
                        ManageYDE(triggerParams, transition);
                    });

            Configure(enCatalogGroupState.Returned)
                .Permit(enCatalogGroupTriggers.SendToYDE, enCatalogGroupState.Sent)
                .OnEntryFrom(TriggerFor(enCatalogGroupTriggers.ReturnFromYDE),
                    (triggerParams, transition) => { ManageYDE(triggerParams, transition); });
        }

        private void ManageApproval(CatalogGroupTriggerParams triggerParams, Transition transition)
        {
            IUnitOfWork uow = triggerParams.UnitOfWork;
            uow.MarkAsNew(GetLog(triggerParams, transition));
            enCatalogGroupState catalogGroupState =
                (enCatalogGroupState) Enum.Parse(typeof(enCatalogGroupState), transition.Destination.GetValue().ToString());
            var paymentOrder = GetPaymentOrder(uow, catalogGroupState);

            CatalogGroup.State = transition.Destination;
            CatalogGroup.Comments = triggerParams.Comments;
        }

        private void ManageYDE(CatalogGroupTriggerParams triggerParams, Transition transition)
        {
            IUnitOfWork uow = triggerParams.UnitOfWork;
            var groupLog = GetLog(triggerParams, transition);
            uow.MarkAsNew(groupLog);
            enCatalogGroupState catalogGroupState =
                (enCatalogGroupState) Enum.Parse(typeof(enCatalogGroupState), transition.Destination.GetValue().ToString());
            var paymentOrder = GetPaymentOrder(uow, catalogGroupState);

            paymentOrder = UpdateOfficeSlipNumber(triggerParams, paymentOrder, catalogGroupState);
            groupLog.OfficeSlipNumber = paymentOrder.OfficeSlipNumber;
            groupLog.OfficeSlipDate = paymentOrder.OfficeSlipDate;
            CatalogGroup.State = transition.Destination;
        }

        private PaymentOrder GetPaymentOrder(IUnitOfWork uow, enCatalogGroupState catalogGroupState)
        {
            var paymentOrder = GetPaymentOrderRep(_repFactory, uow).FindByGroupID(CatalogGroup.ID);
            if (paymentOrder != null)
            {               
                paymentOrder.State = CatalogGroupHelper.MapCatalogGroupStateToPaymentOrderState(catalogGroupState);
            }
            return paymentOrder;
        }

        private IPaymentOrderRepository GetPaymentOrderRep(IRepositoryFactory repFactory, IUnitOfWork uow)
        {
            return repFactory != null ? repFactory.GetRepositoryInstance<PaymentOrder, IPaymentOrderRepository>(uow) : new PaymentOrderRepository(uow);
        }

        private PaymentOrder UpdateOfficeSlipNumber(CatalogGroupTriggerParams triggerParams, PaymentOrder paymentOrder, enCatalogGroupState catalogGroupState)
        {            
            int officeSlipYear;

            if (triggerParams.SentToYDEDate.HasValue)
            {
                officeSlipYear = triggerParams.SentToYDEDate.Value.Year;
            }
            else
            {
                officeSlipYear = DateTime.Today.Year;
            }

            var maxOfficeSlipNumber = GetPaymentOrderRep(_repFactory, triggerParams.UnitOfWork).FindMaxOfficeSlipNumber(officeSlipYear);
            maxOfficeSlipNumber++;

            double paymentOrderAmount = CatalogGroup.TotalAmount != null ? (double) CatalogGroup.TotalAmount.Value : 0;

            // if paymentOrder does not exist for this group, create it
            if (paymentOrder == null)
            {
                var newPaymentOrder = new PaymentOrder()
                {
                    GroupID = CatalogGroup.ID,
                    IsActive = true,
                    OfficeSlipDate = triggerParams.SentToYDEDate,
                    OfficeSlipNumber = Convert.ToInt32(officeSlipYear.ToString() + maxOfficeSlipNumber.ToString("000000")),
                    State = CatalogGroupHelper.MapCatalogGroupStateToPaymentOrderState(catalogGroupState),
                    Comments = triggerParams.Comments,
                    Amount = paymentOrderAmount,
                    CreatedAt = DateTime.Now,
                    CreatedBy = triggerParams.Username
                };

                triggerParams.UnitOfWork.MarkAsNew(newPaymentOrder);
                return newPaymentOrder;
            }
            else // if paymentOrder exists for the group, update its data
            {
                paymentOrder.OfficeSlipDate = triggerParams.SentToYDEDate;
                paymentOrder.OfficeSlipNumber = Convert.ToInt32(officeSlipYear.ToString() + maxOfficeSlipNumber.ToString("000000"));
                paymentOrder.IsActive = true;
                paymentOrder.Comments = triggerParams.Comments;
                paymentOrder.Amount = paymentOrderAmount;
                paymentOrder.UpdatedAt = DateTime.Now;
                paymentOrder.UpdatedBy = triggerParams.Username;

                return paymentOrder;
            }
        }

        #endregion

        #region [ ShortCut Methods ]

        public void Delete(CatalogGroupTriggerParams triggerParams)
        {
            try
            {
                Fire(TriggerFor(enCatalogGroupTriggers.Delete), triggerParams);
            }
            catch (InvalidOperationException ex)
            {
                LogHelper.LogError(ex, this);
            }
        }

        public void Select(CatalogGroupTriggerParams triggerParams)
        {
            try
            {
                Fire(TriggerFor(enCatalogGroupTriggers.Select), triggerParams);
            }
            catch (InvalidOperationException ex)
            {
                LogHelper.LogError(ex, this);
            }
        }

        /// <summary>
        /// Reverted by Supplier ONLY
        /// </summary>
        /// <param name="triggerParams"></param>
        public void RevertSelection(CatalogGroupTriggerParams triggerParams)
        {
            try
            {
                Fire(TriggerFor(enCatalogGroupTriggers.RevertSelection), triggerParams);
            }
            catch (InvalidOperationException ex)
            {
                LogHelper.LogError(ex, this);
            }
        }

        /// <summary>
        /// Reverted by Ministry
        /// </summary>
        /// <param name="triggerParams"></param>
        public void RevertSelectionByMinistry(CatalogGroupTriggerParams triggerParams)
        {
            try
            {
                Fire(TriggerFor(enCatalogGroupTriggers.RevertSelectionByMinistry), triggerParams);
            }
            catch (InvalidOperationException ex)
            {
                LogHelper.LogError(ex, this);
            }
        }

        public void Approve(CatalogGroupTriggerParams triggerParams)
        {
            try
            {
                Fire(TriggerFor(enCatalogGroupTriggers.Approve), triggerParams);
            }
            catch (InvalidOperationException ex)
            {
                LogHelper.LogError(ex, this);
            }
        }

        public void RevertApproval(CatalogGroupTriggerParams triggerParams)
        {
            try
            {
                Fire(TriggerFor(enCatalogGroupTriggers.RevertApproval), triggerParams);
            }
            catch (InvalidOperationException ex)
            {
                LogHelper.LogError(ex, this);
            }
        }

        public void SendToYDE(CatalogGroupTriggerParams triggerParams)
        {
            try
            {
                Fire(TriggerFor(enCatalogGroupTriggers.SendToYDE), triggerParams);
            }
            catch (InvalidOperationException ex)
            {
                LogHelper.LogError(ex, this);
            }
        }

        public void ReturnFromYDE(CatalogGroupTriggerParams triggerParams)
        {
            try
            {
                Fire(TriggerFor(enCatalogGroupTriggers.ReturnFromYDE), triggerParams);
            }
            catch (InvalidOperationException ex)
            {
                LogHelper.LogError(ex, this);
            }
        }



        #endregion
    }
}
