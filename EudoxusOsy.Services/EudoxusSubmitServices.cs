using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using EudoxusOsy.BusinessModel;

namespace EudoxusOsy.Services
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class EudoxusSubmitServices : BaseService
    {
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "SyncPublisher", Method = "POST")]
        public ServiceResponse SyncPublisher(SyncPublisherDto request)
        {
            try
            {
                bool? supplierCreated;
                Supplier supplier = EudoxusSubmitService.SyncPublisher(request, out supplierCreated);

                LogCall(true, enStatusCode.OK);

                if (supplierCreated.Value)
                {
                    return new ServiceResponse(true, enStatusCode.SupplierCreated);
                }
                else
                {
                    return new ServiceResponse(true, enStatusCode.SupplierUpdated);
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                LogCall(false, enStatusCode.SupplierInsertionFailed);
                return new ServiceResponse(true, enStatusCode.Errors, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "SyncMinistryPaymentsUser", Method = "POST")]
        public ServiceResponse SyncMinistryPaymentsUser(SyncMinistryPaymentsUserDto request)
        {
            try
            {
                bool? userCreated;
                Reporter ministryPaymentsUser = EudoxusSubmitService.SyncMinistryPaymentsUser(request, out userCreated);

                LogCall(true, enStatusCode.OK);

                if (userCreated.Value)
                {
                    return new ServiceResponse(true, enStatusCode.SupplierCreated);
                }
                else
                {
                    return new ServiceResponse(true, enStatusCode.SupplierUpdated);
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                return new ServiceResponse(true, enStatusCode.Errors, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }
    }
}
