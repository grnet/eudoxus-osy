using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.Utils.Queue
{
    public class QueueRetryDataCollection : List<QueueRetryData>
    {
        public QueueRetryDataCollection()
            : base() { }

        public QueueRetryDataCollection(int capacity)
            : base(capacity) { }

        public QueueRetryDataCollection(IEnumerable<QueueRetryData> collection)
            : base(collection) { }
    }

    public class QueueRetryData
    {
        public int RetryNumber { get; set; }
        public string Message { get; set; }
        public string ServerName { get; set; }
    }
}
