using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using log4net;

namespace EudoxusOsy.Utils
{
    public class LocotelSmsDispatcher : ISmsDispatcher
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

        public void Send(string msg, string recipID, string recipNumber, string[] fieldValues)
        {
            Dictionary<string, string> recipients = new Dictionary<string, string>();
            recipients.Add(recipID, recipNumber);
            List<string[]> fieldValuesList = null;
            if (fieldValues != null && fieldValues.Length > 0)
            {
                fieldValuesList = new List<string[]>();
                fieldValuesList.Add(fieldValues);
            }
            Send(msg, recipients, fieldValuesList);
        }

        #region [ LocoTel SMS service ]

        private void Send(string msg, IDictionary<string, string> recipients, IList<string[]> fieldValuesList)
        {
            //Perform the necessary tests. We don't want to waste money on invalid SMSs, right?
            if (String.IsNullOrEmpty(msg))
                throw new ArgumentException("Cannot send empty SMS");
            if (recipients == null || recipients.Count == 0)
                throw new ArgumentException("Cannot send SMS because no recipients were defined");
            if (fieldValuesList != null && fieldValuesList.Count != recipients.Count)
                throw new ArgumentException("The list of field values must have as many items as the list of recipients");

            MatchCollection matches = Regex.Matches(msg, "#field.#");
            //Validate the length of the SMS. No longer than 160
            int baseLength = Regex.Replace(msg, "#field.#", String.Empty).Length;
            if (baseLength > 160)
                throw new ArgumentException("The base length of the SMS cannot surpass 160 characters. It now is : " + baseLength);
            if (matches.Count > 0)
                foreach (var fieldValues in fieldValuesList)
                {
                    int smsLength = baseLength;
                    foreach (var field in fieldValues)
                        smsLength += field.Length;
                    if (smsLength > 160)
                        throw new ArgumentException("The resulting length of the SMS cannot surpass 160 characters. An sms has length of " + smsLength);
                }

            if (fieldValuesList != null)
            {
                foreach (var fieldValues in fieldValuesList)
                    if ((fieldValues == null && matches.Count > 0) || (fieldValues.Length != matches.Count))
                        throw new ArgumentException("The number of field values for at least one recipient is different than the number of fields in the message");
            }
            else
                if (matches.Count > 0)
                    throw new ArgumentException("Cannot send null fieldValuesList when the message has field values");

            log.Debug("Before actually calling the CallSendService method");
            CallSendService(msg, recipients, fieldValuesList, matches);
        }

        /// <summary>
        /// For Locotel
        /// </summary>
        private static XDocument ConstructSmsXml(string msg, IDictionary<string, string> recipients, IList<string[]> fieldValuesList, int totalFields)
        {
            log.Debug("Starting the Xml construction method");
            //Create an XDocument and add the basic elements
            XDocument xd = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("msg",
                    new XElement("username", new XText(ConfigurationManager.AppSettings["LocotelUsername"])),
                    new XElement("password", new XText(ConfigurationManager.AppSettings["LocotelPassword"])),
                    new XElement("text", new XText(msg)),
                    new XElement("totalfields", new XText(totalFields.ToString()))
                )
            );

            for (int i = 0; i < recipients.Count; i++)
            {
                var recipient = recipients.ElementAt(i);

                //Add an element for each recipient and also add sub-elements fir uid and msisdn
                xd.Element("msg").Add(new XElement("recipient",
                    new XElement("uid", new XText(recipient.Key)),
                    new XElement("msisdn", new XText(recipient.Value)),
                    new XElement("mobile", new XText("einclusion"))));

                //Add the values for fields for the recipient just added
                if (fieldValuesList != null && fieldValuesList.Count > 0)
                    for (int j = 1; j <= fieldValuesList[i].Length; j++)
                        ((XElement)xd.Element("msg").LastNode).Add(new XElement("field" + j, new XText(fieldValuesList[i][j - 1])));
            }
            log.Debug("Xml Constructed");
            return xd;
        }

        /// <summary>
        /// For Locotel
        /// </summary>
        private static void CallSendService(string msg, IDictionary<string, string> recipients, IList<string[]> fieldValuesList, MatchCollection matches)
        {
            string url = "http://www.locosms.gr/xmlsend.php";

            if (ConfigurationManager.AppSettings["UrlSMS"] != null)
                url = Convert.ToString(ConfigurationManager.AppSettings["UrlSMS"]);

            XDocument xd = ConstructSmsXml(msg, recipients, fieldValuesList, matches.Count);

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

                    //Before parsing the response, check that it is correct
                    if (xdStatus.Element("results") == null || xdStatus.Element("results").Element("status") == null ||
                        (xdStatus.Element("results").Element("status").Value != "OK" && xdStatus.Element("results").Element("status").Value != "FAIL"))
                    {
                        log.ErrorFormat("The response from the SMS service was not the expected. Response : {0}", xdStatus.ToString(SaveOptions.DisableFormatting));
                        throw new ApplicationException("The response from the SMS service was not the expected");
                    }

                    if (xdStatus.Element("results").Element("status").Value == "FAIL")
                    {
                        log.ErrorFormat("Received FAIL status for SMS sending. Reason : [{0}].", (xdStatus.Element("results").Element("reason") != null ? xdStatus.Element("results").Element("reason").Value : "NO REASON WAS GIVEN"));
                        throw new ApplicationException("The SMS service failed in sending the SMS. Reason : " + (xdStatus.Element("results").Element("reason") != null ? xdStatus.Element("results").Element("reason").Value : "NO REASON WAS GIVEN"));
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
