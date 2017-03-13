using System;
using System.Configuration;

namespace EudoxusOsy.Configuration
{
    public class CachingConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("initializeCacheOnStart", IsRequired = false, DefaultValue = true)]
        public bool InitializeCacheOnStart
        {
            get { return (bool)this["initializeCacheOnStart"]; }
            set { this["initializeCacheOnStart"] = value; }
        }

        [ConfigurationProperty("initializeCacheDependenciesOnStart", IsRequired = false, DefaultValue = true)]
        public bool InitializeCacheDependenciesOnStart
        {
            get { return (bool)this["initializeCacheDependenciesOnStart"]; }
            set { this["initializeCacheDependenciesOnStart"] = value; }
        }

        [ConfigurationProperty("monitoredTables", IsRequired = true)]
        public MonitoredTablesCollection MonitoredTables
        {
            get { return (MonitoredTablesCollection)this["monitoredTables"]; }
            set { this["monitoredTables"] = value; }
        }
    }

    [ConfigurationCollection(typeof(MonitoredTableConfigurationSection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class MonitoredTablesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MonitoredTableConfigurationSection();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return ((MonitoredTableConfigurationSection)element).Key;
        }

        [ConfigurationProperty("dbName", IsRequired = true)]
        public string DatabaseName
        {
            get { return (string)this["dbName"]; }
            set { this["dbName"] = value; }
        }
    }

    public class MonitoredTableConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("tableName", IsRequired = true)]
        public string TableName
        {
            get { return (string)this["tableName"]; }
            set { this["tableName"] = value; }
        }

        [ConfigurationProperty("cacheManager", IsRequired = true)]
        public string CacheManager
        {
            get { return (string)this["cacheManager"]; }
            set { this["cacheManager"] = value; }
        }
    }
}