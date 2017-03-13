using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EudoxusOsy.Utils.Queue;

namespace EudoxusOsy.BusinessModel
{
    public partial class QueueEntry : IQueueEntry
    {
        public enQueueEntryType QueueEntryType
        {
            get { return (enQueueEntryType)QueueEntryTypeInt; }
            set
            {
                int intValue = (int)value;
                if (QueueEntryTypeInt != intValue)
                    QueueEntryTypeInt = intValue;
            }
        }

        public enQueueEntryStatus QueueEntryStatus
        {
            get { return (enQueueEntryStatus)QueueEntryStatusInt; }
            set
            {
                int intValue = (int)value;
                if (QueueEntryStatusInt != intValue)
                    QueueEntryStatusInt = intValue;
            }
        }

        public enQueueEntryPriority QueueEntryPriority
        {
            get { return (enQueueEntryPriority)QueueEntryPriorityInt; }
            set
            {
                int intValue = (int)value;
                if (QueueEntryPriorityInt != intValue)
                    QueueEntryPriorityInt = intValue;
            }
        }

        private QueueRetryDataCollection _retryData = null;
        public QueueRetryDataCollection QueueRetryData
        {
            get
            {
                if (_retryData == null)
                {
                    if (!string.IsNullOrEmpty(RetryDataXml))
                        _retryData = new Serializer<QueueRetryDataCollection>().Deserialize(RetryDataXml);
                }
                return _retryData;
            }
            set
            {
                _retryData = value;
                RetryDataXml = new Serializer<QueueRetryDataCollection>().Serialize(value);
            }
        }

        QueueRetryDataCollection IQueueEntry.RetryData
        {
            get { return QueueRetryData; }
            set { QueueRetryData = value; }
        }

        int IQueueEntry.QueueEntryType
        {
            get { return QueueEntryTypeInt; }
            set { QueueEntryTypeInt = value; }
        }

        int IQueueEntry.QueueEntryStatus
        {
            get { return QueueEntryStatusInt; }
            set { QueueEntryStatusInt = value; }
        }

        string IQueueEntry.QueueDataXml
        {
            get { return QueueDataXml; }
            set { QueueDataXml = value; }
        }

        int? IQueueEntry.MaxNumberOfRetries
        {
            get { return MaxNumberOfRetries; }
            set { MaxNumberOfRetries = value; }
        }

        int IQueueEntry.NumberOfRetries
        {
            get { return NumberOfRetries; }
            set { NumberOfRetries = value; }
        }

        int? IQueueEntry.RetryInterval
        {
            get { return RetryInterval; }
            set { RetryInterval = value; }
        }

        DateTime? IQueueEntry.LastAttemptAt
        {
            get { return LastAttemptAt; }
            set { LastAttemptAt = value; }
        }
    }
}
