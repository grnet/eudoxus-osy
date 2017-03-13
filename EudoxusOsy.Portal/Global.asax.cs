using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.InternalServices;
using EudoxusOsy.Utils;
using EudoxusOsy.Utils.Queue;
using EudoxusOsy.Utils.Worker;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Activation;
using System.Threading;
using System.Web;
using System.Web.Routing;
using System.Web.Security;

namespace EudoxusOsy.Portal
{
    public class Global : HttpApplication
    {
        public static string VersionNumber { get; private set; }

        #region [ Init Functions ]


        private void InitWorker()
        {
            try
            {
                var runTimes = new List<TaskLastRunTime>();
                using (var uow = UnitOfWorkFactory.Create())
                    runTimes = new TaskLastRunTimeRepository(uow).LoadAll().ToList();

                var updateReceiptsRunTime = runTimes.FirstOrDefault(x => x.Name == TaskNames.UpdateReceiptsFromAuditReceipts);
                AsyncWorker.Instance.Register(TaskNames.UpdateReceiptsFromAuditReceipts, updateReceiptsRunTime == null ? null : (DateTime?)updateReceiptsRunTime.LastRunTime, () =>
                {
                    /**
                        Daily Task to populate the receipts table from audit_receipts
                    */
                    UpdateReceiptsHelper.UpdateReceiptsFromAuditReceipts();
                });

                var updateBooksRuntime = runTimes.FirstOrDefault(x => x.Name == TaskNames.UpdateBooks);
                AsyncWorker.Instance.Register(TaskNames.UpdateBooks, updateBooksRuntime == null ? null : (DateTime?)updateBooksRuntime.LastRunTime, () =>
                {
                    /**
                        Daily Task to populate the receipts table from audit_receipts
                    */
                    UpdateBooksHelper.UpdateModifiedBooksFromKPS();
                    UpdateBooksHelper.GetNewBooksFromKPS();
                });


                var compareXmlReceiptsRunTime = runTimes.FirstOrDefault(x => x.Name == TaskNames.CompareXmlReceipts);
                AsyncWorker.Instance.Register(TaskNames.CompareXmlReceipts, compareXmlReceiptsRunTime == null ? null : (DateTime?)compareXmlReceiptsRunTime.LastRunTime, () =>
                {
                    /**
                        Daily Task that checks if the compare procedure should run (from shouldCompare... ApplicationData entry) and runs it
                    */
                    UpdateReceiptsHelper.CompareXmlReceipts();
                });

                var cacheStatsRuntime = runTimes.FirstOrDefault(x => x.Name == TaskNames.CacheStats);
                AsyncWorker.Instance.Register(TaskNames.CacheStats, cacheStatsRuntime == null ? null : (DateTime?)cacheStatsRuntime.LastRunTime, () =>
                {
                    using (var ctx = UnitOfWorkFactory.Create())
                    {
                        /**
                            The task should do the calculations for the current active Phase ONLY
                        */
                        ((DBEntities)ctx).CommandTimeout = 600;
                        ((DBEntities)ctx).CacheStats(0);
                    }
                });


                AsyncWorker.Instance.AsyncWorkerItemProcessed += (s, e) =>
                {
                    using (var ctx = UnitOfWorkFactory.Create())
                    {
                        var existing = new TaskLastRunTimeRepository(ctx).FindByName(e.TaskName);
                        if (existing == null)
                            ctx.MarkAsNew(new TaskLastRunTime() { Name = e.TaskName, LastRunTime = e.ProcessedAt });
                        else
                            existing.LastRunTime = e.ProcessedAt;

                        ctx.Commit();
                    }
                };

                AsyncWorker.Instance.Initialize();
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("ApplicationStart").Fatal("Application_Start Error - Init Worker", ex);
            }
        }

        private void InitQueue()
        {
            try
            {
                ServiceQueue.Instance.AddWorker(EmailQueueWorker.Current);
                ServiceQueue.Instance.AddWorker(SMSQueueWorker.Current);
                ServiceQueue.Instance.AddWorker(ServerSyncQueueWorker.Current);

                ServiceQueue.Instance.Initialize();
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("ApplicationStart").Fatal("Application_Start Error - Init Queue", ex);
            }
        }

        private void InitCache()
        {
            try
            {
                if (Config.Caching.InitializeCacheOnStart)
                    CacheManager.Initialize();
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("ApplicationStart").Fatal("Application_Start Error - Init Cache", ex);
            }
        }

        private void InitServices()
        {
            RouteTable.Routes.Add(new ServiceRoute("InternalServices/Sync", new WebServiceHostFactory(), typeof(ServerSyncInternalService)));
            RouteTable.Routes.Add(new ServiceRoute("api/publisher", new WebServiceHostFactory(), typeof(EudoxusOsy.Services.EudoxusSubmitServices)));
            RouteTable.Routes.Add(new ServiceRoute("api/registrations", new WebServiceHostFactory(), typeof(EudoxusOsy.Services.KPSRegistrationServices)));
            RouteTable.Routes.Add(new ServiceRoute("api/books", new WebServiceHostFactory(), typeof(EudoxusOsy.Services.BookServices)));
        }

        #endregion

        protected void Application_Start(object sender, EventArgs e)
        {
            Application[EudoxusOsyConstants.CHANGED_PROVIDER_USERS_KEY] = new HashSet<string>();

            VersionNumber = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            log4net.Config.XmlConfigurator.Configure();

            InitQueue();
            InitWorker();
            InitCache();
            InitServices();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            var sessionCookieKey = Response.Cookies.AllKeys.SingleOrDefault(c => c.ToLower() == "asp.net_sessionid");
            var sessionCookie = Response.Cookies.Get(sessionCookieKey);
            if (sessionCookie != null)
            {
                sessionCookie.HttpOnly = true;
            }
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                var data = AuthenticationService.GetUserData();
                if (data.ReporterID == 0 && string.IsNullOrEmpty(data.ContactName))
                {
                    AuthenticationService.LoginReporter(Thread.CurrentPrincipal.Identity.Name);
                    data = AuthenticationService.GetUserData();
                }

                if (!AuthenticationService.IsCookieValid())
                {
                    AuthenticationService.InvalidateCookie(Thread.CurrentPrincipal.Identity.Name, false);
                    FormsAuthentication.SignOut();
                    FormsAuthentication.RedirectToLoginPage();
                }

                var newIdentity = new EudoxusOsyIdentity(Thread.CurrentPrincipal.Identity.Name, Thread.CurrentPrincipal.Identity.AuthenticationType)
                {
                    ReporterID = data.ReporterID,
                    ContactName = data.ContactName
                };

                var newPrincipal = new EudoxusOsyPrincipal(newIdentity, Roles.GetRolesForUser(Thread.CurrentPrincipal.Identity.Name)) { Identity = newIdentity };

                Thread.CurrentPrincipal = newPrincipal;
                HttpContext.Current.User = newPrincipal;
            }
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject != null)
                LogHelper.LogError(e.ExceptionObject as Exception, "Global", string.Format("Unhandler Exception. Is Application Terminating:{0}", e.IsTerminating));
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception error = Server.GetLastError();

            if (error != null)
            {
                if (error is HttpUnhandledException && HttpContext.Current.Request.Url.ToString().Contains("UploadPhoto"))
                {
                    //Do nothing...
                }
                else
                    LogHelper.LogError(error, "Global");
            }
            else if (HttpContext.Current == null || HttpContext.Current.Error == null)
            {
                LogHelper.LogError(new Exception("Unknown Error - No Context"), "Global");
            }
            else
            {
                LogHelper.LogError(new Exception("Unknown error"), "Global");
            }
        }
    }
}