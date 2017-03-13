using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public class InvoiceItemCollection : List<InvoiceItem>
    {
        public InvoiceItemCollection() : base() { }
        public InvoiceItemCollection(IEnumerable<InvoiceItem> collection) : base(collection) { }
        public InvoiceItemCollection(int capacity) : base(capacity) { }
    }

    public class InvoiceItem
    {
        public Guid ID { get; set; }
        public int OrderIndex { get; set; }
        public int? InvoiceID { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Comments { get; set; }

        public string ToExportString()
        {
            return string.Format("{0}. Αριθμός Παραστατικού: {1}, Ημ/νία: {2}, Ποσό (χωρίς ΦΠΑ): {3}, Παρατηρήσεις: {4}",
                OrderIndex, InvoiceNumber, Date, Amount, Comments);
        }
    }
}
