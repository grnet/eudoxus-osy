using EudoxusOsy.BusinessModel;
using EudoxusOsy.BusinessModel.Services;
using EudoxusOsy.Utils;
using Imis.Domain;
using System;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;

namespace EudoxusOsy.Services
{
    public class ServiceErrorException : Exception
    {
        public ServiceErrorException(enStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public enStatusCode StatusCode { get; private set; }
    }

    public class ServiceErrorsException<T> : ServiceErrorException
    {
        public ServiceErrorsException(T data)
            : base(enStatusCode.Errors)
        {
            Data = data;
        }

        public T Data { get; private set; }
    }

    public abstract class BaseService : IDisposable
    {
        #region [ Fields ]

        private readonly ServiceCallerDetails _caller = new ServiceCallerDetails();
        private readonly Lazy<EudoxusSubmitService> _eudoxusSubmitService;
        private readonly Lazy<KpsRegistrationService> _kpsRegistrationService;
        private readonly Lazy<BookService> _bookService;
        private IUnitOfWork _UnitOfWork = null;

        #endregion

        protected BaseService()
        {
            _eudoxusSubmitService = new Lazy<EudoxusSubmitService>(() => new EudoxusSubmitService(UnitOfWork));
            _kpsRegistrationService = new Lazy<KpsRegistrationService>(() => new KpsRegistrationService(UnitOfWork));
            _bookService = new Lazy<BookService>(() => new BookService(UnitOfWork));

        }

        protected IUnitOfWork UnitOfWork { get { return _UnitOfWork ?? (_UnitOfWork = UnitOfWorkFactory.Create()); } }

        protected ServiceCallerDetails ApiCaller { get { return _caller; } }

        protected EudoxusSubmitService EudoxusSubmitService { get { return _eudoxusSubmitService.Value; } }
        protected KpsRegistrationService KpsRegistrationService { get { return _kpsRegistrationService.Value; } }

        protected BookService BookService { get { return _bookService.Value; } }

        protected ServiceResponse<TResponse> Execute<TResponse>(Func<TResponse> execution, string getParameters = null, [CallerMemberName] string serviceMethodCalled = null)
        {
            try
            {
                var result = execution();
                LogCall(true, enStatusCode.OK, getParameters, serviceMethodCalled);
                return Success(result);
            }
            catch (ServiceErrorException seex)
            {
                LogCall(false, seex.StatusCode, getParameters, serviceMethodCalled);
                return Failure<TResponse>(seex.StatusCode);
            }
            catch (Exception ex)
            {
                if (ex is WebFaultException)
                    throw;

                return UnhandledException<TResponse>(ex, serviceMethodCalled);
            }
        }

        #region [ Logging ]

        private static string GetClientIP()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                var request = HttpContext.Current.Request;

                var ip = request.Headers["X-Forwarded-For"];

                return string.IsNullOrEmpty(ip)
                        ? request.UserHostAddress
                        : ip;
            }

            return string.Empty;
        }

        protected void LogCall(bool success, enStatusCode code, string getParameters = null, string errorEnum = null, [CallerMemberName] string serviceMethodCalled = null)
        {
            try
            {
                using (IUnitOfWork uow = UnitOfWorkFactory.Create())
                {
                    ServiceLog logEntry = new ServiceLog();

                    logEntry.CalledAt = DateTime.Now;
                    logEntry.MethodCall = serviceMethodCalled;
                    logEntry.CalledBy = ApiCaller.Username;
                    logEntry.ErrorCode = code.ToString();
                    logEntry.Success = success;
                    logEntry.IP = GetClientIP();
                    logEntry.PostRequest = OperationContext.Current.RequestContext.RequestMessage.ToString();
                    logEntry.GetParameters = getParameters;

                    uow.MarkAsNew(logEntry);
                    uow.Commit();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError<BaseService>(ex, null, string.Format("Error while saving internal service response for submissionCode {0}", code));
            }
        }

        protected void LogException(Exception ex)
        {
            if (ex != null)
                LogHelper.LogError(ex, typeof(BaseService), ex.Message);

            else if (HttpContext.Current == null || HttpContext.Current.Error == null)
                LogHelper.LogError(new Exception("Unknown Error - No Context"), typeof(BaseService));

            else
                LogHelper.LogError(new Exception("Unknown error"), typeof(BaseService));
        }

        protected ServiceResponse UnhandledException(Exception ex, [CallerMemberName] string srvMethod = null)
        {
            LogException(ex);
            LogCall(false, enStatusCode.UnexpectedError, srvMethod);
            return Failure(enStatusCode.UnexpectedError);
        }

        protected ServiceResponse<T> UnhandledException<T>(Exception ex, [CallerMemberName] string srvMethod = null)
        {
            LogException(ex);
            LogCall(false, enStatusCode.UnexpectedError, srvMethod);
            return Failure<T>(enStatusCode.UnexpectedError);
        }

        #endregion

        #region [ RequestValidation ]

        protected ServiceErrorException ServiceError(enStatusCode statusCode)
        {
            return new ServiceErrorException(statusCode);
        }

        #endregion

        #region [ Results ]

        protected ServiceResponse Success()
        {
            return new ServiceResponse(true);
        }

        protected ServiceResponse Errors()
        {
            return new ServiceResponse(true, enStatusCode.Errors);
        }

        protected ServiceResponse Failure(enStatusCode code)
        {
            return new ServiceResponse(false, code);
        }

        protected ServiceResponse<T> Success<T>(T value)
        {
            return new ServiceResponse<T>(true, value);
        }

        protected ServiceResponse<T> Errors<T>(T value)
        {
            return new ServiceResponse<T>(true, value, enStatusCode.Errors);
        }

        protected ServiceResponse<T> Failure<T>(enStatusCode code)
        {
            return new ServiceResponse<T>(false, default(T), code);
        }

        #endregion

        void IDisposable.Dispose()
        {
            if (_UnitOfWork != null)
                _UnitOfWork.Dispose();
        }
    }
}
