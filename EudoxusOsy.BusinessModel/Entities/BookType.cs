using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public partial class Book
    {
        public enBookType BookType
        {
            get { return (enBookType)BookTypeInt; }
            set
            {
                if (BookTypeInt != (int)value)
                    BookTypeInt = (int)value;
            }
        }
    }
}
