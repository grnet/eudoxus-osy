using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.Utils
{
    public interface IEmailDispatcher
    {
        void Send(string from, string to, string subject, string body, bool htmlBody);
        void Send(string from, string to, string[] ccs, string subject, string body, bool htmlBody);
    }
}
