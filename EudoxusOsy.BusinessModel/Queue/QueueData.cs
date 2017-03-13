using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public class EntityQueueData
    {
        public int EntityID { get; set; }
        public Guid EntityGID { get; set; }
    }

    public enum enServerSyncQueueAction
    {
        InvalidateCookie
    }

    public class ServerSyncQueueData
    {
        public enServerSyncQueueAction QueueAction { get; set; }

        public List<int> ModifiedEntitlementIDs { get; set; }
        public int EntitlementID { get; set; }
        public int ReporterID { get; set; }
        public int EntityID { get; set; }

        public string Username { get; set; }
    }
}
