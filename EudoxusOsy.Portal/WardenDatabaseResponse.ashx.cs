using EudoxusOsy.BusinessModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EudoxusOsy.Portal
{
    /// <summary>
    /// Summary description for Handler1
    /// </summary>
    public class WardenDatabaseResponse : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var booksCount = new BookRepository().LoadAll().Count();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            dynamic response = new JObject();
            response.HttpStatusCode = "OK";
            response.Count = booksCount;
            response.QueryResponseTime = elapsedMs;
            response.QueryDescription = "Επιστρέφει το ακέραιο σύνολο των εγγραφών των βιβλίων της βάσης δεδομένων Eudoxus-Osy.";
            response.InternalProcess = "";

            context.Response.ContentType = "application/json";
            context.Response.Write(response);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}