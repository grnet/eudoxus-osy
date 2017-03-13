using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using log4net;
using System.Configuration;

namespace EudoxusOsy.Utils
{
    public class EmailDispatcher : IEmailDispatcher
    {
        private static readonly ILog log = LogManager.GetLogger("MailSender");

        /// <summary>
        /// When true, no mails are sent, they are only logged.
        /// </summary>

        private static bool? _enableEmail = null;

        private static bool DebugMode
        {
            get
            {
                if (_enableEmail == null)
                    _enableEmail = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableEmail"]);
                return !_enableEmail.Value;
            }
        }

        public void Send(string from, string to, string subject, string body, bool htmlBody)
        {
            Send(from, to, new string[] { }, subject, body, htmlBody);
        }

        public void Send(string from, string to, string[] ccs, string subject, string body, bool htmlBody)
        {
            try
            {
                if (string.IsNullOrEmpty(from))
                    throw new ArgumentException("Will not send email from invalid address");
                else if (string.IsNullOrEmpty(to))
                    throw new ArgumentException("Will not send email to invalid address");
                else
                {
                    if (!htmlBody)
                        body = body.Replace("«", "\"").Replace("»", "\"").Replace("&#171;", "\"").Replace("&#187;", "\"");

                    if (DebugMode)
                        log.InfoFormat("From {0}, to {1}, subject {2}, body {3}", from, to, subject, body);
                    else
                    {
                        MailMessage m = new MailMessage(from, to, subject, body);
                        foreach (var item in ccs)
                            m.CC.Add(new MailAddress(item));

                        SmtpClient sc = new SmtpClient();

                        m.IsBodyHtml = htmlBody;

                        sc.Send(m);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                log.Error(ex.Message, ex);
                throw;
            }
        }

        public void SendWithAttachments(string from, string to, string ccs, string subject, string body, bool htmlBody, List<string> attachments)
        {
            try
            {
                if (string.IsNullOrEmpty(from))
                    throw new ArgumentException("Will not send email from invalid address");
                else if (string.IsNullOrEmpty(to))
                    throw new ArgumentException("Will not send email to invalid address");
                else
                {
                    if (!htmlBody)
                        body = body.Replace("«", "\"").Replace("»", "\"").Replace("&#171;", "\"").Replace("&#187;", "\"");

                    if (DebugMode)
                        log.InfoFormat("From {0}, to {1}, subject {2}, body {3}", from, to, subject, body);
                    else
                    {
                        MailMessage m = new MailMessage();
                        m.From = new MailAddress(from);
                        m.Subject = subject;
                        m.Body = body;

                        foreach (var address in to.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            m.To.Add(address);
                        }

                        if (!string.IsNullOrEmpty(ccs))
                        {
                            foreach (var item in ccs.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                                m.CC.Add(new MailAddress(item));
                        }

                        SmtpClient sc = new SmtpClient();

                        m.IsBodyHtml = htmlBody;

                        foreach (string attach in attachments)
                        {
                            Attachment attachment;
                            attachment = new Attachment(attach);
                            m.Attachments.Add(attachment);
                        }

                        sc.Send(m);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                log.Error(ex.Message, ex);
                throw;
            }
        }
    }
}
