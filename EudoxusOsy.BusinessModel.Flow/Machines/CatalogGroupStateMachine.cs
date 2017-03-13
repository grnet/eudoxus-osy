using EudoxusOsy.Utils;
using Imis.Domain;
using Stateless;
using System;
using System.Collections.Generic;

namespace EudoxusOsy.BusinessModel.Flow
{
    public class CatalogGroupStateMachine : StateMachine<enCatalogGroupState, enCatalogGroupTriggers>
    {
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

        public CatalogGroupStateMachine(CatalogGroup group)
            : base(group.State)
        {
            CatalogGroup = group;
            ConfigureInitial();
        }

        protected CatalogGroup CatalogGroup { get; set; }

        #region [ Configuration Methods ]

        private void ConfigureInitial()
        {
            Configure(enCatalogGroupState.New)
                .PermitIf(enCatalogGroupTriggers.Select, enCatalogGroupState.Selected, () => (
                    !CatalogGroup.ContainsInActiveBooks.Value &&
                    !CatalogGroup.ContainsPendingPriceVerificationCatalogs.Value &&
                        (!CatalogGroup.InvoiceSum.HasValue || CatalogGroup.InvoiceSum <= CatalogGroup.TotalAmount)))
                .OnEntryFrom(TriggerFor(enCatalogGroupTriggers.Delete),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;
                        uow.MarkAsNew(GetLog(triggerParams, transition));
                        var paymentOrder = new PaymentOrderRepository(uow).FindByGroupID(CatalogGroup.ID);
                        if (paymentOrder != null)
                        {
                            paymentOrder.State = (enPaymentOrderState)Enum.Parse(typeof(enPaymentOrderState), transition.Destination.GetValue().ToString());
                        }
                        CatalogGroup.State = transition.Destination;
                    })
                .OnEntryFrom(TriggerFor(enCatalogGroupTriggers.RevertSelection),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;
                        uow.MarkAsNew(GetLog(triggerParams, transition));
                        var paymentOrder = new PaymentOrderRepository(uow).FindByGroupID(CatalogGroup.ID);
                        if (paymentOrder != null)
                        {
                            paymentOrder.State = (enPaymentOrderState)Enum.Parse(typeof(enPaymentOrderState), transition.Destination.GetValue().ToString());
                        }

                        CatalogGroup.State = transition.Destination;
                    })
                .OnEntryFrom(TriggerFor(enCatalogGroupTriggers.RevertSelectionByMinistry),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;
                        uow.MarkAsNew(GetLog(triggerParams, transition));
                        var paymentOrder = new PaymentOrderRepository(uow).FindByGroupID(CatalogGroup.ID);
                        if (paymentOrder != null)
                        {
                            paymentOrder.State = (enPaymentOrderState)Enum.Parse(typeof(enPaymentOrderState), transition.Destination.GetValue().ToString());
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
                        var paymentOrder = new PaymentOrderRepository(uow).FindByGroupID(CatalogGroup.ID);
                        if (paymentOrder != null)
                        {
                            paymentOrder.State = (enPaymentOrderState)Enum.Parse(typeof(enPaymentOrderState), transition.Destination.GetValue().ToString());
                        }

                        CatalogGroup.State = transition.Destination;
                    })
                    .OnEntryFrom(TriggerFor(enCatalogGroupTriggers.RevertApproval),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;
                        uow.MarkAsNew(GetLog(triggerParams, transition));
                        var paymentOrder = new PaymentOrderRepository(uow).FindByGroupID(CatalogGroup.ID);
                        if (paymentOrder != null)
                        {
                            paymentOrder.State = (enPaymentOrderState)Enum.Parse(typeof(enPaymentOrderState), transition.Destination.GetValue().ToString());
                        }

                        CatalogGroup.State = transition.Destination;
                        CatalogGroup.Comments = triggerParams.Comments;
                    });

            Configure(enCatalogGroupState.Approved)
                .Permit(enCatalogGroupTriggers.SendToYDE, enCatalogGroupState.Sent)
                .Permit(enCatalogGroupTriggers.RevertApproval, enCatalogGroupState.Selected)
                .OnEntryFrom(TriggerFor(enCatalogGroupTriggers.Approve),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;
                        uow.MarkAsNew(GetLog(triggerParams, transition));
                        var paymentOrder = new PaymentOrderRepository(uow).FindByGroupID(CatalogGroup.ID);
                        if (paymentOrder != null)
                        {
                            paymentOrder.State = (enPaymentOrderState)Enum.Parse(typeof(enPaymentOrderState), transition.Destination.GetValue().ToString());
                        }

                        CatalogGroup.State = transition.Destination;
                        CatalogGroup.Comments = triggerParams.Comments;

                    });

            Configure(enCatalogGroupState.Sent)
                .Permit(enCatalogGroupTriggers.ReturnFromYDE, enCatalogGroupState.Returned)
                .OnEntryFrom(TriggerFor(enCatalogGroupTriggers.SendToYDE),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;
                        var groupLog = GetLog(triggerParams, transition);
                        uow.MarkAsNew(groupLog);
                        var paymentOrder = new PaymentOrderRepository(uow).FindByGroupID(CatalogGroup.ID);

                        if (paymentOrder != null)
                        {
                            paymentOrder.State = (enPaymentOrderState)Enum.Parse(typeof(enPaymentOrderState), transition.Destination.GetValue().ToString());
                            paymentOrder.Comments = triggerParams.Comments;
                        }

                        paymentOrder = UpdateOfficeSlipNumber(triggerParams, paymentOrder);
                        groupLog.OfficeSlipNumber = paymentOrder.OfficeSlipNumber;
                        groupLog.OfficeSlipDate = paymentOrder.OfficeSlipDate;
                        CatalogGroup.State = transition.Destination;
                    });

            Configure(enCatalogGroupState.Returned)
                .Permit(enCatalogGroupTriggers.SendToYDE, enCatalogGroupState.Sent)
                .OnEntryFrom(TriggerFor(enCatalogGroupTriggers.ReturnFromYDE),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;
                        var groupLog = GetLog(triggerParams, transition);
                        uow.MarkAsNew(groupLog);
                        var paymentOrder = new PaymentOrderRepository(uow).FindByGroupID(CatalogGroup.ID);
                        if (paymentOrder != null)
                        {
                            paymentOrder.State = (enPaymentOrderState)Enum.Parse(typeof(enPaymentOrderState), transition.Destination.GetValue().ToString());
                            paymentOrder.Comments = triggerParams.Comments;
                        }

                        paymentOrder = UpdateOfficeSlipNumber(triggerParams, paymentOrder);
                        groupLog.OfficeSlipNumber = paymentOrder.OfficeSlipNumber;
                        groupLog.OfficeSlipDate = paymentOrder.OfficeSlipDate;
                        CatalogGroup.State = transition.Destination;
                    });
        }

        private PaymentOrder UpdateOfficeSlipNumber(CatalogGroupTriggerParams triggerParams, PaymentOrder paymentOrder)
        {
            CatalogGroup.PhaseReference.EnsureLoad();
            var maxOfficeSlipNumber = new PaymentOrderRepository(triggerParams.UnitOfWork).FindMaxOfficeSlipNumber(CatalogGroup.Phase.Year);
            maxOfficeSlipNumber++;

            // if paymentOrder does not exist for this group, create it
            if (paymentOrder == null)
            {
                var newPaymentOrder = new PaymentOrder()
                {
                    GroupID = CatalogGroup.ID,
                    IsActive = true,
                    OfficeSlipDate = triggerParams.SentToYDEDate,
                    OfficeSlipNumber = Convert.ToInt32(CatalogGroup.Phase.Year.ToString() + maxOfficeSlipNumber.ToString("000000"))
                };

                triggerParams.UnitOfWork.MarkAsNew(newPaymentOrder);
                return newPaymentOrder;
            }
            else // if paymentOrder exists for the group, update its data
            {
                paymentOrder.OfficeSlipDate = triggerParams.SentToYDEDate;
                paymentOrder.OfficeSlipNumber = Convert.ToInt32(CatalogGroup.Phase.Year.ToString() + maxOfficeSlipNumber.ToString("000000"));
                paymentOrder.IsActive = true;
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
