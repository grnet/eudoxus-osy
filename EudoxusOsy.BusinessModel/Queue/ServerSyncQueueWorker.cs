using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EudoxusOsy.Utils.Queue;
using Imis.Domain;

namespace EudoxusOsy.BusinessModel
{
    public class ServerSyncQueueWorker : QueueWorkerBase<ServerSyncQueueData>, IQueueWorker
    {
        #region [ Thread-safe, lazy Singleton ]

        /// <summary>
        /// This is a thread-safe, lazy singleton.  See http://www.yoda.arachsys.com/csharp/singleton.html
        /// for more details about its implementation.
        /// </summary>
        public static ServerSyncQueueWorker Current
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
            internal static readonly ServerSyncQueueWorker dispatcher = new ServerSyncQueueWorker();
        }

        #endregion

        public override string Name { get { return "ServerSyncQueueWorker"; } }

        public override enQueueEntryType QueueEntryType { get { return enQueueEntryType.ServerSync; } }

        #region [ Add Helpers ]

        public void AddInvalidateCookieSyncToQueue(string username, QueueEntrySettings settings = null)
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                AddInvalidateCookieSyncToQueue(uow, username, settings);
            }
        }

        public void AddInvalidateCookieSyncToQueue(IUnitOfWork uow, string username, QueueEntrySettings settings = null)
        {
            var queueData = new ServerSyncQueueData() { QueueAction = enServerSyncQueueAction.InvalidateCookie, Username = username };
            var entry = GetQueueEntry(queueData, settings);
            uow.MarkAsNew(entry);
            uow.Commit();
        }

        #endregion

        #region [ Process Queue Item ]

        protected override void ProcessEntry(IUnitOfWork uow, ServerSyncQueueData queueData, bool lastAttempt)
        {
            var syncSrv = new ServerSyncService();
            switch (queueData.QueueAction)
            {
                case enServerSyncQueueAction.InvalidateCookie:
                    syncSrv.SyncInvalidateCookie(queueData.Username);
                    break;
            }
        }

        #endregion
    }
}
