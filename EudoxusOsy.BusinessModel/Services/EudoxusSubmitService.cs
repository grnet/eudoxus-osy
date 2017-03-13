using Imis.Domain;
using System;
using System.Web.Security;

namespace EudoxusOsy.BusinessModel
{
    public class EudoxusSubmitService
    {
        #region [ Constructors ]

        protected IUnitOfWork UnitOfWork { get; private set; }

        public EudoxusSubmitService()
        {
            UnitOfWork = UnitOfWorkFactory.Create();
        }

        public EudoxusSubmitService(IUnitOfWork uow)
        {
            UnitOfWork = uow;
        }

        #endregion

        #region [ Services ]

        public Supplier SyncPublisher(SyncPublisherDto dto, out bool? supplierCreated)
        {
            var supplier = new SupplierRepository(UnitOfWork).FindByKpsID(dto.PublisherKpsID, x => x.SupplierDetail, x => x.Reporter);

            if (supplier == null)
            {
                CreateSupplier(dto);
                supplierCreated = true;
            }
            else
            {
                UpdateSupplier(supplier, dto);
                supplierCreated = false;
            }

            return supplier;
        }

        private void CreateSupplier(SyncPublisherDto dto)
        {
            var supplier = new Supplier()
            {
                SupplierKpsID = dto.PublisherKpsID,
                SupplierTypeInt = dto.PublisherType,
                Name = dto.PublisherName,
                TradeName = dto.PublisherTradeName,
                AFM = dto.PublisherAFM,
                DOY = dto.PublisherDOY,
                Email = dto.Email,
                VerificationStatusInt = dto.VerificationStatus,
                IsActivated = dto.IsActivated,
                Status = enSupplierStatus.Active,
                CreatedAt = DateTime.Now,
                CreatedBy = "sysadmin"
            };

            var supplierDetail = new SupplierDetail()
            {
                ID = dto.PublisherKpsID,
                PublisherPhone = dto.PublisherPhone,
                PublisherMobilePhone = dto.PublisherMobilePhone,
                PublisherFax = dto.PublisherFax,
                PublisherEmail = dto.PublisherEmail,
                PublisherUrl = dto.PublisherUrl,
                PublisherAddress = dto.PublisherAddress,
                PublisherZipCode = dto.PublisherZipCode,
                PublisherCityID = dto.PublisherCityID,
                PublisherPrefectureID = dto.PublisherPrefectureID,
                LegalPersonName = dto.LegalPersonName,
                LegalPersonPhone = dto.LegalPersonPhone,
                LegalPersonEmail = dto.LegalPersonEmail,
                LegalPersonIdentificationTypeInt = dto.LegalPersonIdentificationType,
                LegalPersonIdentificationNumber = dto.LegalPersonIdentificationNumber,
                LegalPersonIdentificationIssuer = dto.LegalPersonIdentificationIssuer,
                LegalPersonIdentificationIssueDate = dto.LegalPersonIdentificationIssueDate,
                IsSelfRepresented = dto.IsSelfRepresented,
                SelfPublisherIdentificationTypeInt = dto.SelfPublisherIdentificationType,
                SelfPublisherIdentificationNumber = dto.SelfPublisherIdentificationNumber,
                SelfPublisherIdentificationIssuer = dto.SelfPublisherIdentificationIssuer,
                SelfPublisherIdentificationIssueDate = dto.SelfPublisherIdentificationIssueDate,
                ContactIdentificationTypeInt = dto.ContactIdentificationType,
                ContactIdentificationNumber = dto.ContactIdentificationNumber,
                ContactIdentificationIssuer = dto.ContactIdentificationIssuer,
                ContactIdentificationIssueDate = dto.ContactIdentificationIssueDate,
                AlternateContactName = dto.AlternateContactName,
                AlternateContactPhone = dto.AlternateContactPhone,
                AlternateContactMobilePhone = dto.AlternateContactMobilePhone,
                AlternateContactEmail = dto.AlternateContactEmail
            };

            supplier.SupplierDetail = supplierDetail;

            var reporter = new Reporter()
            {
                ID = dto.PublisherKpsID,
                ReporterType = enReporterType.Supplier,
                ContactName = dto.ContactName,
                ContactPhone = dto.ContactPhone,
                ContactMobilePhone = dto.ContactMobilePhone,
                ContactEmail = dto.ContactEmail,
                Username = dto.Username,
                Email = dto.Email,
                VerificationStatusInt = dto.VerificationStatus,
                IsActivated = dto.IsActivated,
                CreatedAt = DateTime.Now.Date,
                CreatedBy = "sysadmin"
            };

            supplier.Reporter = reporter;

            try
            {
                MembershipUser user;
                MembershipCreateStatus status;

                user = Membership.CreateUser(dto.Username, dto.Password, dto.Email, null, null, true, out status);

                if (user != null)
                {
                    try
                    {
                        var aspnet_user = new AspnetUsersRepository(UnitOfWork).FindByUsername(user.UserName);
                        var aspnet_membership = new AspnetMembershipRepository(UnitOfWork).FindByUserID(aspnet_user.UserId);

                        aspnet_membership.Password = dto.Password;
                        aspnet_membership.PasswordSalt = dto.PasswordSalt;

                        UnitOfWork.MarkAsNew(supplier);
                        UnitOfWork.Commit();
                    }
                    catch (Exception ex)
                    {
                        Membership.DeleteUser(user.UserName);
                        throw ex;
                    }

                    var roleProvider = Roles.Provider as EudoxusOsyRoleProvider;
                    roleProvider.AddUsersToRoles(new[] { user.UserName }, new[] { RoleNames.Supplier });
                }
                else
                {
                    throw new Exception(string.Format("Το UserName ({0}) χρησιμοποιείται", dto.Username));
                }
            }
            catch (MembershipCreateUserException ex)
            {
                if (ex.StatusCode == MembershipCreateStatus.DuplicateUserName)
                {
                    throw new Exception(string.Format("Το UserName ({0}) χρησιμοποιείται", dto.Username));
                }
            }
        }

        private void UpdateSupplier(Supplier supplier, SyncPublisherDto dto)
        {
            supplier.SupplierKpsID = dto.PublisherKpsID;
            supplier.SupplierTypeInt = dto.PublisherType;
            supplier.Name = dto.PublisherName;
            supplier.TradeName = dto.PublisherTradeName;
            supplier.AFM = dto.PublisherAFM;
            supplier.DOY = dto.PublisherDOY;
            supplier.Email = dto.Email;
            supplier.VerificationStatusInt = dto.VerificationStatus;
            supplier.IsActivated = dto.IsActivated;

            var supplierDetail = supplier.SupplierDetail;

            supplierDetail.PublisherPhone = dto.PublisherPhone;
            supplierDetail.PublisherMobilePhone = dto.PublisherMobilePhone;
            supplierDetail.PublisherFax = dto.PublisherFax;
            supplierDetail.PublisherEmail = dto.PublisherEmail;
            supplierDetail.PublisherUrl = dto.PublisherUrl;
            supplierDetail.PublisherAddress = dto.PublisherAddress;
            supplierDetail.PublisherZipCode = dto.PublisherZipCode;
            supplierDetail.PublisherCityID = dto.PublisherCityID;
            supplierDetail.PublisherPrefectureID = dto.PublisherPrefectureID;
            supplierDetail.LegalPersonName = dto.LegalPersonName;
            supplierDetail.LegalPersonPhone = dto.LegalPersonPhone;
            supplierDetail.LegalPersonEmail = dto.LegalPersonEmail;
            supplierDetail.LegalPersonIdentificationTypeInt = dto.LegalPersonIdentificationType;
            supplierDetail.LegalPersonIdentificationNumber = dto.LegalPersonIdentificationNumber;
            supplierDetail.LegalPersonIdentificationIssuer = dto.LegalPersonIdentificationIssuer;
            supplierDetail.LegalPersonIdentificationIssueDate = dto.LegalPersonIdentificationIssueDate;
            supplierDetail.IsSelfRepresented = dto.IsSelfRepresented;
            supplierDetail.SelfPublisherIdentificationTypeInt = dto.SelfPublisherIdentificationType;
            supplierDetail.SelfPublisherIdentificationNumber = dto.SelfPublisherIdentificationNumber;
            supplierDetail.SelfPublisherIdentificationIssuer = dto.SelfPublisherIdentificationIssuer;
            supplierDetail.SelfPublisherIdentificationIssueDate = dto.SelfPublisherIdentificationIssueDate;
            supplierDetail.ContactIdentificationTypeInt = dto.ContactIdentificationType;
            supplierDetail.ContactIdentificationNumber = dto.ContactIdentificationNumber;
            supplierDetail.ContactIdentificationIssuer = dto.ContactIdentificationIssuer;
            supplierDetail.ContactIdentificationIssueDate = dto.ContactIdentificationIssueDate;
            supplierDetail.AlternateContactName = dto.AlternateContactName;
            supplierDetail.AlternateContactPhone = dto.AlternateContactPhone;
            supplierDetail.AlternateContactMobilePhone = dto.AlternateContactMobilePhone;
            supplierDetail.AlternateContactEmail = dto.AlternateContactEmail;

            var reporter = supplier.Reporter;

            reporter.ContactName = dto.ContactName;
            reporter.ContactPhone = dto.ContactPhone;
            reporter.ContactMobilePhone = dto.ContactMobilePhone;
            reporter.ContactEmail = dto.ContactEmail;
            reporter.Email = dto.Email;
            reporter.VerificationStatusInt = dto.VerificationStatus;
            reporter.IsActivated = dto.IsActivated;

            MembershipUser user = Membership.GetUser(dto.Username);
            user.Email = dto.Email;

            var aspnet_user = new AspnetUsersRepository(UnitOfWork).FindByUsername(user.UserName);
            var aspnet_membership = new AspnetMembershipRepository(UnitOfWork).FindByUserID(aspnet_user.UserId);

            aspnet_membership.Password = dto.Password;
            aspnet_membership.PasswordSalt = dto.PasswordSalt;

            UnitOfWork.Commit();
        }

        public Reporter SyncMinistryPaymentsUser(SyncMinistryPaymentsUserDto dto, out bool? userCreated)
        {
            var ministryPaymentsUser = new ReporterRepository(UnitOfWork).Load(dto.ID);

            if (ministryPaymentsUser == null)
            {
                CreateMinistryPaymentsUser(dto);
                userCreated = true;
            }
            else
            {
                UpdateMinistryPaymentsUser(ministryPaymentsUser, dto);
                userCreated = false;
            }

            return ministryPaymentsUser;
        }

        private void CreateMinistryPaymentsUser(SyncMinistryPaymentsUserDto dto)
        {
            var ministryPaymentsUser = new Reporter()
            {
                ID = dto.ID,
                ReporterType = enReporterType.Ministry,
                ContactName = dto.ContactName,
                ContactPhone = dto.ContactPhone,
                ContactEmail = dto.ContactEmail,
                AuthorizationTypeInt = dto.MinistryAuthorization,
                Description = dto.Description,
                Username = dto.Username,
                Email = dto.Email,
                VerificationStatusInt = dto.VerificationStatus,
                IsActivated = dto.IsActivated,
                CreatedAt = DateTime.Now,
                CreatedBy = "sysadmin"
            };

            try
            {
                MembershipUser user;
                MembershipCreateStatus status;
                user = Membership.CreateUser(dto.Username, dto.Password, dto.Email, null, null, true, out status);

                if (user != null)
                {
                    try
                    {
                        var aspnet_user = new AspnetUsersRepository(UnitOfWork).FindByUsername(user.UserName);
                        var aspnet_membership = new AspnetMembershipRepository(UnitOfWork).FindByUserID(aspnet_user.UserId);

                        aspnet_membership.Password = dto.Password;
                        aspnet_membership.PasswordSalt = dto.PasswordSalt;

                        UnitOfWork.MarkAsNew(ministryPaymentsUser);
                        UnitOfWork.Commit();
                    }
                    catch (Exception ex)
                    {
                        Membership.DeleteUser(user.UserName);
                        throw ex;
                    }

                    var roleProvider = Roles.Provider as EudoxusOsyRoleProvider;
                    roleProvider.AddUsersToRoles(new[] { user.UserName }, new[] { RoleNames.MinistryPayments });
                }
                else
                {
                    throw new Exception(string.Format("Το UserName ({0}) χρησιμοποιείται", dto.Username));
                }
            }
            catch (MembershipCreateUserException ex)
            {
                if (ex.StatusCode == MembershipCreateStatus.DuplicateUserName)
                {
                    throw new Exception(string.Format("Το UserName ({0}) χρησιμοποιείται", dto.Username));
                }
            }
        }

        private void UpdateMinistryPaymentsUser(Reporter reporter, SyncMinistryPaymentsUserDto dto)
        {
            reporter.ContactName = dto.ContactName;
            reporter.ContactPhone = dto.ContactPhone;
            reporter.ContactEmail = dto.ContactEmail;
            reporter.AuthorizationTypeInt = dto.MinistryAuthorization;
            reporter.Description = dto.Description;
            reporter.Email = dto.Email;
            reporter.VerificationStatusInt = dto.VerificationStatus;
            reporter.IsActivated = dto.IsActivated;

            MembershipUser user = Membership.GetUser(dto.Username);
            user.Email = dto.Email;

            var aspnet_user = new AspnetUsersRepository(UnitOfWork).FindByUsername(user.UserName);
            var aspnet_membership = new AspnetMembershipRepository(UnitOfWork).FindByUserID(aspnet_user.UserId);

            aspnet_membership.Password = dto.Password;
            aspnet_membership.PasswordSalt = dto.PasswordSalt;

            UnitOfWork.Commit();
        }

        #endregion
    }
}
