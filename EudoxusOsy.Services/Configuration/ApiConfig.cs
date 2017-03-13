using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EudoxusOsy.Services.Configuration;

namespace EudoxusOsy.Services
{
    public static class ApiConfig
    {
        private static ServicesConfiguration _cur = null;

        static ApiConfig()
        {
            _cur = ConfigurationManager.GetSection("webServices") as ServicesConfiguration;
            if (_cur == null)
                throw new Exception("Web Services Configuration Error: could not find 'webServices' configuration section.");
        }

        public static ServicesConfiguration Current { get { return _cur; } }
    }
}
