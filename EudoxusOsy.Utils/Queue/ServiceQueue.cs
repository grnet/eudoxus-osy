using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Caching;
using Imis.Web.Utils;
using log4net;

namespace EudoxusOsy.Utils.Queue
{
    public class ServiceQueue : AsyncRunner
    {
        #region [ Thread-safe, lazy Singleton ]

        /// <summary>
        /// This is a thread-safe, lazy singleton.  See http://www.yoda.arachsys.com/csharp/singleton.html
        /// for more details about its implementation.
        /// </summary>
        public static ServiceQueue Instance
        {
            get
            {
                return Nested.dispatcher;
            }
        }

        public QueueConfiguration Config { get { return _config; } }

        private QueueConfiguration _config = null;

        private List<IQueueWorker> _workers = null;

        private ServiceQueue()
        {
            _config = ConfigurationManager.GetSection("queueConfig") as QueueConfiguration;
            _workers = new List<IQueueWorker>();
            this.AsyncRunnerMessage += ServiceQueue_AsyncRunnerMessage;
        }

        private void ServiceQueue_AsyncRunnerMessage(object sender, AsyncRunnerMessageEventArgs e)
        {
            switch (e.Type)
            {
                case enAsyncRunnerMessageType.Error:
                    LogManager.GetLogger("ServiceQueue").Error(e.Message, e.Exception);
                    break;
                case enAsyncRunnerMessageType.Info:
                case enAsyncRunnerMessageType.Warning:
                    LogManager.GetLogger("ServiceQueue").Info(e.Message);
                    break;
            }
        }

        /// <summary>
        /// Assists with ensuring thread-safe, lazy singleton
        /// </summary>
        class Nested
        {
            static Nested() { }
            internal static readonly ServiceQueue dispatcher = new ServiceQueue();
        }

        #endregion

        #region [ Methods ]

        public void AddWorker(IQueueWorker worker)
        {
            _workers.Add(worker);
        }

        public void Initialize()
        {
            base.Initialize(TimeSpan.FromSeconds(_config.ProcessQueueInterval), _config.ProcessQueueOnInitialize);
        }

        public override void Process()
        {
            if (string.IsNullOrWhiteSpace(_config.MachineName) || _config.MachineName.ToLower() == Environment.MachineName.ToLower())
            {
                foreach (var worker in _workers)
                {
                    ThreadPool.QueueUserWorkItem((x) =>
                    {
                        try
                        {
                            int itemsProcessed = worker.ProcessQueue();
                            //LogHelper.LogMessage(string.Format("Queue processed at {0} on {1}. {2} Items Processed on worker: {3}", DateTime.Now.ToString(), Environment.MachineName, itemsProcessed.ToString(), worker.Name), this);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.LogError(ex, this, string.Format("Failed to process queue. Queue Worker: {0}", worker.Name));
                        }
                    });
                }
            }
        }

        #endregion
    }
}
