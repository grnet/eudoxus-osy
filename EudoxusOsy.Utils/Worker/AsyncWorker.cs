using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Imis.Web.Utils;
using log4net;

namespace EudoxusOsy.Utils.Worker
{
    public class AsyncWorkerItemProcessedEventArgs : EventArgs
    {
        public string TaskName { get; set; }

        public DateTime ProcessedAt { get; set; }
    }

    public class AsyncWorkerItemProgressChangedEventArgs : EventArgs
    {
        public string TaskName { get; set; }

        public int Progress { get; set; }
    }

    public class AsyncWorker : AsyncRunner
    {
        #region [ Thread-safe, lazy Singleton ]

        public static AsyncWorker Instance
        {
            get
            {
                return Nested.asyncWorker;
            }
        }

        private WorkerConfiguration _config = null;

        private Dictionary<string, AsyncWorkerItem> _items = null;

        private AsyncWorker()
        {
            _config = ConfigurationManager.GetSection("asyncWorker") as WorkerConfiguration;
            _items = new Dictionary<string, AsyncWorkerItem>();
            this.AsyncRunnerMessage += AsyncWorker_AsyncRunnerMessage;
        }

        private void AsyncWorker_AsyncRunnerMessage(object sender, AsyncRunnerMessageEventArgs e)
        {
            switch (e.Type)
            {
                case enAsyncRunnerMessageType.Error:
                    LogManager.GetLogger("AsyncWorker").Error(e.Message, e.Exception);
                    break;
                case enAsyncRunnerMessageType.Info:
                case enAsyncRunnerMessageType.Warning:
                    LogManager.GetLogger("AsyncWorker").Info(e.Message);
                    break;
            }
        }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly AsyncWorker asyncWorker = new AsyncWorker();
        }

        #endregion

        #region [ Events ]

        public event EventHandler<AsyncWorkerItemProcessedEventArgs> AsyncWorkerItemProcessed;
        public void FireAsyncWorkerItemProcessedEvent(string name, DateTime processedAt)
        {
            var handler = AsyncWorkerItemProcessed;
            if (handler != null)
                handler(this, new AsyncWorkerItemProcessedEventArgs() { TaskName = name, ProcessedAt = processedAt });
        }

        public event EventHandler<AsyncWorkerItemProgressChangedEventArgs> AsyncWorkerItemProgressChanged;
        public void FireAsyncWorkerItemProgressChangedEvent(string name, int progress)
        {
            var handler = AsyncWorkerItemProgressChanged;
            if (handler != null)
                handler(this, new AsyncWorkerItemProgressChangedEventArgs() { TaskName = name, Progress = progress });
        }

        #endregion [ Events ]

        #region [ Register Helpers ]

        public void Register(string name, DateTime? lastRunTime, Action action)
        {
            if (_items.ContainsKey(name))
                throw new InvalidOperationException(string.Format("Task '{0}' already exists in async worker.", name));

            _items[name] = new AsyncWorkerItem(action) { LastRunTime = lastRunTime };
            _items[name].ProgressMonitor.ProgressChanged += (s, e) =>
            {
                _items[name].LastReportedProgress = e;
                FireAsyncWorkerItemProgressChangedEvent(name, e);
            };
        }

        public void Register(string name, DateTime? lastRunTime, Action<CancellationToken> action)
        {
            if (_items.ContainsKey(name))
                throw new InvalidOperationException(string.Format("Task '{0}' already exists in async worker.", name));

            _items[name] = new AsyncWorkerItem(action) { LastRunTime = lastRunTime };
            _items[name].ProgressMonitor.ProgressChanged += (s, e) =>
            {
                _items[name].LastReportedProgress = e;
                FireAsyncWorkerItemProgressChangedEvent(name, e);
            };
        }

        public void Register(string name, DateTime? lastRunTime, Action<IProgress<int>> action)
        {
            if (_items.ContainsKey(name))
                throw new InvalidOperationException(string.Format("Task '{0}' already exists in async worker.", name));

            _items[name] = new AsyncWorkerItem(action) { LastRunTime = lastRunTime };
            _items[name].ProgressMonitor.ProgressChanged += (s, e) =>
            {
                _items[name].LastReportedProgress = e;
                FireAsyncWorkerItemProgressChangedEvent(name, e);
            };
        }

        public void Register(string name, DateTime? lastRunTime, Action<CancellationToken, IProgress<int>> action)
        {
            if (_items.ContainsKey(name))
                throw new InvalidOperationException(string.Format("Task '{0}' already exists in async worker.", name));

            _items[name] = new AsyncWorkerItem(action) { LastRunTime = lastRunTime };
            _items[name].ProgressMonitor.ProgressChanged += (s, e) =>
            {
                _items[name].LastReportedProgress = e;
                FireAsyncWorkerItemProgressChangedEvent(name, e);
            };
        }

        #endregion

        #region [ Helpers ]

        public void Start(string task)
        {
            if (!_items.ContainsKey(task))
                return;

            var item = _items[task];
            if (item.State == enWorkerItemMonitorState.Pending)
                RunItem(task, item);
        }

        public void RequestCancel(string task)
        {
            if (!_items.ContainsKey(task))
                return;

            var item = _items[task];
            if (item.State == enWorkerItemMonitorState.Running)
                item.CancellationSource.Cancel();
        }

        public List<WorkerItemMonitor> MonitorTasks()
        {
            var result = new List<WorkerItemMonitor>();

            foreach (var item in _items)
            {
                var monitor = new WorkerItemMonitor();
                monitor.Name = item.Key;
                monitor.State = item.Value.State;
                monitor.IsCancellationRequested = item.Value.CancellationSource.IsCancellationRequested;
                monitor.Progress = item.Value.LastReportedProgress;
                monitor.StartedAt = item.Value.StartedAt;
                monitor.LastRunTime = item.Value.LastRunTime;
                monitor.LastException = item.Value.LastException;

                var workerItem = _config.WorkerItems.OfType<WorkerItem>().FirstOrDefault(x => x.Name == item.Key);
                if (workerItem != null)
                {
                    monitor.RunType = workerItem.RunType;
                    monitor.RunInterval = workerItem.RunInterval;
                    monitor.RunAt = workerItem.RunAt;
                }

                result.Add(monitor);
            }

            return result;
        }

        #endregion

        #region [ Process ]

        public void Initialize()
        {
            if (!_config.Enabled)
                return;

            if (_items.Count == 0)
                return;

            base.Initialize(TimeSpan.FromSeconds(_config.RunInterval), _config.ProcessQueueOnInitialize);
        }

        public override void Process()
        {
            DateTime processingStartedAt = DateTime.Now;
            foreach (var i in _items)
                ProcessItem(i, processingStartedAt);
        }

        private void ProcessItem(KeyValuePair<string, AsyncWorkerItem> item, DateTime? processingStartedAt = null)
        {
            if (string.IsNullOrEmpty(_config.MachineName) || _config.MachineName.ToLower() == Environment.MachineName.ToLower())
            {
                if (!processingStartedAt.HasValue)
                    processingStartedAt = DateTime.Now;

                var workerItem = _config.WorkerItems.OfType<WorkerItem>().FirstOrDefault(x => x.Name == item.Key);
                if (workerItem != null)
                {
                    var lastRunTime = item.Value.LastRunTime;
                    if (workerItem.RunType == enWorkerItemRunType.Recurrent)
                    {
                        if (!lastRunTime.HasValue || (processingStartedAt.Value - lastRunTime.Value).TotalSeconds > (double)workerItem.RunInterval)
                            RunItem(item.Key, item.Value);
                    }
                    else if (workerItem.RunType == enWorkerItemRunType.Daily)
                    {
                        //if (processingStartedAt.Value.TimeOfDay > workerItem.RunAt && (!lastRunTime.HasValue || (processingStartedAt.Value - lastRunTime.Value).TotalHours >= 23))
                        //    RunItem(item.Key, item.Value);

                        if (processingStartedAt.Value.TimeOfDay > workerItem.RunAt && (!lastRunTime.HasValue || processingStartedAt.Value.Date != lastRunTime.Value.Date))
                            RunItem(item.Key, item.Value);
                    }
                }
            }
        }

        private async void RunItem(string name, AsyncWorkerItem item)
        {
            if (item.State == enWorkerItemMonitorState.Running)
                return;

            var t = item.GetTask();
            if (t == null)
                return;

            try
            {
                LogHelper.LogMessage(string.Format("Starting task: {0}.", name), this);
                item.State = enWorkerItemMonitorState.Running;
                item.StartedAt = DateTime.Now;

                //(item.ProgressMonitor as IProgress<int>).Report(0);
                t.Start();
                await t;

                item.LastException = null;

                if (!item.CancellationSource.IsCancellationRequested)
                {
                    item.LastRunTime = DateTime.Now;
                    FireAsyncWorkerItemProcessedEvent(name, DateTime.Now);
                    LogHelper.LogMessage(string.Format("Task {0} runned successfully.", name), this);
                }
                else
                {
                    LogHelper.LogMessage(string.Format("Task {0} was cancelled.", name), this);
                }
            }
            catch (Exception ex)
            {
                //item.LastRunTime = DateTime.Now;
                item.LastException = ex;
                LogHelper.LogError(ex, this, string.Format("Task {0} failed to run.", name));
            }
            finally
            {
                item.State = enWorkerItemMonitorState.Pending;
                item.StartedAt = null;
            }
        }

        #endregion
    }
}