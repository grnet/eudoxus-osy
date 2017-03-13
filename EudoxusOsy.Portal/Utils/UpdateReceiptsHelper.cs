using EudoxusOsy.BusinessModel;
using EudoxusOsy.Utils;
using Imis.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EudoxusOsy.Portal
{
    public class UpdateReceiptsHelper
    {
        public static void UpdateReceiptsFromAuditReceipts()
        {
            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                var returnValue = new System.Data.Objects.ObjectParameter("ret", (int)enUpdateReceiptResult.ProcedureFailed);

                #region [ TEST SP Data]

                /**
                    Just a test with sample values
                */
                //int? registrationKpsID = 0;
                //DateTime? receivedAt = DateTime.Now;
                //int? bookKpsID = 0;
                //int? secretariatKpsId = 0;
                //DateTime? sentByKpsAt = DateTime.Now;
                //string reason = enReceiptState.Delivered.GetLabel();

                /**
                    Just Testing
                */
                //registrationKpsID = 1;
                //receivedAt =  DateTime.Now;
                //bookKpsID = 6934;
                //secretariatKpsId = 1648;
                //sentByKpsAt = DateTime.Now;
                //reason = enReceiptState.Delivered.GetLabel("service");

                #endregion

                var results = new UpdateReceiptsFromAuditReceiptsResults();
                var auditRepository = new AuditReceiptRepository(uow);

                var startIndex = 0;
                int totalAuditRecords = 0;
                while (startIndex < totalAuditRecords + 1000)
                {
                    var criteria = new Criteria<AuditReceipt>();
                    criteria.MaximumRows = 1000;
                    criteria.StartRowIndex = startIndex;
                    var auditReceipts = auditRepository.FindAuditsToUpdate(criteria, out totalAuditRecords);

                    foreach (var auditReceipt in auditReceipts)
                    {
                        var isForLibrary = false;
                        var librariesIDs = CacheManager.GetLibraries().Select(x => x.LibraryKpsID).ToList();
                        if (librariesIDs.Contains((int)auditReceipt.SecreteriatKpsID))
                        {
                            isForLibrary = true;
                        }
                        ((DBEntities)uow).spReceipt(auditReceipt.RegistrationKpsID, auditReceipt.ReceivedAt, auditReceipt.KpsBookID, auditReceipt.SecreteriatKpsID, auditReceipt.SentByKpsAt, (int?)auditReceipt.Reason, auditReceipt.ID, returnValue);

                        if ((int)returnValue.Value == (int)enUpdateReceiptResult.Success)
                        {
                            if (isForLibrary)
                            {
                                results.LibrarySucceededBookCount += auditReceipt.Amount;
                                results.LibrarySucceededCount++;
                            }
                            else
                            {
                                results.StudentSucceededCount++;
                                results.StudentsSucceededBookCount++;
                            }
                        }
                        else
                        {
                            if (isForLibrary)
                            {
                                results.LibraryFailedBookCount += auditReceipt.Amount;
                                results.LibraryFailedCount++;
                            }
                            else
                            {
                                results.StudentFailedCount++;
                                results.StudentsFailedBookCount++;
                            }
                        }
                        results.TotalProcessedCount++;
                    }

                    startIndex += 1000;
                }

                LogHelper.LogMessage<UpdateReceiptsHelper>(GenerateResultsReportEN(results));

                var reportEmail = EmailFactory.GetDailyUpdateReceiptsFromAuditReceiptsReport(GenerateResultsReportEL(results));
                uow.MarkAsNew(reportEmail);
                uow.Commit();
                EmailQueueWorker.Current.AddEmailDispatchToQueue(reportEmail.ID);
            }
        }

        public static void CompareXmlReceipts()
        {
            var appData = EudoxusOsyCacheManager<ApplicationData>.Current.GetItems().FirstOrDefault(x => x.Name == ApplicationDataNames.ShouldRunXmlComparisonAndSendReports);
            if (appData != null && appData.Value == enYesNo.Yes.GetValue().ToString())
            {
                using (IUnitOfWork uow = UnitOfWorkFactory.Create())
                {
                    // get the latest and active 
                    var currentPhase = new PhaseRepository(uow).GetCurrentPhase();

                    if (currentPhase != null)
                    {
                        var kpsOnly = new AuditReceiptRepository(uow).GetKpsXmlOnly(currentPhase.ID);
                        var receiptsOnly = new AuditReceiptRepository(uow).GetOsyReceiptsOnly(currentPhase.ID);

                        string kpsFileName = Path.Combine(Config.XmlFilesPath, string.Format("kpsOnly_{0}.csv", DateTime.Now.Ticks));
                        string osyFileName = Path.Combine(Config.XmlFilesPath, string.Format("receiptsOnly_{0}.csv", DateTime.Now.Ticks));

                        System.IO.File.WriteAllLines(kpsFileName, kpsOnly.Select(x => x.RegistrationKpsID + "," + x.BookKpsID + "," + x.SecretaryOrLibraryKpsID + "," + x.BookCount).ToArray());
                        System.IO.File.WriteAllLines(osyFileName, receiptsOnly.Select(x => x.RegistrationKpsID + "," + x.BookKpsID + "," + x.SecretaryOrLibraryKpsID + "," + x.BookCount).ToArray());

                        List<string> attachments = new List<string>();
                        attachments.Add(kpsFileName);
                        attachments.Add(osyFileName);

                        var emailDispatcher = new EmailDispatcher();

                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("Αναφορά αποτελεσμάτων: Σύγκριση παραδόσεων ΚΠΣ και ΟΣΥ ({0}) \r\n", DateTime.Now.ToShortDateString());
                        sb.Append("----------------------------------------------------------\r\n");
                        sb.AppendFormat("Αριθμός παραδόσεων που βρέθηκαν αποκλειστικά στο ΚΠΣ: {0}\r\n", kpsOnly.Count);
                        sb.AppendLine();
                        sb.AppendFormat("Αριθμός παραδόσεων που βρέθηκαν αποκλειστικά στο ΟΣΥ: {0}\r\n", receiptsOnly.Count);
                        sb.AppendLine();
                        sb.AppendLine();
                        sb.Append("Συνημμένα:");
                        sb.AppendLine();
                        sb.AppendFormat(kpsOnly.Count > 0 ? string.Format("\t  {0} , αρχείο με αποκλειστικές παραδόσεις ΚΠΣ \r\n", kpsFileName) : "Δεν υπάρχουν αποκλειστικές παραδόσεις του ΚΠΣ");
                        sb.AppendLine();
                        sb.AppendFormat(receiptsOnly.Count > 0 ? string.Format("\t  {0} , αρχείο με αποκλειστικές παραδόσεις ΚΠΣ \r\n", osyFileName) : "Δεν υπάρχουν αποκλειστικές παραδόσεις του ΟΣΥ");



                        emailDispatcher.SendWithAttachments("noreply@eudoxusosy.pilotiko.gr", //from
                            Config.DailyUpdateReceiptsFromAuditReceiptsEmailTo,  //to
                            null,//ccs
                            "EuryWhere - Αποτελέσματα Σύγκρισης KPS-OSY receipt entries",//subject
                            string.Format(sb.ToString()),//body
                            false, attachments);//attachment

                        /**
                            Update the shouldRunXmlComparison value in order to NOT run the comparison until a new xml gets uploaded
                        */
                        BusinessHelper.UpdateApplicationDataEntry(ApplicationDataNames.ShouldRunXmlComparisonAndSendReports, enYesNo.Yes.GetValue().ToString());
                        EudoxusOsyCacheManager<ApplicationData>.Current.Refresh();
                        LogHelper.LogMessage<UpdateReceiptsHelper>("Στάλθηκε η αναφορά σύγκρισης παραδόσεων ΚΠΣ - ΟΣΥ του εξαμήνου");
                    }
                }
            }
        }

        public static void InsertCorrectedKPSOnlyFile(string fileName)
        {
            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                var auditRepo = new AuditReceiptRepository(uow);
                auditRepo.InsertCorrectedFiles(Config.XmlFilesPath, fileName, 1);
            }
            BusinessHelper.UpdateApplicationDataEntry(ApplicationDataNames.ShouldRunComplementReceipts, enYesNo.Yes.GetValue().ToString());
        }

        public static void InsertCorrectedOSYOnlyFile(string fileName)
        {
            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                var auditRepo = new AuditReceiptRepository(uow);
                auditRepo.InsertCorrectedFiles(Config.XmlFilesPath, fileName, 2);
            }
            BusinessHelper.UpdateApplicationDataEntry(ApplicationDataNames.ShouldRunComplementReceipts, enYesNo.Yes.GetValue().ToString());
        }

        public static void ComplementReceipts()
        {
            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                var currentPhase = new PhaseRepository(uow).GetCurrentPhase();
                new AuditReceiptRepository(uow).ComplementReceipts(currentPhase.ID);

                BusinessHelper.UpdateApplicationDataEntry(ApplicationDataNames.ShouldRunComplementReceipts, enYesNo.No.GetValue().ToString());
            }
        }


        public static string GenerateResultsReportEL(UpdateReceiptsFromAuditReceiptsResults results)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Αναφορά αποτελεσμάτων: Επεξεργασία παραδόσεων ({0}) \r\n", DateTime.Now.ToShortDateString());
            sb.Append("----------------------------------------------------------\r\n");
            sb.AppendFormat("Σύνολο παραδόσεων: {0}\r\n", results.TotalProcessedCount);
            sb.Append("Παραδόσεις σε φοιτητές:\r\n");
            sb.AppendFormat("\t Επιτυχής καταχώριση: {0} (αντίτυπα: {1}) \r\n", results.StudentSucceededCount, results.StudentsSucceededBookCount);
            sb.AppendFormat("\t Αδυναμία καταχώρισης: {0} (αντίτυπα: {1}) \r\n", results.StudentFailedCount, results.StudentsFailedBookCount);
            sb.AppendLine();
            sb.Append("Παραδόσεις σε βιβλιοθήκες:\r\n");
            sb.AppendFormat("\t Επιτυχής καταχώριση: {0} (αντίτυπα: {1}) \r\n", results.LibrarySucceededCount, results.LibrarySucceededBookCount);
            sb.AppendFormat("\t Αδυναμία καταχώρισης: {0} (αντίτυπα: {1}) \r\n", results.LibraryFailedCount, results.LibraryFailedBookCount);

            return sb.ToString();
        }

        public static string GenerateResultsReportEN(UpdateReceiptsFromAuditReceiptsResults results)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Results Report: Processing deliveries from {0} \r\n", DateTime.Now.ToShortDateString());
            sb.Append("----------------------------------------------------------\r\n");
            sb.AppendFormat("Total deliveries processed: {0}\r\n", results.TotalProcessedCount);
            sb.Append("Deliveries to students:\r\n");
            sb.AppendFormat("\t Successfully inserted: {0} (book count: {1}) \r\n", results.StudentSucceededCount, results.StudentsSucceededBookCount);
            sb.AppendFormat("\t Failed to process: {0} (book count: {1}) \r\n", results.StudentFailedCount, results.StudentsFailedBookCount);
            sb.AppendLine();
            sb.Append("Deliveries to libraries:\r\n");
            sb.AppendFormat("\t Successfully inserted: {0} (book count: {1}) \r\n", results.LibrarySucceededCount, results.LibrarySucceededBookCount);
            sb.AppendFormat("\tFailed to process: {0} (book count: {1}) \r\n", results.LibraryFailedCount, results.LibraryFailedBookCount);

            return sb.ToString();
        }
    }

    public class UpdateReceiptsFromAuditReceiptsResults
    {
        public int TotalProcessedCount { get; set; }
        public int StudentSucceededCount { get; set; }
        public int StudentsSucceededBookCount { get; set; }
        public int StudentFailedCount { get; set; }
        public int StudentsFailedBookCount { get; set; }
        public int LibrarySucceededCount { get; set; }
        public int LibrarySucceededBookCount { get; set; }
        public int LibraryFailedCount { get; set; }
        public int LibraryFailedBookCount { get; set; }
    }
}