using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.Services
{
    public class PagedResult<T>
    {
        public PagedResult() { }

        public PagedResult(IEnumerable<T> items, int totalCount, int startIndex)
        {
            Items = items.ToArray();
            StartIndex = startIndex;
            ResultCount = Items.Count();
            TotalCount = totalCount;
        }

        public T[] Items { get; set; }
        public int StartIndex { get; set; }
        public int ResultCount { get; set; }
        public int TotalCount { get; set; }
    }
}
