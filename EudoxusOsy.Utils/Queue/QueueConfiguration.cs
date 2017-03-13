using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace EudoxusOsy.Utils.Queue
{
    public class QueueConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("maxNoOfRetries", DefaultValue = 5, IsRequired = false)]
        public int MaxNoOfRetries
        {
            get { return (int)this["maxNoOfRetries"]; }
            set { this["maxNoOfRetries"] = value; }
        }

        [ConfigurationProperty("retryInterval", DefaultValue = 0, IsRequired = false)]
        public int RetryInterval
        {
            get { return (int)this["retryInterval"]; }
            set { this["retryInterval"] = value; }
        }

        [ConfigurationProperty("processQueueInterval", DefaultValue = 60, IsRequired = false)]
        public int ProcessQueueInterval
        {
            get { return (int)this["processQueueInterval"]; }
            set { this["processQueueInterval"] = value; }
        }

        [ConfigurationProperty("maxBatchSize", DefaultValue = 20, IsRequired = false)]
        public int MaxBatchSize
        {
            get { return (int)this["maxBatchSize"]; }
            set { this["maxBatchSize"] = value; }
        }

        [ConfigurationProperty("processQueueOnInitialize", DefaultValue = false, IsRequired = false)]
        public bool ProcessQueueOnInitialize
        {
            get { return (bool)this["processQueueOnInitialize"]; }
            set { this["processQueueOnInitialize"] = value; }
        }

        [ConfigurationProperty("continuallyProcessBatches", DefaultValue = true, IsRequired = false)]
        public bool ContinuallyProcessBatches
        {
            get { return (bool)this["continuallyProcessBatches"]; }
            set { this["continuallyProcessBatches"] = value; }
        }

        [ConfigurationProperty("machineName", DefaultValue = "", IsRequired = false)]
        public string MachineName
        {
            get { return (string)this["machineName"]; }
            set { this["machineName"] = value; }
        }
    }
}
