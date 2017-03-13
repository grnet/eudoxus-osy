using EudoxusOsy.BusinessModel;
using EudoxusOsy.BusinessModel.Services;
using EudoxusOsy.Services.Models;
using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Runtime.Serialization;

namespace EudoxusOsy.Services
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class BookServices : BaseService
    {
        [WebInvoke(UriTemplate = "putKpsBook", Method = "POST")]
        public ServiceResponse PutKpsBook(Stream finalXml)
        {
            try
            {
                StreamReader sr = new StreamReader(finalXml);
                var book = new Serializer<KpsBookDto>().Deserialize(sr.ReadToEnd());

                BookService.MapFromDto(book);

                LogCall(true, enStatusCode.OK);
                /*
                if (registrationInserted == true)
                {
                    return new ServiceResponse(true, enStatusCode.KpsBooksInsertionSucceeded);
                }
                else
                {
                    return new ServiceResponse(true, enStatusCode.KpsBooksInsertionFailed);
                }
                */
                return null;
            }
            catch (Exception ex)
            {
                return new ServiceResponse(true, enStatusCode.Errors, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "coauthors", Method = "PUT")]
        public ServiceResponse InsertCoAuthors(CoAuthorsDto request)
        {
            try
            {
                bool? registrationInserted;
                registrationInserted = true; //KpsRegistrationService.InsertRegistration(request);



                LogCall(true, enStatusCode.OK);

                if (registrationInserted == true)
                {
                    return new ServiceResponse(true, enStatusCode.KPSRegistrationInsertionSucceeded);
                }
                else
                {
                    return new ServiceResponse(true, enStatusCode.KPSRegistrationInsertionFailed);
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse(true, enStatusCode.Errors, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

    }
}
