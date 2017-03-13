using System;

namespace EudoxusOsy.BusinessModel
{
    public partial class SMS : IDeliverableItem
    {
        public string[] FieldValues
        {
            get { return FieldValuesInternal != null ? FieldValuesInternal.Split(new char[] { '#' }) : null; }
            set
            {
                FieldValuesInternal = null;
                if (value != null)
                    foreach (var str in value)
                        FieldValuesInternal += str + "#";
                if (FieldValuesInternal != null)
                    FieldValuesInternal = FieldValuesInternal.Remove(FieldValuesInternal.Length - 1, 1);
            }
        }

        public enDeliveryStatus DeliveryStatus
        {
            get { return (enDeliveryStatus)DeliveryStatusInt; }
            set
            {
                int intValue = (int)value;
                if (DeliveryStatusInt != intValue)
                    DeliveryStatusInt = intValue;
            }
        }

        int? IDeliverableItem.ReporterID
        {
            get { return ReporterID; }
        }

        int? IDeliverableItem.EntityID
        {
            get { return EntityID; }
        }

        string IDeliverableItem.Message
        {
            get { return Msg; }
        }

        string IDeliverableItem.RecipientAddress
        {
            get { return ReporterNumber; }
        }

        enDeliveryStatus IDeliverableItem.DeliveryStatus
        {
            get { return DeliveryStatus; }
            set { DeliveryStatus = value; }
        }

        DateTime? IDeliverableItem.SentAt
        {
            get { return SentAt; }
            set { SentAt = value; }
        }

        DateTime? IDeliverableItem.LastAttemptAt
        {
            get { return LastAttemptAt; }
            set { LastAttemptAt = value; }
        }
    }
}