using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Utils;
using Imis.Domain;

namespace EudoxusOsy.Portal.InternalServices
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class ServerSyncInternalService : BaseInternalService
    {
        #region [ Constructor ]

        public ServerSyncInternalService() : base() { }

        public ServerSyncInternalService(IUnitOfWork uow) : base(uow) { }

        #endregion

        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        public ServiceResponse SyncInvalidateCookie(InvalidateCookieRequest request)
        {
            try
            {
                AuthenticationService.InvalidateCookieInternal(request.Username);
                return Success();
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex, this);
                return Failure(ex.Message);
            }
        }
    }
}