using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace EudoxusOsy.Utils.Worker
{
    internal class AsyncWorkerItem
    {
        #region [ Constructors ]

        internal AsyncWorkerItem(Action action)
        {
            Delegate = action;
            CancellationSource = new CancellationTokenSource();
            ProgressMonitor = new Progress<int>();
            State = enWorkerItemMonitorState.Pending;
            StartedAt = null;
        }

        internal AsyncWorkerItem(Action<CancellationToken> action)
        {
            CancelableDelegate = action;
            CancellationSource = new CancellationTokenSource();
            ProgressMonitor = new Progress<int>();
            State = enWorkerItemMonitorState.Pending;
            StartedAt = null;
        }

        internal AsyncWorkerItem(Action<IProgress<int>> action)
        {
            DelegateWithProgess = action;
            CancellationSource = new CancellationTokenSource();
            ProgressMonitor = new Progress<int>();
            State = enWorkerItemMonitorState.Pending;
            StartedAt = null;
        }

        internal AsyncWorkerItem(Action<CancellationToken, IProgress<int>> action)
        {
            CancelableDelegateWithProgess = action;
            CancellationSource = new CancellationTokenSource();
            ProgressMonitor = new Progress<int>();
            State = enWorkerItemMonitorState.Pending;
            StartedAt = null;
        }

        #endregion

        #region [ Methods ]

        internal Task GetTask()
        {
            CancellationSource = new CancellationTokenSource();

            Task t = null;
            if (Delegate != null)
                t = new Task(Delegate, CancellationSource.Token);
            else if (CancelableDelegate != null)
                t = new Task(() => CancelableDelegate(CancellationSource.Token), CancellationSource.Token);
            else if (DelegateWithProgess != null)
                t = new Task(() => DelegateWithProgess(ProgressMonitor), CancellationSource.Token);
            else if (CancelableDelegateWithProgess != null)
                t = new Task(() => CancelableDelegateWithProgess(CancellationSource.Token, ProgressMonitor), CancellationSource.Token);

            return t;
        }

        #endregion

        #region [ Fields ]

        internal enWorkerItemMonitorState State { get; set; }
        internal CancellationTokenSource CancellationSource { get; set; }
        internal Progress<int> ProgressMonitor { get; set; }
        internal int LastReportedProgress { get; set; }

        internal Action Delegate { get; private set; }
        internal Action<CancellationToken> CancelableDelegate { get; private set; }
        internal Action<IProgress<int>> DelegateWithProgess { get; private set; }
        internal Action<CancellationToken, IProgress<int>> CancelableDelegateWithProgess { get; private set; }

        internal DateTime? StartedAt { get; set; }
        internal DateTime? LastRunTime { get; set; }

        internal Exception LastException { get; set; }

        #endregion
    }
}