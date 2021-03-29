using System;
using EudoxusOsy.Portal.Controls;
using EudoxusOsy.BusinessModel;

namespace EudoxusOsy.Portal.Secure
{
    public class GenerateCatalogPDFFromCatalogLog : BaseHttpHandler
    {
        protected int CatalogGroupLogID
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["id"]);
            }
        }

        protected CatalogGroupLog CurrentCatalogGroupLog;
        protected CatalogGroupChangeValues ChangeValues;        

        protected override void DoProcessRequest()
        {
            CurrentCatalogGroupLog = new CatalogGroupLogRepository(UnitOfWork).Load(CatalogGroupLogID);
            ChangeValues = CurrentCatalogGroupLog.GetNewValues();            

            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename=CatalogGroupPDF-{0}.pdf", CurrentCatalogGroupLog.GroupID.ToString()));            

            if (CurrentCatalogGroupLog.PdfTypeInt == (int) enPdfType.CatalogGroupLogByDepartment)
            {
                CurrentCatalogGroupLog.ConvertLogInfoToDto().CreatePDF(Response, "~/_rdlc/CatalogInvoice.rdlc");
            }
            else if (CurrentCatalogGroupLog.PdfTypeInt == (int)enPdfType.CatalogGroupLogByBook)
            {
                CurrentCatalogGroupLog.ConvertLogInfoToDto().CreatePDF(Response, "~/_rdlc/CatalogInvoiceByBook.rdlc");
            }            
        }        
    }    
}