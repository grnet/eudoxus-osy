using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.Utils
{
    public interface ISmsDispatcher
    {
        void Send(string msg, string recipID, string recipNumber, string[] fieldValues);
    }
}
