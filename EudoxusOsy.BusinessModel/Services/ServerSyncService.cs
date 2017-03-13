using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using EudoxusOsy.Utils;

namespace EudoxusOsy.BusinessModel
{
    public class InvalidateCookieRequest
    {
        public string Username { get; set; }
    }

    public class ServerSyncService
    {
        #region [ Helpers ]

        private HttpWebRequest GetRequest(string serviceName)
        {
            var endpoint = GetSyncEndpoint();
            if (string.IsNullOrEmpty(endpoint))
                return null;

            endpoint = endpoint + serviceName;

            var request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            return request;
        }

        private ServiceResponse GetResponse(HttpWebRequest request)
        {
            var response = request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var data = reader.ReadToEnd();
                return new JavaScriptSerializer().Deserialize<ServiceResponse>(data);
            }
        }

        private string GetSyncEndpoint()
        {
            string endpoint = string.Empty;

            if (Environment.MachineName.ToLower() == "web01")
                endpoint = "http://EudoxusOsy-app.web02.webapps.local/InternalServices/Sync/";
            else if (Environment.MachineName.ToLower() == "web02")
                endpoint = "http://EudoxusOsy-app.web01.webapps.local/InternalServices/Sync/";

            return endpoint;
        }

        #endregion

        public bool SyncInvalidateCookie(string username)
        {
            try
            {
                var request = GetRequest("SyncInvalidateCookie");
                if (request == null)
                    return false;

                var requestWriter = new StreamWriter(request.GetRequestStream());
                requestWriter.Write(new JavaScriptSerializer().Serialize(new InvalidateCookieRequest() { Username = username }));
                requestWriter.Close();

                var response = GetResponse(request);
                return response.Success;
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex, this);
                return false;
            }
        }
    }
}
