using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public interface IDeliverableItem
    {
        int? ReporterID { get; }
        int? EntityID { get; }
        string Message { get; }
        string RecipientAddress { get; }
        enDeliveryStatus DeliveryStatus { get; set; }
        DateTime? SentAt { get; set; }
        DateTime? LastAttemptAt { get; set; }
    }
}
