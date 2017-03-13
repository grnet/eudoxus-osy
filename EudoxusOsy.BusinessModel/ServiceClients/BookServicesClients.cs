using EudoxusOsy.Utils;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;

namespace EudoxusOsy.BusinessModel
{
    public static class BookServicesClients
    {

        public static GetBooksResponse GetModifiedBooks()
        {
            var endpoint = string.Format("http://{0}.eudoxus.gr:9080/rest/bsa/GetModifiedBooks", Config.IsPilotSite ? "test" : "service");
            var request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Method = "GET";
            //request.ContentType = "application/json; charset=utf-8";


            var response = request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var data = reader.ReadToEnd();
                var serializer = new JavaScriptSerializer() { MaxJsonLength = 86753090 };
                return serializer.Deserialize<GetBooksResponse>(data);
            }
        }

        public static GetBooksResponse GetNewBooks()
        {
            var endpoint = string.Format("http://{0}.eudoxus.gr:9080/rest/bsa/GetNewBooks", Config.IsPilotSite ? "test" : "service");
            var request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Method = "GET";
            //request.ContentType = "application/json; charset=utf-8";

            try
            {
                var response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var data = reader.ReadToEnd();
                    var serializer = new JavaScriptSerializer() { MaxJsonLength = 300000000 };
                    return serializer.Deserialize<GetBooksResponse>(data);
                }
            }
            catch (WebException ex)
            {
                LogHelper.LogError(ex, typeof(BookServicesClients), ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                throw ex.InnerException;
            }
        }

        public static UpdateBookStatusResponse UpdateBookStatus(UpdateBookStatusRequest requestData)
        {
            var endpoint = string.Format("http://{0}.eudoxus.gr:9080/rest/bsa/UpdateBookStatus", Config.IsPilotSite ? "test" : "service");
            var request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            StreamWriter requestWriter = new StreamWriter(request.GetRequestStream());
            requestWriter.Write(string.Format("id={0}&bsaId={1}&bsaStatus={2}", requestData.id, requestData.bsaId, requestData.bsaStatus));
            LogHelper.LogMessage(string.Format("id={0}&bsaId={1}&bsaStatus={2}", requestData.id, requestData.bsaId, requestData.bsaStatus), typeof(BookServicesClients));
            requestWriter.Close();

            var response = request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var data = reader.ReadToEnd();
                return new JavaScriptSerializer().Deserialize<UpdateBookStatusResponse>(data);
            }
        }

    }

    /**  update book status
     Full = Book is in Sync between KPS and BSA
     Modified = Book is modified in KPS, BSA should be updated
     New = Book is created in KPS, BSA should create a record
     Unpublished = Books removed in BSA and thus removed from KPS (Currently unused)
     Deleted = Book is deleted in KPS, BSA should delete this book also
    */
    public class UpdateBookStatusRequest
    {
        public long id { get; set; }
        public long bsaId { get; set; }
        public string bsaStatus { get; set; } //values Full|Modified|New|Unpublished|Deleted
    }

    public class UpdateBookStatusResponse
    {
    }

    public class GetBooksResponse
    {
        public int numResults { get; set; }
        public List<BookDTO> books { get; set; }
    }

    public class BookDTO
    {
        public int id { get; set; }
        public bool active { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public string authors { get; set; }
        public string isbn { get; set; }
        public bool hasMoreVolumes { get; set; }
        public long masterVolume { get; set; }
        public int publicationYear { get; set; }
        public int? firstRegistrationYear { get; set; }
        public string editorialHouse { get; set; }
        public string description { get; set; }
        public string binding { get; set; } // values 'SOFT|HARD'
        public List<string> keywords { get; set; }
        public List<string> subjects { get; set; }
        public string dimensions { get; set; }
        public int pages { get; set; }
        public string pathToCover { get; set; }
        public decimal suggestedPrice { get; set; }
        public bool hasBeenPriced { get; set; }
        public bool finalizedByPublisher { get; set; }
        public string kindBook { get; set; } // Published
        public int publicationNumber { get; set; }
        public int supplierCode { get; set; }
        public string linkToEH { get; set; }
        public string bsaStatus { get; set; } // values Full|Modified|New
        public string publisher { get; set; }
        public decimal? price { get; set; }
        public bool checkedPrice { get; set; }
        public string priceComments { get; set; }
        public string decisionNumber { get; set; }
    }
}
