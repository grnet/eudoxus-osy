using System.Collections.Generic;

namespace EudoxusOsy.BusinessModel
{
    public interface IGetBookResponseSlim
    {
        int numResults { get; set; }
        List<BookDTOSlim> books { get; set; }
    }
}