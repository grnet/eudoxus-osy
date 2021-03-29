using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System.IO;
using System.Linq;
using System.Web.SessionState;

namespace EudoxusOsy.Portal.Secure
{
    /// <summary>
    /// Summary description for FileDownload
    /// </summary>
    public class FileDownload : BaseHttpHandler, IRequiresSessionState
    {
        private const string AlertFormat = "<script type='text/javascript'>alert('{0}');</script>";

        public int FileID { get { return int.Parse(Request.QueryString["fid"]); } }

        protected override void DoProcessRequest()
        {
            var file = (BusinessModel.File)null;
            //var entity = (Reporter)null;

            bool isRelatedSelfPublisher = false;
            bool isUploader = false;
            using (UnitOfWork.SingleConnection())
            {
                var fileRepository = new FileRepository(UnitOfWork);

                file = fileRepository.Load(FileID, x => x.FileSelfPublishers);
                if (file.CreatedBy == User.Identity.Name)
                {
                    isUploader = true;
                }

                if (file.FileSelfPublishers != null && file.FileSelfPublishers.Count > 0)
                {
                    var selfPublishersIDs = file.FileSelfPublishers.Select(x => x.SupplierID);
                    var selfPublishers = new SupplierRepository(UnitOfWork).LoadMany(selfPublishersIDs, x => x.Reporter);
                    if (selfPublishers.Select(x => x.Reporter.Username).Contains(User.Identity.Name))
                    {
                        isRelatedSelfPublisher = true;
                    }

                }
                //    entity = fileRepository.GetRelatedEntity(file.ID);
            }
            /**
                Security check, do not let download if not related to the file
            */
            if (file != null && (isRelatedSelfPublisher
                || isUploader
                || User.IsInRole(RoleNames.SuperMinistry)
                || User.IsInRole(RoleNames.MinistryPayments))
                || User.IsInRole(RoleNames.SystemAdministrator)
                || User.IsInRole(RoleNames.MinistryWelfare))
            {
                var filePath = Path.Combine(Config.FileUpload.UploadPath, file.PathName);
                string fileName = file.FileName;
                //if (entity != null)
                //{
                string prefix = string.Empty;
                //if (entity.ReporterType == enReporterType.Consultant)
                //    prefix = "C";
                //else if (entity.ReporterType == enReporterType.Provider)
                //    prefix = "PR";
                //else
                //    prefix = "R";
                fileName = string.Format("{0}", fileName);
                //}

                if (System.IO.File.Exists(filePath))
                {
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                    // Response.AddHeader("Content-Length", file.FileSize.ToString());
                    Response.ContentType = "application/octet-stream";
                    Response.TransmitFile(filePath);
                    Response.Flush();
                }
                else
                {
                    WriteError("Το αρχείο δεν υπάρχει πλέον, επικοινωνήστε με τον διαχειριστή του συστήματος.");
                }
            }
            else
            {
                WriteError("Το αρχείο που ζητήσατε δεν υπάρχει πλέον.");
            }
        }

        private void WriteError(string errorMessage)
        {
            RedirectAndNotify(Request.UrlReferrer.OriginalString, errorMessage);
            //Response.ContentType = "text/html";
            //Response.Write(string.Format(AlertFormat, errorMessage));
        }
    }
}