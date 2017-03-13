using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.Services.Configuration
{
    public class ServicesConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("ticketExpiration", IsRequired = false, DefaultValue = "23:59:59")]
        public TimeSpan TicketExpiration
        {
            get { return (TimeSpan)this["ticketExpiration"]; }
            set { this["ticketExpiration"] = value; }
        }

        [ConfigurationProperty("maxPageSize", IsRequired = false, DefaultValue = 50)]
        public int MaxPageSize
        {
            get { return (int)this["maxPageSize"]; }
            set { this["maxPageSize"] = value; }
        }

        [ConfigurationProperty("defaultPageSize", IsRequired = false, DefaultValue = 20)]
        public int DefaultPageSize
        {
            get { return (int)this["defaultPageSize"]; }
            set { this["defaultPageSize"] = value; }
        }
    }
}
