using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.ComponentModel;
using EudoxusOsy.BusinessModel;
using System.Web.Security;
using Imis.Domain;
using System.Threading;
using Newtonsoft.Json;
using System.Web.Caching;

namespace EudoxusOsy.Portal.PortalServices
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class Services : System.Web.Services.WebService
    {
        #region [ Cascading DropDowns ]

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public object GetCities(string prefectureID)
        {
            var cities = CacheManager.GetOrderedCities(int.Parse(prefectureID));

            return cities.Select(city => new { id = city.ID, name = city.Name });
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public object GetDepartments(string institutionID)
        {
            var academics = CacheManager.GetOrderedDepartments(int.Parse(institutionID));

            return academics.Select(department => new { id = department.ID, name = department.Name });
        }

        #endregion

        #region [ Registration ]

        [WebMethod]
        public bool UserNameExists(string userName)
        {
            if (Membership.GetUser(userName) == null)
                return false;

            return true;
        }

        [WebMethod]
        public bool EmailExists(string email)
        {
            if (string.IsNullOrEmpty(Membership.GetUserNameByEmail(email)))
                return false;

            return true;
        }

        #endregion
    }
}