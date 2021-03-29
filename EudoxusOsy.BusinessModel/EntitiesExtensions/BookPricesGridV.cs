using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public partial class BookPricesGridV
    {
        public string TitleView
        {
            get { return Title.Replace("\"", "'"); }
        }
    }
}
