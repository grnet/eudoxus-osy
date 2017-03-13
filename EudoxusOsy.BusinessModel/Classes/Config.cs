using EudoxusOsy.Configuration;
using System;
using System.Configuration;

namespace EudoxusOsy.BusinessModel
{
    public static class Config
    {
        private static string _applicationUrl = null;
        public static string ApplicationUrl
        {
            get
            {
                if (_applicationUrl == null)
                    _applicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"];

                return _applicationUrl;
            }
        }

        private static string _portalUrl = null;
        public static string PortalUrl
        {
            get
            {
                if (_portalUrl == null)
                    _portalUrl = ConfigurationManager.AppSettings["PortalUrl"];

                return _portalUrl;
            }
        }

        private static bool? _enableEmail = null;
        public static bool EnableEmail
        {
            get
            {
                if (_enableEmail == null)
                    _enableEmail = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableEmail"]);

                return _enableEmail.Value;
            }
        }

        private static bool? _enableSMS = null;
        public static bool EnableSMS
        {
            get
            {
                if (_enableSMS == null)
                    _enableSMS = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSMS"]);

                return _enableSMS.Value;
            }
        }

        private static int? _maxSMSAllowed = null;
        public static int MaxSMSAllowed
        {
            get
            {
                if (_maxSMSAllowed == null)
                    _maxSMSAllowed = int.Parse(ConfigurationManager.AppSettings["MaxSMSAllowed"]);

                return _maxSMSAllowed.Value;
            }
        }

        private static bool? _isPilotSite = null;
        public static bool IsPilotSite
        {
            get
            {
                if (_isPilotSite == null)
                    _isPilotSite = Convert.ToBoolean(ConfigurationManager.AppSettings["IsPilotSite"]);

                return _isPilotSite.Value;
            }
        }

        private static bool? _isSSL = null;
        public static bool IsSSL
        {
            get
            {
                if (_isSSL == null)
                    _isSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSSL"]);

                return _isSSL.Value;
            }
        }

        private static string _ibanChangesEmailTo = null;
        public static string IbanChangesEmailTo
        {
            get
            {
                if (_ibanChangesEmailTo == null)
                    _ibanChangesEmailTo = ConfigurationManager.AppSettings["IbanChangesEmailTo"];

                return _ibanChangesEmailTo;
            }
        }

        private static string _dailyUpdateReceiptsFromAuditReceiptsEmailTo = null;
        public static string DailyUpdateReceiptsFromAuditReceiptsEmailTo
        {
            get
            {
                if (_dailyUpdateReceiptsFromAuditReceiptsEmailTo == null)
                    _dailyUpdateReceiptsFromAuditReceiptsEmailTo = ConfigurationManager.AppSettings["DailyUpdateReceiptsFromAuditReceiptsEmailTo"];

                return _dailyUpdateReceiptsFromAuditReceiptsEmailTo;
            }
        }


        private static bool? _enableServerSync = null;
        public static bool EnableServerSync
        {
            get
            {
                if (_enableServerSync == null)
                    _enableServerSync = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableServerSync"]);

                return _enableServerSync.Value;
            }
        }

        private static int? _lockWindow = null;
        public static int LockWindow
        {
            get
            {
                if (_lockWindow == null)
                    _lockWindow = int.Parse(ConfigurationManager.AppSettings["LockWindow"]);

                return _lockWindow.Value;
            }
        }

        private static FileUploadConfigurationSection _fileUpload = null;
        public static FileUploadConfigurationSection FileUpload
        {
            get
            {
                if (_fileUpload == null)
                    _fileUpload = (FileUploadConfigurationSection)ConfigurationManager.GetSection("fileUploadConfig");

                return _fileUpload;
            }
        }

        private static CachingConfigurationSection _caching = null;
        public static CachingConfigurationSection Caching
        {
            get
            {
                if (_caching == null)
                    _caching = (CachingConfigurationSection)ConfigurationManager.GetSection("cachingConfig");

                return _caching;
            }
        }

        private static string _xmlFilesPath = null;
        public static string XmlFilesPath
        {
            get
            {
                if (_xmlFilesPath == null)
                    _xmlFilesPath = ConfigurationManager.AppSettings["XmlFilesPath"];

                return _xmlFilesPath;
            }
        }

        private static string _kpsReceiptsOnlyFileName = null;
        public static string KpsReceiptsOnlyFileName
        {
            get
            {
                if (_kpsReceiptsOnlyFileName == null)
                    _kpsReceiptsOnlyFileName = ConfigurationManager.AppSettings["KpsReceiptsOnlyFileName"];

                return _kpsReceiptsOnlyFileName;
            }
        }

        private static string _localReceiptsOnlyFileName = null;
        public static string LocalReceiptsOnlyFileName
        {
            get
            {
                if (_localReceiptsOnlyFileName == null)
                    _localReceiptsOnlyFileName = ConfigurationManager.AppSettings["LocalReceiptsOnlyFileName"];

                return _localReceiptsOnlyFileName;
            }
        }

    }
}
