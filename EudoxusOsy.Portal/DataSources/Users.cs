using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Web.Security;
using EudoxusOsy.BusinessModel;
using Imis.Domain;
using Imis.Domain.EF;

namespace EudoxusOsy.Portal.DataSources
{
    [DataObject(true)]
    public class Users
    {
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<string> FindUsersInRoles(string[] roleNames)
        {
            List<string> result = new List<string>();

            foreach (string roleName in roleNames)
            {
                result.AddRange(Roles.GetUsersInRole(roleName));
            }

            return result.Distinct().OrderBy(x => x).ToList();
        }
    }
}