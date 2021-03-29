using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;

namespace EudoxusOsy.Portal.Secure
{
    /// <summary>
    /// Summary description for IBANCertificate
    /// </summary>
    public class IBANCertificate : BaseHttpHandler
    {
        protected int SupplierID
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["id"]);
            }
        }

        protected override void DoProcessRequest()
        {
            var supplier = new SupplierRepository().Load(SupplierID, x => x.SupplierIBANs);

            if (supplier.CurrentIBAN == null || supplier.CurrentIBAN.IBANCertificateID == null)
            {
                return;
            }

            var file = new FileRepository().Load(supplier.CurrentIBAN.IBANCertificateID.Value);

            if (file != null)
            {                
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", file.FileName));
                Response.TransmitFile(Config.FileUpload.UploadPath + "/"+ file.PathName);                
            }                        
        }        
    }
}