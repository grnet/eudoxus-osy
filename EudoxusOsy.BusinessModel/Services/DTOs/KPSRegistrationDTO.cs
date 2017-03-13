using System;

namespace EudoxusOsy.BusinessModel
{
    public class KPSRegistrationDTO
    {
        public int ID { get; set; }
        public long KpsRegistrationID { get; set; }
        public long KpsBookID { get; set; }
        public DateTime ReceivedAt { get; set; }
        public int SecretariatKpsID { get; set; }
        public DateTime SentByKpsAt { get; set; }
        public int Reason { get; set; }
        public int Amount { get; set; }
        public string RequestXML { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Status { get; set; }
    }


    public class RegistrationRequest
    {
        public long kpsregistrationId { get; set; }
        public long kpsBookId { get; set; }
        public long deliveryDate { get; set; }
        public long secreteriatId { get; set; }
        public string reason { get; set; }
        public long timestamp { get; set; }
    }


    public class LibraryRegistrationRequest
    {
        public long kpsregistrationId { get; set; }
        public int year { get; set; }
        public long academicId { get; set; }
        public long libraryId { get; set; }
        public long kpsBookId { get; set; }
        public long? bsaBookId { get; set; }
        public int amount { get; set; }
        public string reason { get; set; }
        public long reasonDate { get; set; }
        public long timestamp { get; set; }
    }
}
