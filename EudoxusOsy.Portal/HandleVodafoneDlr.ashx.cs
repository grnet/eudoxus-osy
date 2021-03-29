using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EudoxusOsy.Portal
{
    /// <summary>
    /// Summary description for HandleVodafoneDlr
    /// </summary>
    public class HandleVodafoneDlr : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //Handle vodafone dlr response
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