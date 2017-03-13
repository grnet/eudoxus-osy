using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;
using Imis.Domain;
using EudoxusOsy.Portal.CacheManagerExtensions;
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
        List<BookInCatalogInfo> BooksInCatalog = new List<BookInCatalogInfo>();

        protected override void DoProcessRequest()
        {
            CurrentCatalogGroupLog = new CatalogGroupLogRepository(UnitOfWork).Load(CatalogGroupLogID);
            ChangeValues = CurrentCatalogGroupLog.GetNewValues();
            BooksInCatalog = ChangeValues.IncludedCatalogs;

            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename=CatalogGroupPDF-{0}.pdf", CurrentCatalogGroupLog.GroupID.ToString()));
            CreatePDF();
        }

        private void CreatePDF()
        {
            using (LocalReport lr = new LocalReport())
            {
                ConfigureReport(lr);

                string deviceInfo = @"<DeviceInfo>
                    <OutputFormat>PDF</OutputFormat>
                    <PageWidth>29.7cm</PageWidth>
                    <PageHeight>21cm</PageHeight>
                    <MarginTop>0.5in</MarginTop>
                    <MarginLeft>0.0in</MarginLeft>
                    <MarginRight>0.0in</MarginRight>
                    <MarginBottom>0.5in</MarginBottom>
                    </DeviceInfo>";

                string reportType = "PDF";
                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                Response.BinaryWrite(renderedBytes);
            }
        }

        private void ConfigureReport(LocalReport localReport)
        {
            localReport.ReportPath = HttpContext.Current.Server.MapPath("~/_rdlc/CatalogInvoice.rdlc");

            List<ReportParameter> parameters = new List<ReportParameter>();

            parameters.Add(new ReportParameter("YearFromToLiteral", ChangeValues.AcademicYearString));
            parameters.Add(new ReportParameter("AFM", ChangeValues.SupplierAFM));
            parameters.Add(new ReportParameter("SupplierName", ChangeValues.SupplierName ));
            parameters.Add(new ReportParameter("DOY", ChangeValues.SupplierDOY));
            parameters.Add(new ReportParameter("IBAN", ChangeValues.SupplierIBAN));
            parameters.Add(new ReportParameter("Email",ChangeValues.SupplierEmail));
            parameters.Add(new ReportParameter("Comments", CurrentCatalogGroupLog.PdfNotes));
            parameters.Add(new ReportParameter("AcademicInstitution", ChangeValues.AcademicInstitutionName));
            parameters.Add(new ReportParameter("IncomeTax", ChangeValues.Deductions.FirstOrDefault(x=> x.DeductionType == DeductionNames.IncomeTaxAmount).DeductionAmount.ToString("c")));
            parameters.Add(new ReportParameter("IncomeTaxPerc", ChangeValues.IncomeTaxPerc.ToString()));
            parameters.Add(new ReportParameter("TotalDeductionsAmount", ChangeValues.Deductions.FirstOrDefault(x => x.DeductionType == DeductionNames.TotalDeductionsAmount).DeductionAmount.ToString("c")));
            parameters.Add(new ReportParameter("PaymentAmount", ChangeValues.PaymentAmount.Value.ToString("c")));
            parameters.Add(new ReportParameter("StateLabel", ChangeValues.StateLabel));
            parameters.Add(new ReportParameter("TotalVatAmount", ChangeValues.Deductions.FirstOrDefault(x => x.DeductionType == DeductionNames.TotalVatAmount).DeductionAmount.ToString("c")));
            parameters.Add(new ReportParameter("GrandTotalAmount", ChangeValues.GrandTotalAmount.Value.ToString("c")));

            localReport.SetParameters(parameters);

            ReportDataSource ds = new ReportDataSource("BookInCatalogInfo", BooksInCatalog.ToDataTable());
            localReport.DataSources.Clear();
            localReport.DataSources.Add(ds);
            localReport.Refresh();
        }
    }


}