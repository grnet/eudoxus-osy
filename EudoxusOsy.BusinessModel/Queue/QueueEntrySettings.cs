using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public class QueueEntrySettings
    {
        public int? RetryInterval { get; set; }
        public int? MaxNumberOfRetries { get; set; }
        public enQueueEntryPriority? Priority { get; set; }
    }
}
