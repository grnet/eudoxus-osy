using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace EudoxusOsy.BusinessModel
{
    public static class SMSFactory
    {
        #region [ Helpers ]

        private static int maxNameLength = 56;

        private static string AddZeroPadding(this string s, int maxLength)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            int sLength = s.Length;
            int numberOfZeros = maxLength - sLength;

            string sPadded = "";

            for (int i = 0; i < numberOfZeros; i++)
                sPadded += "0";

            sPadded += s;

            return sPadded;
        }

        private static SMS CreateSms(enSMSType smsType, string[] fieldValues, int reporterID, string mobilePhone, int? entityID = null)
        {
            var smsDetails = SMSDetailsReader.GetSMSDetails(smsType);

            var sms = new SMS();
            sms.DeliveryStatus = enDeliveryStatus.Pending;
            sms.ReporterID = reporterID;
            sms.EntityID = entityID;
            sms.TypeInt = (int)smsType;
            sms.SendID = reporterID.ToString().AddZeroPadding(8) + DateTime.Now.ToString("yyMMddhhmmss") + Convert.ToInt32(smsType);
            sms.ReporterNumber = "30" + mobilePhone;
            sms.Msg = smsDetails.Message;
            sms.FieldValues = fieldValues;
            return sms;
        }

        #endregion

        public static SMS GetCustomMessage(Reporter r, string customMessage)
        {
            //return CreateSms(enSMSType.CustomMessage, new string[] { customMessage }, r.ID, r.ContactMobilePhone);
            return null;
        }

        #region [ XML SMSs configuration ]

        private enum enXmlElementNames
        {
            SMS,
            Message,
            Category
        }

        private class SMSDetails
        {
            public string Message;
        }

        private static class SMSDetailsReader
        {
            public static SMSDetails GetSMSDetails(enSMSType smsType)
            {
                return (from z in CachedSMSDetails.Descendants(enXmlElementNames.SMS.ToString())
                        where (string)z.Attributes(enXmlElementNames.Category.ToString()).Single() == smsType.ToString()
                        select new SMSDetails()
                        {
                            Message = z.Descendants(enXmlElementNames.Message.ToString()).Single().Value
                        }).Single();
            }

            private static XElement cachedSMSDetails = null;
            private static XElement CachedSMSDetails
            {
                get
                {
                    if (HttpContext.Current == null)
                        cachedSMSDetails = XElement.Parse(System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/SMSs.xml")));
                    else
                        cachedSMSDetails = XElement.Parse(System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/SMSs.xml")));
                    return cachedSMSDetails;
                }
            }
        }

        #endregion
    }
}
