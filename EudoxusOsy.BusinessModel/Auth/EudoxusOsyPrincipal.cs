using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public class EudoxusOsyIdentity : GenericIdentity
    {
        public EudoxusOsyIdentity(GenericIdentity identity)
            : base(identity) { }

        public EudoxusOsyIdentity(string name)
            : base(name) { }

        public EudoxusOsyIdentity(string name, string type)
            : base(name, type) { }

        public int ReporterID { get; set; }
        public string ContactName { get; set; }
    }

    public class EudoxusOsyPrincipal : GenericPrincipal
    {
        public EudoxusOsyPrincipal(IIdentity identity, string[] roles)
            : base(identity, roles) { }

        public EudoxusOsyIdentity Identity { get; set; }
    }
}
