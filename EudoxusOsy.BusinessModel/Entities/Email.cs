using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public partial class Email : IDeliverableItem
    {
        public enEmailType Type
        {
            get { return (enEmailType)TypeInt; }
            set
            {
                int intValue = (int)value;
                if (TypeInt != intValue)
                    TypeInt = intValue;
            }
        }

        public enEmailEntityType EmailEntityType
        {
            get { return (enEmailEntityType)EmailEntityTypeInt; }
            set
            {
                int intValue = (int)value;
                if (EmailEntityTypeInt != intValue)
                    EmailEntityTypeInt = intValue;
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
            get { return Body; }
        }

        string IDeliverableItem.RecipientAddress
        {
            get { return EmailAddress; }
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
