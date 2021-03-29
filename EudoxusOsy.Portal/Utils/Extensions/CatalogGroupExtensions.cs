using System.Collections.Generic;
using System.Linq;
using System.Web;
using EudoxusOsy.BusinessModel;
using Microsoft.Reporting.WebForms;

namespace EudoxusOsy.Portal
{
    public static class CatalogGroupExtensions
    {
        public static CatalogGroupLogPDFDTO ConvertLogInfoToDto(this CatalogGroupLog catalogGroupLog)
        {
            CatalogGroupChangeValues changeValues = catalogGroupLog.GetNewValues();

            CatalogGroupLogPDFDTO logPdfdto = new CatalogGroupLogPDFDTO();

            logPdfdto.CatalogGroupID = catalogGroupLog.GroupID.ToString();
            logPdfdto.YearFromToLiteral = changeValues.AcademicYearString;
            logPdfdto.AFM = changeValues.SupplierAFM;
            logPdfdto.SupplierName = changeValues.SupplierName;
            logPdfdto.DOY = changeValues.SupplierDOY;
            logPdfdto.IBAN = changeValues.SupplierIBAN;
            logPdfdto.Email = changeValues.SupplierEmail;
            logPdfdto.Comments = catalogGroupLog.PdfNotes;
            logPdfdto.AcademicInstitution = changeValues.AcademicInstitutionName;
            logPdfdto.IncomeTax = changeValues.Deductions.FirstOrDefault(x => x.DeductionType == DeductionNames.IncomeTaxAmount).DeductionAmount.ToString("c");
            logPdfdto.IncomeTaxPerc = changeValues.IncomeTaxPerc.ToString();
            logPdfdto.TotalDeductionsAmount = changeValues.Deductions.FirstOrDefault(x => x.DeductionType == DeductionNames.TotalDeductionsAmount).DeductionAmount.ToString("c");
            logPdfdto.PaymentAmount = changeValues.PaymentAmount.Value.ToString("c");
           
            logPdfdto.StateLabel = changeValues.StateLabel;
           
            logPdfdto.TotalVatAmount = changeValues.Deductions.FirstOrDefault(x => x.DeductionType == DeductionNames.TotalVatAmount).DeductionAmount.ToString("c");
            logPdfdto.GrandTotalAmount = changeValues.GrandTotalAmount.Value.ToString("c");

            logPdfdto.HideVAT = changeValues.HideVat + "";

            logPdfdto.StampAmount = changeValues.Deductions.FirstOrDefault(x => x.DeductionType == DeductionNames.Stamp).DeductionAmount.ToString("c");
            logPdfdto.OgaAmount = changeValues.Deductions.FirstOrDefault(x => x.DeductionType == DeductionNames.Oga).DeductionAmount.ToString("c");

            logPdfdto.BookInCatalogInfo = changeValues.IncludedCatalogs.ToDataTable();
            return logPdfdto;
        }

        public static void CreatePDF(this CatalogGroupLogPDFDTO currentCatalogGroup, HttpResponse httpResponse, string mapPath)
        {
            using (LocalReport lr = new LocalReport())
            {
                ConfigureReport(currentCatalogGroup, lr, mapPath);

                string deviceInfo = @"<DeviceInfo>
                    <OutputFormat>PDF</OutputFormat>
                    <PageWidth>29.7cm</PageWidth>
                    <PageHeight>21cm</PageHeight>
                    <MarginTop>0.1in</MarginTop>
                    <MarginLeft>0.0in</MarginLeft>
                    <MarginRight>0.0in</MarginRight>
                    <MarginBottom>0.1in</MarginBottom>
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

                httpResponse.BinaryWrite(renderedBytes);
            }
        }

        private static void ConfigureReport(this CatalogGroupLogPDFDTO currentCatalogGroup, LocalReport localReport,  string mapPath)
        {
            localReport.ReportPath = HttpContext.Current.Server.MapPath(mapPath);

            List<ReportParameter> parameters = new List<ReportParameter>();

            parameters.Add(new ReportParameter("CatalogGroupID", currentCatalogGroup.CatalogGroupID));
            parameters.Add(new ReportParameter("YearFromToLiteral", currentCatalogGroup.YearFromToLiteral));
            parameters.Add(new ReportParameter("AFM", currentCatalogGroup.AFM));
            parameters.Add(new ReportParameter("SupplierName", currentCatalogGroup.SupplierName));
            parameters.Add(new ReportParameter("DOY", currentCatalogGroup.DOY));
            parameters.Add(new ReportParameter("IBAN", currentCatalogGroup.IBAN));
            parameters.Add(new ReportParameter("Email", currentCatalogGroup.Email));
            parameters.Add(new ReportParameter("Comments", currentCatalogGroup.Comments));
            parameters.Add(new ReportParameter("AcademicInstitution", currentCatalogGroup.AcademicInstitution));
            parameters.Add(new ReportParameter("IncomeTax", currentCatalogGroup.IncomeTax));
            parameters.Add(new ReportParameter("IncomeTaxPerc", currentCatalogGroup.IncomeTaxPerc));
            parameters.Add(new ReportParameter("TotalDeductionsAmount", currentCatalogGroup.TotalDeductionsAmount));
            parameters.Add(new ReportParameter("PaymentAmount", currentCatalogGroup.PaymentAmount));

            parameters.Add(new ReportParameter("StateLabel", currentCatalogGroup.StateLabel));

            parameters.Add(new ReportParameter("TotalVatAmount", currentCatalogGroup.TotalVatAmount));
            parameters.Add(new ReportParameter("GrandTotalAmount", currentCatalogGroup.GrandTotalAmount));

            parameters.Add(new ReportParameter("HideVAT", currentCatalogGroup.HideVAT));

            parameters.Add(new ReportParameter("StampAmount", currentCatalogGroup.StampAmount));
            parameters.Add(new ReportParameter("OgaAmount", currentCatalogGroup.OgaAmount));

            localReport.SetParameters(parameters);

            ReportDataSource ds = new ReportDataSource("BookInCatalogInfo", currentCatalogGroup.BookInCatalogInfo);
            localReport.DataSources.Clear();
            localReport.DataSources.Add(ds);
            localReport.Refresh();

            
        }
    }
}