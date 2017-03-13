using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public partial class CatalogLog
    {
        public enCatalogLogAction Action
        {
            get { return (enCatalogLogAction)ActionInt; }
            set
            {
                if (ActionInt != (int)value)
                    ActionInt = (int)value;
            }
        }
    }
}
