using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Services.Resources;

namespace EudoxusOsy.Services
{
    public static class Messages
    {
        public static string GetStatusCode(enStatusCode code)
        {
            var resourceKey = code.ToString();
            var msg = StatusMessages.ResourceManager.GetString(resourceKey);

            return string.IsNullOrEmpty(msg) ? resourceKey : msg;
        }
    }
}
