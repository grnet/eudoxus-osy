using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;

namespace EudoxusOsy.Portal.Controls.ScriptControls
{
    public partial class FileUpload : BaseScriptControl, IScriptControl
    {
        #region [ Properties ]

        public int? FileID { get; set; }

        public bool IsRequired { get; set; }

        public string FieldName { get; set; }

        public string ErrorMessageFormat { get; set; }

        public string ClientValidationFunction { get; set; }

        private string _valGroup = null;
        public string ValidationGroup
        {
            get { return _valGroup; }
            set { cvFileUpload.ValidationGroup = _valGroup = value; }
        }

        protected override string ClientControlName
        {
            get { return "EudoxusOsy.Portal.Controls.ScriptControls.FileUpload"; }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            var id = 0;
            if (int.TryParse(Request.Form[hfFileID.UniqueID], out id))
            {
                FileID = id;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!string.IsNullOrEmpty(FieldName))
            {
                if (!string.IsNullOrEmpty(ErrorMessageFormat))
                    cvFileUpload.ErrorMessage = string.Format(ErrorMessageFormat, FieldName);
                else
                    cvFileUpload.ErrorMessage = string.Format("Το πεδίο '{0}' είναι υποχρεωτικό", FieldName);
            }

            base.OnPreRender(e);
        }

        #region [ Bind And Extract Value ]

        public void BindFile(BusinessModel.File file)
        {
            if (file == null)
                return;

            FileID = file.ID;
            lnkDownload.InnerText = file.FileName;
            //lnkDownload.Title = BusinessHelper.ToHumanReadableFileSize(file.FileSize);
            hfFileID.Value = file.ID.ToString();
        }

        public void BindFile(int fileID)
        {
            var file = new FileRepository().Load(fileID);

            if (file == null)
                return;

            FileID = file.ID;
            lnkDownload.InnerText = file.FileName;
            //lnkDownload.Title = BusinessHelper.ToHumanReadableFileSize(file.FileSize);
            hfFileID.Value = file.ID.ToString();
        }

        public object ExtractValue()
        {
            int fileID;
            if (int.TryParse(hfFileID.Value, out fileID))
                return fileID;
            else
                return null;
        }

        #endregion

        #region [ IScriptControl Members ]

        public override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            var scd = new ScriptControlDescriptor(ClientControlName, ClientID);
            scd.AddProperty("initParams", new
            {
                DownloadUrl = ResolveClientUrl(Config.FileUpload.DownloadUrl),
                IsRequired = IsRequired
            });
            scd.AddProperty("fieldDescription", FieldName);
            scd.AddProperty("clientValidationFunction", ClientValidationFunction);
            scd.AddElementProperty("cvFileUpload", cvFileUpload.ClientID);
            scd.AddElementProperty("ifrDownload", ifrDownload.ClientID);
            scd.AddElementProperty("lnkDownload", lnkDownload.ClientID);
            scd.AddElementProperty("uploadRow", uploadRow.ClientID);
            scd.AddElementProperty("downloadRow", downloadRow.ClientID);
            scd.AddElementProperty("hfFileID", hfFileID.ClientID);
            scd.AddScriptProperty("popupFileUpload", string.Format("window['{0}']", popupFileUpload.ClientID));
            scd.AddScriptProperty("btnShowPopup", string.Format("window['{0}']", dxBtnUpload.ClientID));
            scd.AddScriptProperty("btnDelete", string.Format("window['{0}']", dxBtnDelete.ClientID));

            yield return scd;
        }

        public override IEnumerable<ScriptReference> GetScriptReferences()
        {
            yield return new ScriptReference("~/Controls/ScriptControls/FileUpload.js");
        }

        #endregion
    }
}