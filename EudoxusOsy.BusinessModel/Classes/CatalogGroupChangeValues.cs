using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public class CatalogGroupChangeValues
    {
        public bool IsLocked { get; set; }
        public int? ReporterID { get; set; }
        public DateTime? SentAt { get; set; }
        public string PdfLink { get; set; }
        public decimal? Amount { get; set; }
        public enCatalogLogAction? Action { get; set; }
        public string NoticeNumber { get; set; }
        public string AcademicYearString { get; set; }
        public string SupplierAFM { get; set; }
        public string SupplierDOY { get; set; }
        public string SupplierIBAN { get; set; }
        public string SupplierEmail { get; set; }
        public string SupplierName { get; set; }
        public string AcademicInstitutionName { get; set; }
        public decimal? PaymentAmount { get; set; }
        public string StateLabel { get; set; }
        public decimal? GrandTotalAmount { get; set; }
        public int IncomeTaxPerc { get; set; }

        public bool HideVat { get; set; }

        public List<BookInCatalogInfo> IncludedCatalogs;
        public List<DeductionPdfObject> Deductions;
    }

    public class DeductionPdfObject
    {
        public string DeductionType { get; set;}
        public decimal DeductionAmount { get; set; }
    }

    //public class CatalogForPdf
    //{
    //    /// <summary>
    //    /// the catalog ID - το ID της διανομής
    //    /// </summary>
    //    public int ID { get; set; }
    //    /// <summary>
    //    /// The title of the book - Ο τίτλος του βιβλίου
    //    /// </summary>
    //    public string BookTitle { get; set; }
    //    /// <summary>
    //    /// The names of the authors of the book - Τα ονόματα των συγγραφέων του βιβλίου
    //    /// </summary>
    //    public string Authors { get; set; }
    //    /// <summary>
    //    /// The department of the catalog - Το τμήμα στο οποίο αφορά η διανομή
    //    /// </summary>
    //    public string Department { get; set; }

    //    /// <summary>
    //    /// The price of the book - Η τιμή του βιβλίου
    //    /// </summary>
    //    public decimal BookPrice { get; set; }

    //    /// <summary>
    //    /// Book Count - Αριθμός αντιτύπων
    //    /// </summary>
    //    public int BookCount { get; set; }
    //}
}
