using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.Utils
{
    public static class DispatcherFactory
    {
        private static IEmailDispatcher _emailDispatcher = null;
        private static ISmsDispatcher _smsDispatcher = null;

        public static IEmailDispatcher GetEmailDispatcher()
        {
            if (_emailDispatcher == null)
                _emailDispatcher = new EmailDispatcher();
            return _emailDispatcher;
        }

        public static ISmsDispatcher GetSmsDispatcher()
        {
            if (_smsDispatcher == null)
                _smsDispatcher = new VodafoneSmsDispatcher();
            return _smsDispatcher;
        }
    }
}
