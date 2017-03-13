using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using EudoxusOsy.BusinessModel;

namespace EudoxusOsy.Services
{
    [DataContract]
    public class ServiceResponse<T> : ServiceResponse
    {
        [DataMember]
        public T Result { get; set; }

        public ServiceResponse(bool isValid, T result, enStatusCode code = enStatusCode.OK)
            : base(isValid, code)
        {
            Result = result;
        }
    }

    [DataContract]
    public class ServiceResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public int StatusCode { get; set; }

        [DataMember]
        public string StatusMessage { get; set; }

        public ServiceResponse(bool isValid, enStatusCode code = enStatusCode.OK, string statusMessage = null)
        {
            Success = isValid;
            StatusCode = code.GetValue();
            StatusMessage = !string.IsNullOrEmpty(statusMessage) ? statusMessage : Messages.GetStatusCode(code);
        }
    }
}
