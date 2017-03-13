using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace EudoxusOsy.Utils.Worker
{
    public class WorkerConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("enabled", DefaultValue = false, IsRequired = false)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;

            }
        }

        [ConfigurationProperty("processQueueOnInitialize", DefaultValue = false, IsRequired = false)]
        public bool ProcessQueueOnInitialize
        {
            get
            {
                return (bool)this["processQueueOnInitialize"];
            }
            set
            {
                this["processQueueOnInitialize"] = value;

            }
        }

        [ConfigurationProperty("runInterval", DefaultValue = 300, IsRequired = false)]
        public int RunInterval
        {
            get { return (int)this["runInterval"]; }
            set { this["runInterval"] = value; }
        }

        [ConfigurationProperty("machineName", DefaultValue = "", IsRequired = false)]
        public string MachineName
        {
            get { return (string)this["machineName"]; }
            set { this["machineName"] = value; }
        }

        [ConfigurationProperty("workerItems", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(WorkerItemCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public WorkerItemCollection WorkerItems
        {
            get
            {
                WorkerItemCollection urlsCollection = (WorkerItemCollection)base["workerItems"];
                return urlsCollection;

            }
        }
    }

    public class WorkerItemCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new WorkerItem();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((WorkerItem)element).Name;
        }
    }

    public enum enWorkerItemRunType
    {
        Daily,
        Recurrent
    }

    public enum enWorkerItemStatus
    {
        Idle = 0,
        Running = 1
    }

    public class WorkerItem : ConfigurationSection
    {
        [ConfigurationProperty("name", DefaultValue = "", IsRequired = false)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("runType", IsRequired = true)]
        public enWorkerItemRunType RunType
        {
            get { return (enWorkerItemRunType)Enum.Parse(typeof(enWorkerItemRunType), this["runType"].ToString()); }
            set { this["runType"] = value.ToString(); }
        }

        [ConfigurationProperty("runAt", IsRequired = false)]
        [TimeSpanValidator(MinValueString = "00:00:00", MaxValueString = "23:59:59", ExcludeRange = false)]
        public TimeSpan RunAt
        {
            get { return (TimeSpan)this["runAt"]; }
            set { this["runAt"] = value; }
        }

        [ConfigurationProperty("runInterval", IsRequired = false)]
        public int RunInterval
        {
            get { return (int)this["runInterval"]; }
            set { this["runInterval"] = value; }
        }
    }
}