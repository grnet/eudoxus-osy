using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imis.Domain;

namespace EudoxusOsy.BusinessModel.Flow
{
    public class CatalogGroupTriggerParams
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public string Username { get; set; }
        public string Comments { get; set; }
        public DateTime? SentToYDEDate { get; set; }
    }
}
