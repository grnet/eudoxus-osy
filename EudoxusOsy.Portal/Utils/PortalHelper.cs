using DevExpress.Web;
using Imis.Domain;
using EudoxusOsy.BusinessModel;
using System;
using System.Web;
using System.Linq;
using EudoxusOsy.Portal.DataSources;
using System.Collections.Generic;
using System.Xml.Linq;

namespace EudoxusOsy.Portal
{
    public static class RequestHelper
    {
        public static string GetClientIP(this HttpRequest request)
        {
            var ip = request.Headers["X-Forwarded-For"];

            return string.IsNullOrEmpty(ip)
                    ? request.UserHostAddress
                    : ip;
        }

        public static string GetScheme(this HttpRequest request)
        {
            var scheme = request.Headers["X-Proto"];

            return !string.IsNullOrEmpty(scheme) && scheme == "SSL"
                    ? "https"
                    : request.Url.Scheme;
        }
    }

    public static class VerificationHelper
    {
        //public static void SendVerificationInfo(Reporter reporter, IUnitOfWork UnitOfWork)
        //{
        //    SendEmailVerification(reporter, UnitOfWork);
        //    SendMobilePhoneVerificationInfo(reporter, UnitOfWork);

        //    UnitOfWork.Commit();
        //}



        #region [ Helper Methods ]

        //public static void SendEmailVerification(Reporter reporter, IUnitOfWork UnitOfWork)
        //{
        //    Uri baseURI;
        //    if (Config.IsSSL)
        //    {
        //        baseURI = new Uri("https://" + HttpContext.Current.Request.Url.Authority + "/Common/");
        //    }
        //    else
        //    {
        //        baseURI = new Uri("http://" + HttpContext.Current.Request.Url.Authority + "/Common/");
        //    }

        //    Uri uri = new Uri(baseURI, "VerifyEmail.aspx?id=" + reporter.EmailVerificationCode);

        //    Email email = new Email();

        //    //if (reporter.ReporterType == enReporterType.Provider)
        //    //{
        //    //    email = EmailFactory.GetProviderEmailVerification(reporter, uri);
        //    //}
        //    //else if (reporter.ReporterType == enReporterType.Beneficiary)
        //    //{
        //    //    email = EmailFactory.GetBeneficiaryEmailVerification(reporter, uri);
        //    //}

        //    UnitOfWork.MarkAsNew(email);
        //    UnitOfWork.Commit();

        //    EmailQueueWorker.Current.AddEmailDispatchToQueue(email.ID);
        //}

        public static void SendMobilePhoneVerificationInfo(Reporter reporter, IUnitOfWork UnitOfWork)
        {
            //var sms = SMSFactory.GetVerificationCode(reporter);
            //UnitOfWork.MarkAsNew(sms);
            //reporter.SMSSentCount++;

            //UnitOfWork.Commit();

            //SMSQueueWorker.Current.AddSmsDispatchToQueue(sms.ID);
        }

        #endregion
    }

    public static class DropDownHelper
    {
        public static void FillFromEnum<T>(this ASPxComboBox dropDown, string promptValue = "-- αδιάφορο --", bool showPromptValue = true, bool includeZeroValue = false)
           where T : struct
        {
            dropDown.Items.Clear();

            if (showPromptValue)
            {
                dropDown.Items.Add(new ListEditItem(promptValue, null));
            }

            foreach (Enum item in Enum.GetValues(typeof(T)))
            {
                if (item.GetValue() == 0 && !includeZeroValue)
                    continue;

                dropDown.Items.Add(new ListEditItem(item.GetLabel(), item.GetValue()));
            }
        }

        public static void FillFromEnum<T>(this ASPxCheckBoxList checkBoxList, bool includeZeroValue = false)
          where T : struct
        {
            checkBoxList.Items.Clear();

            foreach (Enum item in Enum.GetValues(typeof(T)))
            {
                if (item.GetValue() == 0 && !includeZeroValue)
                    continue;

                checkBoxList.Items.Add(new ListEditItem(item.GetLabel(), item.GetValue()));
            }
        }

        public static void FillFromEnum<T>(this ASPxRadioButtonList radioButtonList, bool includeZeroValue = false)
           where T : struct
        {
            radioButtonList.Items.Clear();

            foreach (Enum item in Enum.GetValues(typeof(T)))
            {
                if (item.GetValue() == 0 && !includeZeroValue)
                    continue;

                radioButtonList.Items.Add(new ListEditItem(item.GetLabel(), item.GetValue()));
            }
        }

        public static void FillTrueFalse(this ASPxComboBox dropDown, string promptValue = "-- αδιάφορο --")
        {
            dropDown.Items.Add(new ListEditItem(promptValue, null));

            dropDown.Items.Add(new ListEditItem("ΝΑΙ", 1));
            dropDown.Items.Add(new ListEditItem("ΟΧΙ", 0));
        }

        public static void FillReporterTypes(this ASPxComboBox dropDown, string promptValue = "-- αδιάφορο --")
        {
            dropDown.Items.Add(new ListEditItem(promptValue, null));

            dropDown.Items.Add(new ListEditItem(enReporterType.Supplier.GetLabel(), enReporterType.Supplier.GetValue()));
            dropDown.Items.Add(new ListEditItem(enReporterType.Ministry.GetLabel(), enReporterType.Ministry.GetValue()));
        }

        public static void FillPrefectures(this ASPxComboBox ddlPrefecture, string promptValue = "-- επιλέξτε περιφερειακή ενότητα --")
        {
            ddlPrefecture.Items.Add(new ListEditItem(promptValue, null));

            foreach (var item in CacheManager.GetOrderedPrefectures())
            {
                ddlPrefecture.Items.Add(new ListEditItem(item.Name, item.ID));
            }
        }

        public static void FillCities(this ASPxComboBox ddlCity, string prefID, string promptValue = "-- επιλέξτε καλλικρατικό δήμο --")
        {
            int prefectureID;
            if (int.TryParse(prefID, out prefectureID) && prefectureID > 0)
            {
                var cities = CacheManager.GetOrderedCities(prefectureID);

                ddlCity.Items.Clear();
                if (cities.Count() == 1)
                {
                    ddlCity.Items.Add(new ListEditItem(cities.FirstOrDefault().Name, cities.FirstOrDefault().ID));
                }
                else
                {
                    ddlCity.Items.Add(new ListEditItem(promptValue, null));

                    foreach (City item in cities)
                    {
                        ddlCity.Items.Add(new ListEditItem(item.Name, item.ID));
                    }
                }
            }
        }

        public static void FillInstitutions(this ASPxComboBox ddlInstitution, string promptValue = "-- επιλέξτε ίδρυμα --")
        {
            ddlInstitution.Items.Clear();
            ddlInstitution.Items.Add(new ListEditItem(promptValue, null));

            foreach (var item in CacheManager.GetOrderedInstitutions())
            {
                ddlInstitution.Items.Add(new ListEditItem(item.Name, item.ID));
            }
        }

        public static void FillDepartments(this ASPxComboBox ddlDepartment, string instID, string promptValue = "-- επιλέξτε τμήμα --")
        {
            int institutionID;
            if (int.TryParse(instID, out institutionID) && institutionID > 0)
            {
                var departments = CacheManager.GetOrderedDepartments(institutionID);

                ddlDepartment.Items.Clear();
                if (departments.Count() == 1)
                {
                    ddlDepartment.Items.Add(new ListEditItem(departments.FirstOrDefault().Name, departments.FirstOrDefault().ID));
                }
                else
                {
                    ddlDepartment.Items.Add(new ListEditItem(promptValue, null));

                    foreach (Department item in departments)
                    {
                        ddlDepartment.Items.Add(new ListEditItem(item.Name, item.ID));
                    }
                }
            }
        }

        public static void FillPfos(this ASPxComboBox ddlPfo, string promptValue = "-- επιλέξτε Δ.Ο.Υ. Πληρωμών --")
        {
            ddlPfo.Items.Add(new ListEditItem(promptValue, null));

            foreach (var item in CacheManager.GetOrderedPublicFinancialOffices())
            {
                ddlPfo.Items.Add(new ListEditItem(item.Name, item.ID));
            }
        }

        public static void FillBanks(this ASPxComboBox ddlBank, bool isBank, string promptValue = "-- επιλέξτε τράπεζα --")
        {
            ddlBank.Items.Add(new ListEditItem(promptValue, null));

            var banks = new BankRepository().LoadAll().OrderBy(x=> x.Name);

            foreach (var item in banks.Where(x => x.IsBank == isBank && x.IsActive))
            {
                ddlBank.Items.Add(new ListEditItem(item.Name, item.ID));
            }
        }

        public static void FillPhases(this ASPxComboBox ddlPhase, bool isPhase, string promptValue = "-- επιλέξτε περίοδο πληρωμών --")
        {
            ddlPhase.Items.Add(new ListEditItem(promptValue, null));

            var phases = new PhaseRepository().LoadAll().OrderByDescending(x => x.ID);

            foreach (var item in phases.Where(x => x.IsActive))
            {
                ddlPhase.Items.Add(new ListEditItem(item.AcademicYearAndSemester, item.ID));
            }
        }

        public static void FillPhasesAllowedForManualCatalogCreation(this ASPxComboBox ddlPhase, bool isPhase, string promptValue = "-- επιλέξτε περίοδο πληρωμών --")
        {
            ddlPhase.Items.Add(new ListEditItem(promptValue, null));

            var phases = new PhaseRepository().LoadAll().Where(x => x.CatalogsCreated).OrderByDescending(x => x.ID);

            foreach (var item in phases.Where(x => x.IsActive))
            {
                ddlPhase.Items.Add(new ListEditItem(item.AcademicYearAndSemester, item.ID));
            }
        }

        public static void FillYears(this ASPxComboBox ddlPhase, bool isPhase, string promptValue = "-- επιλέξτε περίοδο πληρωμών --")
        {
            ddlPhase.Items.Add(new ListEditItem(promptValue, null));

            var phases = new PhaseRepository().LoadAll().OrderByDescending(x => x.ID);

            foreach (var item in phases.Where(x => x.IsActive))
            {
                if (ddlPhase.Items.IndexOfText(item.AcademicYearString) == -1)
                {
                    ddlPhase.Items.Add(new ListEditItem(item.AcademicYearString, item.AcademicYear));
                }
            }
        }

        public static void FillPhasesIDs(this ASPxComboBox ddlPhase, bool isPhase, string promptValue = "-- επιλέξτε περίοδο πληρωμών --")
        {
            ddlPhase.Items.Add(new ListEditItem(promptValue, null));

            var phases = new PhaseRepository().LoadAll().OrderByDescending(x => x.ID);

            foreach (var item in phases.Where(x => x.IsActive))
            {
                ddlPhase.Items.Add(new ListEditItem(item.ID.ToString(), item.ID));
            }
        }
    }
}