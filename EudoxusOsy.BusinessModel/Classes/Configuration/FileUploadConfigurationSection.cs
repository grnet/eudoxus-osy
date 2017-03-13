using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace EudoxusOsy.Configuration
{
    public class FileUploadConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("defaultFileSize", IsRequired = true)]
        public int DefaultFileSize
        {
            get { return (int)this["defaultFileSize"]; }
            set { this["defaultFileSize"] = value; }
        }

        [ConfigurationProperty("allowedFileExtensions", IsRequired = true)]
        public string AllowedFileExtensions
        {
            get { return (string)this["allowedFileExtensions"]; }
            set { this["allowedFileExtensions"] = string.Join(", ", value); }
        }

        [ConfigurationProperty("uploadPath", IsRequired = true)]
        public string UploadPath
        {
            get
            {
                var path = (string)this["uploadPath"];
                if (path.StartsWith("~"))
                    return HostingEnvironment.MapPath(path);
                else
                    return path;
            }
            set { this["uploadPath"] = value; }
        }

        [ConfigurationProperty("downloadUrl", IsRequired = true)]
        public string DownloadUrl
        {
            get { return (string)this["downloadUrl"]; }
            set { this["downloadUrl"] = value; }
        }

        [ConfigurationProperty("exceptions", IsRequired = false)]
        public FileUploadExceptionCollection Exceptions
        {
            get { return (FileUploadExceptionCollection)this["exceptions"]; }
            set { this["exceptions"] = value; }
        }
    }

    [ConfigurationCollection(typeof(FileUploadExceptionConfigurationSection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class FileUploadExceptionCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FileUploadExceptionConfigurationSection();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return ((FileUploadExceptionConfigurationSection)element).Username;
        }
    }

    public class FileUploadExceptionConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("username", IsRequired = true)]
        public string Username
        {
            get { return (string)this["username"]; }
            set { this["username"] = value; }
        }

        [ConfigurationProperty("fileSize", IsRequired = true)]
        public int FileSize
        {
            get { return (int)this["fileSize"]; }
            set { this["fileSize"] = value; }
        }
    }
}
