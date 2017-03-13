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
    public class EmailQueueWorker : QueueWorkerBase<EntityQueueData>, IQueueWorker
    {
        #region [ Thread-safe, lazy Singleton ]

        /// <summary>
        /// This is a thread-safe, lazy singleton.  See http://www.yoda.arachsys.com/csharp/singleton.html
        /// for more details about its implementation.
        /// </summary>
        public static EmailQueueWorker Current
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
            internal static readonly EmailQueueWorker dispatcher = new EmailQueueWorker();
        }

        #endregion

        public override string Name { get { return "EmailQueueWorker"; } }

        public override enQueueEntryType QueueEntryType { get { return enQueueEntryType.DispatchEmail; } }

        #region [ Add Helpers ]

        public void AddEmailDispatchToQueue(int emailID, QueueEntrySettings settings = null)
        {
            AddEmailDispatchToQueue(new List<int>() { emailID }, settings);
        }

        public void AddEmailDispatchToQueue(IEnumerable<int> emailIDs, QueueEntrySettings settings = null)
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                AddEmailDispatchToQueue(uow, emailIDs, settings);
            }
        }

        public void AddEmailDispatchToQueue(IUnitOfWork uow, IEnumerable<int> emailIDs, QueueEntrySettings settings = null)
        {
            foreach (var item in emailIDs)
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
            var email = new EmailRepository(uow).Load(queueData.EntityID);
            try
            {
                if (!string.IsNullOrEmpty(email.CCedEmailAddresses))
                    DispatcherFactory.GetEmailDispatcher().Send(email.SenderEmailAddress, email.EmailAddress, email.CCedEmailAddresses.Split(';'), email.Subject, email.Body, email.Type.IsHtmlEmail());
                else
                    DispatcherFactory.GetEmailDispatcher().Send(email.SenderEmailAddress, email.EmailAddress, email.Subject, email.Body, email.Type.IsHtmlEmail());

                email.DeliveryStatus = enDeliveryStatus.Successful;
                email.SentAt = email.LastAttemptAt = DateTime.Now;
            }
            catch (Exception)
            {
                email.LastAttemptAt = DateTime.Now;
                if (lastAttempt)
                    email.DeliveryStatus = enDeliveryStatus.Failed;
                throw;
            }
            uow.Commit();
        }

        #endregion
    }
}
