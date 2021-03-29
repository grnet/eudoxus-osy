using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public interface IGetBookResponse
    {
        int numResults { get; set; }
        List<BookDTO> books { get; set; }      
    }
}
