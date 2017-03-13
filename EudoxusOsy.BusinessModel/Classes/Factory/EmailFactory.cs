using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace EudoxusOsy.BusinessModel
{
    public static class EmailFactory
    {
        #region [ Helpers ]

        private static string ReplaceVars(string body, Dictionary<string, string> values)
        {
            string bodyReplaced = body;
            foreach (var value in values)
            {
                bodyReplaced = bodyReplaced.Replace(string.Format("%{0}%", value.Key.ToUpper()), value.Value);
            }

            return bodyReplaced;
        }

        private static Email CreateEmail(enEmailType emailType, Dictionary<string, string> values, int? reporterID, string to, int? entityID = null, string senderEmail = null, string ccedEmails = null, bool showMailFooter = true)
        {
            var mailDetails = MailDetailsReader.GetMailDetails(emailType);

            var email = new Email();
            email.EmailEntityType = enEmailEntityType.None;
            email.DeliveryStatus = enDeliveryStatus.Pending;
            email.ReporterID = reporterID;
            email.Type = emailType;
            email.SenderEmailAddress = string.IsNullOrEmpty(senderEmail) ? DefaultSenderEmail : senderEmail;
            email.CCedEmailAddresses = ccedEmails;
            email.EmailAddress = to;
            email.Subject = mailDetails.Subject;
            email.Body = ReplaceVars(mailDetails.Body, values);

            if (showMailFooter)
            {
                email.Body += MailDetailsReader.GetMailFooter();
            }

            if (entityID.HasValue)
            {
                email.EntityID = entityID;
            }

            return email;
        }

        private static List<string> CreateContactEmails(Reporter reporter)
        {
            List<string> emails = new List<string>();

            emails.Add(reporter.Email);

            return emails.Distinct().ToList();
        }

        #endregion

        public static Email GetCustomMessage(int? reporterID, string to, string subject, string message)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("message", message);
            values.Add("subject", subject);

            var email = CreateEmail(enEmailType.CustomMessage, values, reporterID, to);
            email.Subject = ReplaceVars(email.Subject, values);
            return email;
        }

        public static Email GetCustomMessage(Reporter reporter, string subject, string message)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("message", message);
            values.Add("subject", subject);

            var email = CreateEmail(enEmailType.CustomMessage, values, reporter.ID, reporter.Email);
            email.Subject = ReplaceVars(email.Subject, values);
            return email;
        }

        public static Email GetIBANChanges(Reporter reporter, string ibanChanges)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("ibanChanges", ibanChanges);

            return CreateEmail(enEmailType.IbanChanges, values, reporter.ID, Config.IbanChangesEmailTo);
        }

        public static Email GetDailyUpdateReceiptsFromAuditReceiptsReport(string message)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("message", message);
            values.Add("subject", string.Format("Ενημέρωση παραδόσεων ({0})", DateTime.Today.AddDays(-1).ToShortDateString()));

            return CreateEmail(enEmailType.DailyAuditReceiptsProcessReport, values, 1, Config.DailyUpdateReceiptsFromAuditReceiptsEmailTo);
        }

        public static Email GetServiceBookChangesReport(string subject, string message)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("message", message);
            values.Add("subject", subject);

            //TODO : Change receipients list -> Config.DailyUpdateReceiptsFromAuditReceiptsEmailTo
            return CreateEmail(enEmailType.ServiceBookChangesReport, values, 1, Config.DailyUpdateReceiptsFromAuditReceiptsEmailTo);
        }

        #region [ XML emails configuration ]

        private const string DefaultSenderEmail = "no-reply@eudoxus.gr";

        private enum enXmlElementNames
        {
            Mail,
            Subject,
            Body,
            Footer,
            Category
        }

        private class MailDetails
        {
            public string Subject;
            public string Body;
        }

        private static class MailDetailsReader
        {
            private static XElement cachedMailsDetails = null;
            private static XElement CachedMailsDetails
            {
                get
                {
                    if (cachedMailsDetails == null)
                    {
                        if (HttpContext.Current == null)
                            cachedMailsDetails = XElement.Parse(System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Mails.xml")));
                        else
                            cachedMailsDetails = XElement.Parse(System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/Mails.xml")));
                    }
                    return cachedMailsDetails;
                }
            }

            public static MailDetails GetMailDetails(enEmailType emailType)
            {
                return (from z in CachedMailsDetails.Descendants(enXmlElementNames.Mail.ToString())
                        where (string)z.Attributes(enXmlElementNames.Category.ToString()).Single() == emailType.ToString()
                        select new MailDetails()
                        {
                            Subject = z.Descendants(enXmlElementNames.Subject.ToString()).Single().Value,
                            Body = z.Descendants(enXmlElementNames.Body.ToString()).Single().Value
                        }).Single();
            }

            public static string GetMailFooter()
            {
                return (from z in CachedMailsDetails.Elements(enXmlElementNames.Footer.ToString())
                        select ((XCData)z.FirstNode).Value).Single();
            }
        }

        #endregion
    }
}
