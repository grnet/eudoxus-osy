using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EudoxusOsy.Utils;
using EudoxusOsy.Utils.Queue;
using Imis.Domain;

namespace EudoxusOsy.BusinessModel
{
    public class SMSQueueWorker : QueueWorkerBase<EntityQueueData>, IQueueWorker
    {
        #region [ Thread-safe, lazy Singleton ]

        /// <summary>
        /// This is a thread-safe, lazy singleton.  See http://www.yoda.arachsys.com/csharp/singleton.html
        /// for more details about its implementation.
        /// </summary>
        public static SMSQueueWorker Current
        {
            get
            {
                return Nested.dispatcher;
            }
        }

        /// <summary>
        /// Assists with ensuring thread-safe, lazy singleton
        /// </summary>
        class Nested
        {
            static Nested() { }
            internal static readonly SMSQueueWorker dispatcher = new SMSQueueWorker();
        }

        #endregion

        public override string Name { get { return "SmsQueueWorker"; } }

        public override enQueueEntryType QueueEntryType { get { return enQueueEntryType.DispatchSms; } }

        #region [ Add Helpers ]

        public void AddSmsDispatchToQueue(int smsID, QueueEntrySettings settings = null)
        {
            AddSmsDispatchToQueue(new List<int>() { smsID }, settings);
        }

        public void AddSmsDispatchToQueue(IEnumerable<int> smsIDs, QueueEntrySettings settings = null)
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                AddSmsDispatchToQueue(uow, smsIDs, settings);
            }
        }

        public void AddSmsDispatchToQueue(IUnitOfWork uow, IEnumerable<int> smsIDs, QueueEntrySettings settings = null)
        {
            foreach (var item in smsIDs)
            {
                var entry = GetQueueEntry(new EntityQueueData() { EntityID = item }, settings);
                uow.MarkAsNew(entry);
            }
            uow.Commit();
        }

        #endregion

        #region [ Process Queue Item ]

        protected override void ProcessEntry(IUnitOfWork uow, EntityQueueData queueData, bool lastAttempt)
        {
            var sms = new SmsRepository(uow).Load(queueData.EntityID);
            try
            {
                DispatcherFactory.GetSmsDispatcher().Send(sms.Msg, sms.SendID, sms.ReporterNumber, sms.FieldValues);
                sms.DeliveryStatus = enDeliveryStatus.Successful;
                sms.SentAt = sms.LastAttemptAt = DateTime.Now;
            }
            catch (Exception)
            {
                sms.LastAttemptAt = DateTime.Now;
                if (lastAttempt)
                    sms.DeliveryStatus = enDeliveryStatus.Failed;
                throw;
            }
            uow.Commit();
        }

        #endregion
    }
}
