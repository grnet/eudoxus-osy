using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using EudoxusOsy.BusinessModel;
using System.Web.Security;
using System.Threading;

namespace EudoxusOsy.BusinessModel
{
    public class EudoxusOsyRoleProvider : SqlRoleProvider
    {
        #region [ Helpers ]

        private DBEntities CreateContext()
        {
            DBEntities context = new DBEntities();
            context.ContextOptions.LazyLoadingEnabled = true;
            return context;
        }

        private string[] GetRolesFromCookie()
        {
            var cookie = HttpContext.Current.Request.Cookies[Roles.CookieName + "_"];
            if (cookie == null)
            {
                cookie = HttpContext.Current.Response.Cookies[Roles.CookieName + "_"];
            }
            if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
            {
                var ticker = FormsAuthentication.Decrypt(cookie.Value);
                string[] roles = ticker.Name.Split(';');
                return roles;
            }
            else
                return null;
        }

        private void SetRolesToCookie(IList<string> roles)
        {
            var t = new FormsAuthenticationTicket(string.Join(";", roles), true, Roles.CookieTimeout);
            var c = new HttpCookie(Roles.CookieName + "_", FormsAuthentication.Encrypt(t));
            c.Expires = DateTime.Now.AddMinutes(Roles.CookieTimeout);
            HttpContext.Current.Response.Cookies.Set(c);
        }

        private bool IsCurrentUser(string username)
        {
            return Thread.CurrentPrincipal.Identity.IsAuthenticated && Thread.CurrentPrincipal.Identity.Name == username;
        }

        #endregion

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            using (var context = CreateContext())
            {
                var rolesToAdd = context.Roles.Where(r => roleNames.Contains(r.RoleName)).ToList();
                var members = context.Reporters.Include("Roles").Where(m => usernames.Contains(m.Username)).ToList();
                foreach (var user in members)
                {
                    foreach (var role in rolesToAdd)
                    {
                        if (!user.Roles.Any(x => x.RoleName == role.RoleName))
                        {
                            user.Roles.Add(role);
                        }
                    }
                }
                context.SaveChanges();
            }
        }

        public override void CreateRole(string roleName)
        {
            if (RoleExists(roleName))
                return;
            Role r = new Role();
            r.RoleName = roleName;
            using (var context = CreateContext())
            {
                context.MarkAsNew(r);
                context.SaveChanges();
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            return false;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            using (var context = CreateContext())
            {
                var role = context.Roles.FirstOrDefault(x => x.RoleName == roleName);
                if (role != null)
                    return role.Reporters.Select(x => x.Username).ToArray();
                return new string[0];
            }
        }

        public override string[] GetAllRoles()
        {
            using (var context = CreateContext())
            {
                return context.Roles.Select(x => x.RoleName).ToArray();
            }
        }

        public override string[] GetRolesForUser(string username)
        {
            if (IsCurrentUser(username))
            {
                var roles = GetRolesFromCookie();
                if (roles != null)
                    return roles;
            }

            using (var context = CreateContext())
            {
                var roles = context.Reporters.Where(x => x.Username == username).SelectMany(x => x.Roles).ToList();
                if (roles.Count != 0)
                {
                    var roleNames = roles.Select(x => x.RoleName).ToArray();

                    if (IsCurrentUser(username))
                        SetRolesToCookie(roleNames);

                    return roleNames;
                }
                return new string[0];
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            using (var context = CreateContext())
            {
                return context.Reporters.Where(x => x.Roles.Any(y => y.RoleName == roleName)).Select(x => x.Username).ToArray();
            }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            if (IsCurrentUser(username))
            {
                var roles = GetRolesFromCookie();
                if (roles != null)
                    return roles.Any(x => x == roleName);
                else
                {
                    roles = GetRolesForUser(username);
                    return roles.Any(x => x == roleName);
                }
            }

            using (var context = CreateContext())
            {
                return context.Reporters.Any(m => m.Username == username && m.Roles.Any(r => r.RoleName == roleName));
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            using (var context = CreateContext())
            {
                //TODO: Improve with one Query
                foreach (string uname in usernames)
                {
                    string u = uname;
                    var user = context.Reporters.Include("Roles").FirstOrDefault(x => x.Username == u);
                    if (user == null)
                        continue;

                    var roles = user.Roles.ToList();
                    for (int i = roles.Count - 1; i >= 0; i--)
                    {
                        var userRole = roles[i];
                        if (roleNames.Any(r => r == userRole.RoleName))
                        {
                            user.Roles.Remove(userRole);
                        }
                    }
                }
                context.SaveChanges();
            }
        }

        public override bool RoleExists(string roleName)
        {
            using (var context = CreateContext())
            {
                return context.Roles.Any(x => x.RoleName == roleName);
            }
        }

        public override string ApplicationName
        {
            get
            {
                return "EudoxusOsyApp";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public static bool IsAuthorizedEditorUser()
        {
            return HttpContext.Current.User.IsInRole(RoleNames.MinistryPayments) 
                || HttpContext.Current.User.IsInRole(RoleNames.SuperMinistry) 
                || HttpContext.Current.User.IsInRole(RoleNames.SystemAdministrator);
        }

        public static bool IsAuthorizedMinistryUserForView()
        {
            return IsAuthorizedEditorUser()
                || HttpContext.Current.User.IsInRole(RoleNames.MinistryAuditor);
        }
    }
}
