using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.Utils.Worker
{
    public class WorkerItemMonitor
    {
        public string Name { get; set; }
        public enWorkerItemMonitorState State { get; set; }
        public bool IsCancellationRequested { get; set; }
        public int Progress { get; set; }

        public DateTime? StartedAt { get; set; }
        public DateTime? LastRunTime { get; set; }

        public Exception LastException { get; set; }

        public enWorkerItemRunType? RunType { get; set; }
        public TimeSpan RunAt { get; set; }
        public int RunInterval { get; set; }

    }
}
