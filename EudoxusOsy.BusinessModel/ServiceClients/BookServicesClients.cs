using System;
using System.CodeDom;
using EudoxusOsy.Utils;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using EudoxusOsy.BusinessModel.Interfaces;
using iTextSharp.text.log;

namespace EudoxusOsy.BusinessModel
{
    public static class BookServicesClients
    {
        public static bool GetNewBooksToLocalFile()
        {
            bool ok = false;

            if (System.IO.File.Exists(Config.SystemNewBooksFile))
            {
                System.IO.File.Delete(Config.SystemNewBooksFile);
            }

            try
            {                
                var response = NewSvc();                

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var data = reader.ReadToEnd();                    
                    using (var swriter = System.IO.File.CreateText(Config.SystemNewBooksFile))
                    {
                        swriter.Write(data);
                        swriter.Close();
                    }

                }
                ok = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex, typeof(BookServicesClients), ex.InnerException != null ? ex.InnerException.Message : ex.Message);              
            }

            return ok;
        }
        public static bool GetModifiedBooksToLocalFile()
        {
            bool ok = false;

            if (System.IO.File.Exists(Config.SystemModifiedBooksFile))
            {
                System.IO.File.Delete(Config.SystemModifiedBooksFile);
            }

            try
            {
                var response = ModifiedSvc();

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var data = reader.ReadToEnd();                    
                    using (var swriter = System.IO.File.CreateText(Config.SystemModifiedBooksFile))
                    {
                        swriter.Write(data);
                        swriter.Close();
                    }
                }
                ok = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex, typeof(BookServicesClients), (ex.InnerException != null ? ex.InnerException.Message != null : false) ? ex.InnerException.Message : ex.Message);             
            }

            return ok;
        }        

        public static bool GetSpecificBooksToLocalFile(string booksList)
        {
            bool ok = false;

            if (System.IO.File.Exists(Config.SystemModifiedBooksFile))
            {
                System.IO.File.Delete(Config.SystemModifiedBooksFile);
            }

            try
            {
                var response = SpecificBooksSvc(booksList);

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var data = reader.ReadToEnd();                    
                    using (var swriter = System.IO.File.CreateText(Config.SystemModifiedBooksFile))
                    {
                        swriter.Write(data);
                        swriter.Close();
                    }
                }
                ok = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex, typeof(BookServicesClients), (ex.InnerException != null ? ex.InnerException.Message != null : false) ? ex.InnerException.Message : ex.Message);             
            }

            return ok;
        }        
        
        public static GetBooksResponseSlim GetBooksFromFile(string path)
        {
            using (var reader = new StreamReader(path))
            {
                var data = reader.ReadToEnd();
                var serializer = new JavaScriptSerializer() { MaxJsonLength = 86753090 };
                return serializer.Deserialize<GetBooksResponseSlim>(data);
            }
        }
    
        #region Call service and store data in memory
        public static GetBooksResponse GetModifiedBooks()
        {
            var response = ModifiedSvc();
            try
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var data = reader.ReadToEnd();
                    var serializer = new JavaScriptSerializer() { MaxJsonLength = 86753090 };
                    return serializer.Deserialize<GetBooksResponse>(data);
                }
            }
            catch (WebException ex)
            {
                LogHelper.LogError(ex, typeof(BookServicesClients), (ex.InnerException != null ? ex.InnerException.Message != null : false) ? ex.InnerException.Message : ex.Message);
                throw ex.InnerException;
            }
        }

        public static GetBooksResponse GetSpecificBooks(string booksList)
        {
            var response = SpecificBooksSvc(booksList);
            try
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var data = reader.ReadToEnd();
                    var serializer = new JavaScriptSerializer() { MaxJsonLength = 86753090 };
                    return serializer.Deserialize<GetBooksResponse>(data);
                }
            }
            catch (WebException ex)
            {
                LogHelper.LogError(ex, typeof(BookServicesClients), (ex.InnerException != null ? ex.InnerException.Message != null : false) ? ex.InnerException.Message : ex.Message);
                throw ex.InnerException;
            }
        }
        public static GetBooksResponse GetNewBooks()
        {
            var response = NewSvc();

            try
            {                
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var data = reader.ReadToEnd();
                    var serializer = new JavaScriptSerializer() { MaxJsonLength = 86753090 };
                    return serializer.Deserialize<GetBooksResponse>(data);
                }
            }
            catch (WebException ex)
            {
                LogHelper.LogError(ex, typeof(BookServicesClients), (ex.InnerException != null ? ex.InnerException.Message != null : false) ? ex.InnerException.Message : ex.Message);
                throw ex.InnerException;
            }
        }
#endregion
        #region service

        
        private static WebResponse SpecificBooksSvc(string booksList)
        {
            //http://test.eudoxus.gr/rest/bsa/GetBooksByIdList?idList=68400142                    
            //http://service.eudoxus.gr/rest/bsa/GetBooksByBsaIdList
            var endpoint = string.Format("https://{0}.eudoxus.gr/rest/bsa/GetBooksByIdList?idList={1}",
                Config.IsPilotSite ? "test" : "service", booksList);
            
            var request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";

            WebResponse response ;

            try
            {
                response = request.GetResponse();
            }
            catch (Exception ex)
            {
                response = null;
                LogHelper.LogError(ex, typeof(BookServicesClients), (ex.InnerException != null ? ex.InnerException.Message != null : false) ? ex.InnerException.Message : ex.Message);                
            }
            
            return response;
        }

        private static WebResponse ModifiedSvc()
        {
            //http://test.eudoxus.gr/rest/bsa/GetModifiedBooks
            //http://service.eudoxus.gr/rest/bsa/GetModifiedBooks
            var endpoint = string.Format("https://{0}.eudoxus.gr/rest/bsa/GetModifiedBooks",
                Config.IsPilotSite ? "test" : "service");
            var request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";

            WebResponse response ;

            try
            {
                response = request.GetResponse();
            }
            catch (Exception ex)
            {
                response = null;
                LogHelper.LogError(ex, typeof(BookServicesClients), (ex.InnerException != null ? ex.InnerException.Message != null : false) ? ex.InnerException.Message : ex.Message);                
            }
            
            return response;
        }

        private static WebResponse NewSvc()
        {
            //http://test.eudoxus.gr/rest/bsa/GetNewBooks
            //http://service.eudoxus.gr/rest/bsa/GetNewBooks
            var endpoint = string.Format("https://{0}.eudoxus.gr/rest/bsa/GetNewBooks",
                Config.IsPilotSite ? "test" : "service");
            var request = (HttpWebRequest) WebRequest.Create(endpoint);
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";

            WebResponse response;

            try
            {                
                response = request.GetResponse();                
            }
            catch (Exception ex)
            {
                response = null;
                LogHelper.LogError(ex, typeof(BookServicesClients), (ex.InnerException != null ? ex.InnerException.Message != null : false) ? ex.InnerException.Message : ex.Message);                
            }

            return response;
        }
#endregion

        public static UpdateBookStatusResponse UpdateBookStatus(UpdateBookStatusRequest requestData) 
        {
            if (Config.EnableKPSUpdate)
            {
                var endpoint = string.Format("https://{0}.eudoxus.gr/rest/bsa/UpdateBookStatus", Config.IsPilotSite ? "test" : "service");
                var request = (HttpWebRequest)WebRequest.Create(endpoint);
                request.Method = Config.UpdateBookStatusKPSMethod !=null  ? Config.UpdateBookStatusKPSMethod : "POST";
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
            else
            {
                return new UpdateBookStatusResponse();
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
    public class GetBooksResponse : IGetBookResponse
    {
        public int numResults { get; set; }
        public List<BookDTO> books { get; set; }
    }

    public class GetBooksResponseSlim : IGetBookResponseSlim
    {
        public int numResults { get; set; }
        public List<BookDTOSlim> books { get; set; }
    }
    public class ArchiveDTO
    {
         public int id { get; set; }
         public int bookId { get; set; }
         public int year { get; set; }
         public decimal? price { get; set; }
         public decimal suggestedPrice { get; set; }
         public string fek { get; set; }
         public string lastUpdate { get; set; }
         public bool finalizedByPublisher { get; set; }
         public string decisionNumber { get; set; }
         public string priceComments { get; set; }
         public bool? checkedPrice { get; set; }
    }
    public class BookDTOSlim
    {
        public int id { get; set; }
        public bool active { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public string authors { get; set; }
        public string isbn { get; set; }        
        public int? firstPostYear { get; set; }        
        public int pages { get; set; }        
        public decimal suggestedPrice { get; set; }        
        public string type { get; set; } // Published        
        public int publisherId { get; set; }        
        public string publisherName { get; set; }
        public decimal? price { get; set; }
        public bool checkedPrice { get; set; }
        public string priceComments { get; set; }
        public string decisionNumber { get; set; }
        public string fek { get; set; }
        public List<ArchiveDTO> archives { get; set; }
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
        public int? firstPostYear { get; set; }
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
        public string type { get; set; } // Published
        public int publicationNumber { get; set; }
        public int publisherId { get; set; }
        public string linkToEH { get; set; }
        public string bsaStatusRecord { get; set; } // values Full|Modified|New
        public string publisherName { get; set; }
        public decimal? price { get; set; }
        public bool checkedPrice { get; set; }
        public string priceComments { get; set; }
        public string decisionNumber { get; set; }
        public string fek { get; set; }
        public List<ArchiveDTO> archives { get; set; }
    }
}
