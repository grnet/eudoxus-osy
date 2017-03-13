using Imis.Domain;
using System;
using System.Collections.Generic;

namespace EudoxusOsy.BusinessModel
{
    public class KpsRegistrationService
    {
        #region [ Constructors ]

        protected IUnitOfWork UnitOfWork { get; private set; }

        public KpsRegistrationService()
        {
            UnitOfWork = UnitOfWorkFactory.Create();
        }

        public KpsRegistrationService(IUnitOfWork uow)
        {
            UnitOfWork = uow;
        }

        #endregion

        #region [ Services ]

        public bool InsertRegistration(RegistrationRequest dto)
        {
            AuditReceipt receipt = new AuditReceipt();
            receipt.RegistrationKpsID = dto.kpsregistrationId;
            receipt.KpsBookID = dto.kpsBookId;
            receipt.ReceivedAt = BusinessHelper.UnixTimeStampToDateTime(dto.deliveryDate);
            receipt.SecreteriatKpsID = dto.secreteriatId;
            receipt.SentByKpsAt = BusinessHelper.UnixTimeStampToDateTime(dto.timestamp);
            receipt.Reason = dto.reason == "DELIVERED" ? enAuditReceiptReason.Delivered : enAuditReceiptReason.Canceled;
            receipt.Amount = 1;
            receipt.Request = new Serializer<RegistrationRequest>().Serialize(dto, true);

            receipt.CreatedBy = "sysadmin";
            receipt.CreatedAt = DateTime.Now;
            //status

            UnitOfWork.MarkAsNew(receipt);
            UnitOfWork.Commit();
            return true;
        }

        public bool LibraryRegistration(LibraryRegistrationRequest dto)
        {
            AuditReceipt receipt = new AuditReceipt();
            receipt.RegistrationKpsID = dto.kpsregistrationId;
            receipt.KpsBookID = dto.kpsBookId;
            receipt.ReceivedAt = BusinessHelper.UnixTimeStampToDateTime(dto.reasonDate);
            receipt.SecreteriatKpsID = dto.libraryId;
            receipt.SentByKpsAt = BusinessHelper.UnixTimeStampToDateTime(dto.timestamp);
            receipt.Reason = dto.reason == "DELIVERED" ? enAuditReceiptReason.Delivered : enAuditReceiptReason.Canceled;
            receipt.Amount = dto.amount;
            receipt.Request = new Serializer<LibraryRegistrationRequest>().Serialize(dto, true);

            receipt.CreatedBy = "sysadmin";
            receipt.CreatedAt = DateTime.Now;
            //status

            UnitOfWork.MarkAsNew(receipt);
            UnitOfWork.Commit();
            return true;
        }

        public bool PostRegistrationXML(string finalXml)
        {
            try
            {
                pricingRegistration xml = new Serializer<pricingRegistration>().Deserialize(finalXml);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion
    }

    public class pricingRegistration
    {
        public int year { get; set; }
        public string period { get; set; }
        public long creationDate { get; set; }


        public List<registration> registrations { get; set; }
        public List<libraryregistration> libraryregistrations { get; set; }

    }
    public class libraryregistration
    {
        public long kpsregistration_id;
        public long kpsBook_id;
        public long bsabook_id;
        public int academic_id;
        public int library_id;
        public int amount;
        public string reason;
        public long reasondate;
    }
    public class registration
    {
        public long kpsregistration_id;
        public long kpsBook_id;
        public long bsabook_id;
        public long deliveryDate;
        public long secretariat_id;
        public string reason;
        public long timestamp;
    }
}
