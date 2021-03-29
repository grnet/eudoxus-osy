using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public class BookInCatalogInfo
    {
        public int ID { get; set; }
        public int BookKpsID { get; set; }
        public string Department{ get; set; }
        public int BookCount { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public decimal PaymentPrice { get; set; }
        public decimal? TotalAmount { get; set; }
        public string ISBN { get; set; }
        public string Publisher { get; set; }
        public string Comments { get; set; }
        public string PageCount { get; set;}
        public string Subtitle { get; set; }
        public int DiscountID { get; set; }
        //public int CatalogType { get; set; }
    }
}
