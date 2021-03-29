using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public class SyncPublisherDto
    {
        public int PublisherKpsID { get; set; }
        public int PublisherType { get; set; }
        public string PublisherName { get; set; }
        public string PublisherTradeName { get; set; }
        public string PublisherAFM { get; set; }
        public string PublisherDOY { get; set; }
        public string PublisherPhone { get; set; }
        public string PublisherMobilePhone { get; set; }
        public string PublisherFax { get; set; }
        public string PublisherEmail { get; set; }
        public string PublisherUrl { get; set; }
        public string PublisherAddress { get; set; }
        public string PublisherZipCode { get; set; }
        public int PublisherCityID { get; set; }
        public int PublisherPrefectureID { get; set; }
        public string LegalPersonName { get; set; }
        public string LegalPersonPhone { get; set; }
        public string LegalPersonEmail { get; set; }
        public int? LegalPersonIdentificationType { get; set; }
        public string LegalPersonIdentificationNumber { get; set; }
        public string LegalPersonIdentificationIssuer { get; set; }
        public DateTime? LegalPersonIdentificationIssueDate { get; set; }
        public bool? IsSelfRepresented { get; set; }
        public int? SelfPublisherIdentificationType { get; set; }
        public string SelfPublisherIdentificationNumber { get; set; }
        public string SelfPublisherIdentificationIssuer { get; set; }
        public DateTime? SelfPublisherIdentificationIssueDate { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactMobilePhone { get; set; }
        public string ContactEmail { get; set; }
        public int ContactIdentificationType { get; set; }
        public string ContactIdentificationNumber { get; set; }
        public string ContactIdentificationIssuer { get; set; }
        public DateTime ContactIdentificationIssueDate { get; set; }
        public string AlternateContactName { get; set; }
        public string AlternateContactPhone { get; set; }
        public string AlternateContactMobilePhone { get; set; }
        public string AlternateContactEmail { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string Email { get; set; }
        public bool IsActivated { get; set; }
        public int VerificationStatus { get; set; }
        public bool? HasLogisticBooks { get; set; }
    }
}
