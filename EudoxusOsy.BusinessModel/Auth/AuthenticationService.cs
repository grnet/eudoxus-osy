using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace EudoxusOsy.BusinessModel
{
    public static class AuthenticationService
    {
        #region [ MembershipUser Helper ]

        private static readonly ConcurrentDictionary<string, DateTime> _passwordsLastChangedAt = new ConcurrentDictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);

        private static DateTime? GetPasswordLastChangedAt(string username)
        {
            var lastChangedAt = DateTime.MinValue;
            if (!_passwordsLastChangedAt.TryGetValue(username, out lastChangedAt))
            {
                var user = Membership.GetUser(username);
                if (user != null)
                {
                    lastChangedAt = user.LastPasswordChangedDate;
                    _passwordsLastChangedAt.TryAdd(username, lastChangedAt);
                }
            }

            return lastChangedAt == DateTime.MinValue
                ? (DateTime?)null
                : lastChangedAt;
        }

        #endregion

        public static bool IsCookieValid()
        {
            var data = GetUserData();

            if (data.ReporterID == 0)
                return false;

            var passLastChanged = GetPasswordLastChangedAt(Thread.CurrentPrincipal.Identity.Name);
            if (!passLastChanged.HasValue)
                return true;
            else
            {
                if (data.PasswordLastChangedAt.HasValue)
                    return Math.Abs((passLastChanged.Value - data.PasswordLastChangedAt.Value).TotalSeconds) <= 1;
                else
                    return false;
            }
        }

        public static void InvalidateCookieInternal(string username)
        {
            var lastChangedAt = DateTime.MinValue;
            _passwordsLastChangedAt.TryRemove(username, out lastChangedAt);
        }

        public static void InvalidateCookie(string username, bool resetCookie)
        {
            InvalidateCookieInternal(username);

            if (resetCookie)
                LoginReporter(username);

            if (Config.EnableServerSync)
                ServerSyncQueueWorker.Current.AddInvalidateCookieSyncToQueue(username);
        }

        public static void LoginReporter(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be empty");
            }

            using (var uow = UnitOfWorkFactory.Create())
            {
                var rep = new ReporterRepository(uow).FindByUsername(username);

                if (rep == null)
                {
                    throw new ArgumentException(string.Format("Reporter not found for username '{0}'", username));
                }

                LoginReporter(rep);
            }
        }

        public static void LoginReporter(Reporter reporter)
        {
            if (reporter == null)
            {
                throw new ArgumentNullException("reporter");
            }

            var cookie = FormsAuthentication.GetAuthCookie(reporter.Username, true);
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            var userData = new UserTicketData() { ReporterID = reporter.ID, ContactName = reporter.ContactName, PasswordLastChangedAt = GetPasswordLastChangedAt(reporter.Username) };

            var newTicket = new FormsAuthenticationTicket(ticket.Version,
                ticket.Name,
                ticket.IssueDate,
                ticket.Expiration,
                true,
                userData.ToCookie(),
                ticket.CookiePath);

            cookie.Value = FormsAuthentication.Encrypt(newTicket);
            HttpContext.Current.Response.Cookies.Set(cookie);

            ClearRoleCookie();
        }

        public static UserTicketData GetUserData()
        {
            if (HttpContext.Current == null || HttpContext.Current.Request == null || !Thread.CurrentPrincipal.Identity.IsAuthenticated)
                return new UserTicketData();

            var enCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            var ticket = FormsAuthentication.Decrypt(enCookie.Value);

            return UserTicketData.FromCookie(ticket.UserData);
        }

        public static void ClearRoleCookie()
        {
            HttpCookie rolesCookie = new HttpCookie(Roles.CookieName + "_") { Expires = DateTime.Today.AddDays(-1) };
            HttpContext.Current.Response.Cookies.Set(rolesCookie);
        }
    }
}
