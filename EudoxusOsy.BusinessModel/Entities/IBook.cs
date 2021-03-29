using System;
using System.Data.Objects.DataClasses;

namespace EudoxusOsy.BusinessModel
{
    public interface IBook
    {
        string Author { get; set; }
        int BookKpsID { get; set; }
        enBookType BookType { get; set; }
        EntityCollection<Catalog> Catalogs { get; set; }
        string Comments { get; set; }
        DateTime CreatedAt { get; set; }
        string CreatedBy { get; set; }
        EntityCollection<Discount> Discounts { get; set; }
        int? FirstRegistrationYear { get; set; }
        bool HasPendingPriceVerification { get; }
        bool HasUnexpectedPriceChange { get; }
        int? HasUnexpectedPriceChangePhaseID { get; set; }
        int ID { get; set; }
        bool IsActive { get; set; }
        string ISBN { get; set; }        
        int Pages { get; set; }
        bool? PendingCommitteePriceVerification { get; set; }
        string Publisher { get; set; }
        EntityCollection<Receipt> Receipts { get; set; }
        string Subtitle { get; set; }
        int? SupplierCode { get; set; }
        string Title { get; set; }
        DateTime? UpdatedAt { get; set; }
        string UpdatedBy { get; set; }
    }
}