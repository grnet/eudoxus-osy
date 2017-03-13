using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;

namespace EudoxusOsy.Portal.Secure.EditorPopups
{
    public partial class FileUpload : System.Web.UI.Page
    {
        public string JsCallback { get { return Request.QueryString["cb"]; } }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            var fileUplodSize = BusinessHelper.GetFileUploadSizeForUser();

            if (Request.IsLocal)
                dxUploadControl.AdvancedModeSettings.TemporaryFolder = "~/Uploads";

            dxUploadControl.ShowProgressPanel = true;
            dxUploadControl.ValidationSettings.MaxFileSize = fileUplodSize;
            dxUploadControl.ValidationSettings.MaxFileSizeErrorText = string.Format(dxUploadControl.ValidationSettings.MaxFileSizeErrorText, BusinessHelper.ToHumanReadableFileSize(fileUplodSize));
            dxUploadControl.ValidationSettings.AllowedFileExtensions = Config.FileUpload.AllowedFileExtensions.Split(',').Select(x => x.Trim()).ToArray();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClientScript.RegisterStartupScript(GetType(), "callback", string.Format("window.parent.{0}([{1},{2}]);", JsCallback, dxUploadControl.ClientID, dxBtnUpload.ClientID), true);
            }
        }

        protected void dxUploadControl_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            var id = "*hereismyid*";
            int newid = 0;
            var filename = string.Format("{0}-{1:N}{2}", User.Identity.Name, id, Path.GetExtension(e.UploadedFile.FileName));

            if (e.UploadedFile.ContentLength > BusinessHelper.GetFileUploadSizeForUser())
                return;

            using (var uow = UnitOfWorkFactory.Create())
            {
                var uploadedFile = new BusinessModel.File()
                {
                    FileName = e.UploadedFile.FileName, //OriginalFilename
                    //FileSize = e.UploadedFile.FileBytes.Length,
                    PathName = filename,
                    CreatedAt = DateTime.Now,
                    CreatedBy = Page.User.Identity.Name,
                    IsActive = true
                };

                uow.MarkAsNew(uploadedFile);
                uow.Commit();

                newid = uploadedFile.ID;
                uploadedFile.PathName = filename = filename.Replace(id, newid.ToString());
                uow.Commit();
            }

            var filePath = Path.Combine(Config.FileUpload.UploadPath, filename);
            e.UploadedFile.SaveAs(filePath);

            var callbackData = new
            {
                FileID = newid,
                FileName = e.UploadedFile.FileName,
                Title = BusinessHelper.ToHumanReadableFileSize(e.UploadedFile.ContentLength)
            };

            e.CallbackData = new JavaScriptSerializer().Serialize(callbackData);
            e.IsValid = true;
        }
    }
}