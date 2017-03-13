using System;
using System.Collections.Generic;
using Imis.Domain;
using EudoxusOsy.Utils;
using EudoxusOsy.Utils.Queue;

namespace EudoxusOsy.BusinessModel
{
    public abstract class QueueWorkerBase<T> : IQueueWorker
    {
        public virtual void AddToQueue(IQueueEntry entry)
        {
            throw new NotImplementedException();
        }

        public virtual void RemoveFromQueue(IQueueEntry entry)
        {
            throw new NotImplementedException();
            //using (var uow = UnitOfWorkFactory.Create())
            //{
            //    var e = new QueueEntryRepository(uow).FindByDataAndType((int)entry.QueueData, entry.QueueEntryType);
            //    if (e != null)
            //    {
            //        uow.MarkAsDeleted(e);
            //        uow.Commit();
            //    }
            //}
        }

        public abstract string Name { get; }

        public abstract enQueueEntryType QueueEntryType { get; }

        #region [ Proccess ]

        public int ProcessQueue()
        {
            int totalItemsProcessed = 0;

            if (ServiceQueue.Instance.Config.ContinuallyProcessBatches)
            {
                bool moreRemaining = false;
                do
                {
                    int itemsProcessed = ProcessBatch();
                    totalItemsProcessed += itemsProcessed;
                    moreRemaining = itemsProcessed == ServiceQueue.Instance.Config.MaxBatchSize;
                }
                while (moreRemaining);
            }
            else
            {
                totalItemsProcessed = ProcessBatch();
            }

            return totalItemsProcessed;
        }

        private int ProcessBatch()
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                List<QueueEntry> entries = new List<QueueEntry>();
                if (new QueueEntryRepository(uow).TryGetPendingEntries(ServiceQueue.Instance.Config.MaxBatchSize, out entries, QueueEntryType))
                {
                    foreach (var item in entries)
                        ProcessQueueEntry(uow, item);

                    return entries.Count;
                }
                return 0;
            }
        }

        private void ProcessQueueEntry(IUnitOfWork uow, QueueEntry entry)
        {
            if (entry.NumberOfRetries > entry.MaxNumberOfRetries.Value)
            {
                var ex = new Exception(string.Format("Queue Entry failed to run for {0} times.", entry.NumberOfRetries));
                LogHelper.LogError(ex, this, string.Format("QueueRetryData:\n {0}", entry.RetryDataXml));

                entry.QueueEntryStatus = enQueueEntryStatus.Failed;
                uow.Commit();
                return;
            }

            if (entry.RetryInterval.Value != 0 && entry.LastAttemptAt.HasValue && (DateTime.Now - entry.LastAttemptAt.Value).TotalSeconds < entry.RetryInterval.Value)
                return;

            try
            {
                var queueData = new Serializer<T>().Deserialize(entry.QueueDataXml);
                ProcessEntry(uow, queueData, entry.NumberOfRetries == entry.MaxNumberOfRetries.Value);
                uow.MarkAsDeleted(entry);
            }
            catch (Exception ex)
            {
                entry.NumberOfRetries++;
                entry.LastAttemptAt = DateTime.Now;
                var data = entry.QueueRetryData;
                data.Add(new QueueRetryData
                {
                    RetryNumber = entry.NumberOfRetries,
                    Message = ex.Message,
                    ServerName = Environment.MachineName
                });
                entry.QueueRetryData = data;
            }

            uow.Commit();
        }

        protected abstract void ProcessEntry(IUnitOfWork uow, T queueData, bool lastAttempt);

        #endregion

        #region [ Helpers ]

        protected QueueEntry GetQueueEntry(T queueData, QueueEntrySettings settings)
        {
            if (settings == null)
                settings = new QueueEntrySettings();

            return new QueueEntry()
            {
                NumberOfRetries = 0,
                MaxNumberOfRetries = settings.MaxNumberOfRetries.HasValue ? settings.MaxNumberOfRetries.Value : ServiceQueue.Instance.Config.MaxNoOfRetries,
                QueueDataXml = new Serializer<T>().Serialize(queueData),
                QueueEntryType = QueueEntryType,
                QueueEntryStatus = enQueueEntryStatus.Running,
                QueueEntryPriority = settings.Priority.HasValue ? settings.Priority.Value : enQueueEntryPriority.Normal,
                QueueRetryData = new QueueRetryDataCollection(),
                RetryInterval = settings.RetryInterval.HasValue ? settings.RetryInterval.Value : ServiceQueue.Instance.Config.RetryInterval
            };
        }

        #endregion
    }

}
