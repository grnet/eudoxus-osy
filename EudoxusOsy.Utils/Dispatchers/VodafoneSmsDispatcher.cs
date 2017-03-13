using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using log4net;

namespace EudoxusOsy.Utils
{
    public class VodafoneSmsDispatcher : ISmsDispatcher
    {
        private static readonly ILog log = LogManager.GetLogger("SMSSender");

        /// <summary>
        /// When true, no SMS are sent, they are only logged.
        /// </summary>
        private static bool DebugMode
        {
            get
            {
                if (_enableSMS == null)
                    _enableSMS = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSMS"]);
                return !_enableSMS.Value;
            }
        }

        private static bool? _enableSMS = null;

        private static string _appUrl = ConfigurationManager.AppSettings["ApplicationUrl"];

        public void Send(string msg, string recipID, string recipNumber, string[] fieldValues)
        {
            Send(msg, recipNumber, fieldValues);
        }

        #region [ Vodafone Sms Service ]

        private void Send(string msg, string recipient, IList<string> fieldValuesList)
        {
            //Perform the necessary tests. We don't want to waste money on invalid SMSs, right?
            if (string.IsNullOrEmpty(msg))
                throw new ArgumentException("Cannot send empty SMS");
            if (string.IsNullOrEmpty(recipient))
                throw new ArgumentException("Cannot send SMS because no recipients were defined");

            for (var i = 1; i <= fieldValuesList.Count; i++)
                msg = msg.Replace("#field" + i + "#", fieldValuesList[i - 1]);

            log.Debug("Before actually calling the CallSendService method");
            CallSendService(msg, recipient);
        }

        private static XDocument ConstructSmsXml(string msg, string recipient)
        {
            log.Debug("Starting the Xml construction method");


            //Create an XDocument and add the basic elements
            XDocument xd = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("send",
                    new XElement("username", new XText(ConfigurationManager.AppSettings["VodafoneUsername"])),
                    new XElement("password", new XText(ConfigurationManager.AppSettings["VodafonePassword"])),
                    new XElement("senderId", new XText("einclusion")),
                    new XElement("recipients", new XElement("recipient", new XText(recipient))),
                    new XElement("message", new XText(msg)),
                    new XElement("dlr-url", new XText(_appUrl + "/HandleVodafoneDlr.ashx"))
                )
            );
            log.Debug("Xml Constructed");
            return xd;
        }

        private static void CallSendService(string msg, string recipient)
        {
            string url = "https://mybsms.gr/ws/send.xml";

            if (ConfigurationManager.AppSettings["UrlSMS"] != null)
                url = Convert.ToString(ConfigurationManager.AppSettings["UrlSMS"]);

            XDocument xd = ConstructSmsXml(msg, recipient);

            log.Debug("Before constructing the post to the webpage to be sent");
            if (!DebugMode)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "text/xml";
                request.Method = "POST";

                string data = xd.Declaration.ToString() + xd.ToString(SaveOptions.DisableFormatting);

                byte[] postData = Encoding.UTF8.GetBytes(data);
                request.ContentLength = postData.Length;

                try
                {
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(postData, 0, postData.Length);
                    requestStream.Close();

                    log.Debug("Before doing the post");

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    string strResponse = new StreamReader(responseStream).ReadToEnd();
                    XDocument xdStatus = (XDocument)XDocument.Parse(strResponse.Substring(strResponse.IndexOf('<')));

                    var error = xdStatus.Element("response").Element("error").Value;
                    if (!string.IsNullOrEmpty(error))
                    {
                        log.ErrorFormat("Received FAIL status for SMS sending. Reason : [{0}].", error);
                        throw new ApplicationException("The SMS service failed in sending the SMS. Reason : " + error);
                    }

                    responseStream.Close();
                }
                catch (WebException wex)
                {
                    //Log the exception and rethrow it
                    log.Error("Failure sending the SMS : [" + xd.ToString(SaveOptions.DisableFormatting) + "]", wex);
                    throw;
                }
            }
        }

        #endregion
    }
}
