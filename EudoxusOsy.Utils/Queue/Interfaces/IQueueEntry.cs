using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EudoxusOsy.Utils.Queue
{
    public interface IQueueEntry
    {
        int QueueEntryType { get; set; }
        int QueueEntryStatus { get; set; }
        string QueueDataXml { get; set; }

        int? MaxNumberOfRetries { get; set; }
        int NumberOfRetries { get; set; }
        int? RetryInterval { get; set; }         //Seconds

        DateTime? LastAttemptAt { get; set; }

        QueueRetryDataCollection RetryData { get; set; }
    }
}
