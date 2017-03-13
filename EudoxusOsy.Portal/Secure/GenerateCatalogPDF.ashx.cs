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
    public class GenerateCatalogPDF : BaseHttpHandler
    {
        decimal IncomeTaxPerc;
        decimal IncomeTaxAmount;
        decimal TotalDeductionsAmount;
        decimal TotalVatAmount;

        protected int CatalogGroupID
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["id"]);
            }
        }

        protected string Comments
        {
            get
            {
                return Request.QueryString["comments"];
            }
        }

        List<BookInCatalogInfo> BooksInCatalog = new List<BookInCatalogInfo>();

        protected CatalogGroup CurrentCatalogGroup;


        protected override void DoProcessRequest()
        {
            CurrentCatalogGroup = new CatalogGroupRepository(UnitOfWork).Load(CatalogGroupID, 
                x => x.SupplierIBAN, 
                x => x.Supplier,
                x=> x.Phase,
                x => x.Institution,
                x => x.Deduction);

            BooksInCatalog = CurrentCatalogGroup.GetBooksInCatalogGroup(CurrentCatalogGroup.ID, UnitOfWork);

            var isBefore2014 = CurrentCatalogGroup.Phase.AcademicYear < 2014;
            var hasLogisticBooks = CurrentCatalogGroup.Supplier.HasLogisticBooks ?? false;
            decimal stamp; // χαρτόσημο

            if (hasLogisticBooks)
            {
                IncomeTaxPerc = 0.04m;
                stamp = 0.02m;
            }
            else
            {
                stamp = isBefore2014 ? 0.0306m : 0.03m;
                IncomeTaxPerc = 0.2m;
            }

            decimal deductionMTPY = 0m;
            decimal deductionStamp = 0m;
            decimal deductionOGA = 0m;

            if (CurrentCatalogGroup.Deduction != null)
            {

                if (isBefore2014)
                {
                    deductionMTPY = Math.Round(CurrentCatalogGroup.Deduction.MtpyPercentage / 100, 2) * CurrentCatalogGroup.TotalAmount.Value;
                }

                if (isBefore2014 || !hasLogisticBooks)
                {
                    deductionStamp = stamp * (hasLogisticBooks ? deductionMTPY : CurrentCatalogGroup.TotalAmount.Value);
                    deductionOGA = deductionStamp * Math.Round(CurrentCatalogGroup.Deduction.OgaPercentage / 100, 2);
                }


                if (hasLogisticBooks)
                {
                    IncomeTaxAmount = Math.Round((CurrentCatalogGroup.TotalAmount.Value - deductionStamp - deductionOGA - deductionMTPY) * IncomeTaxPerc, 2);
                }
                else
                {
                    IncomeTaxAmount = Math.Round(CurrentCatalogGroup.TotalAmount.Value * IncomeTaxPerc, 2);
                }

                TotalDeductionsAmount = Math.Round(deductionMTPY + deductionStamp + deductionOGA + IncomeTaxAmount, 2);
                TotalVatAmount = Math.Round(CurrentCatalogGroup.Deduction.Vat / 100 * CurrentCatalogGroup.TotalAmount.Value,2);
            }
            else
            {
                IncomeTaxAmount = Math.Round(CurrentCatalogGroup.TotalAmount.Value * IncomeTaxPerc, 2);
                TotalDeductionsAmount = IncomeTaxAmount;
                TotalVatAmount = CurrentCatalogGroup.Vat.HasValue ? CurrentCatalogGroup.Vat.Value : 0m;
            }

            var log = new CatalogGroupLog();

            var catalogGroupChangeValues = new CatalogGroupChangeValues();
            catalogGroupChangeValues.Action = enCatalogLogAction.PDFPrint;
            catalogGroupChangeValues.Amount = CurrentCatalogGroup.TotalAmount;
            catalogGroupChangeValues.ReporterID = CurrentCatalogGroup.SupplierID;
            catalogGroupChangeValues.IncludedCatalogs = BooksInCatalog;
            catalogGroupChangeValues.Deductions = new List<DeductionPdfObject>();
            catalogGroupChangeValues.Deductions.Add(new DeductionPdfObject() { DeductionType = DeductionNames.Mtpy, DeductionAmount = deductionMTPY });
            catalogGroupChangeValues.Deductions.Add(new DeductionPdfObject() { DeductionType = DeductionNames.Oga, DeductionAmount = deductionOGA });
            catalogGroupChangeValues.Deductions.Add(new DeductionPdfObject() { DeductionType = DeductionNames.Stamp, DeductionAmount = deductionStamp });
            catalogGroupChangeValues.Deductions.Add(new DeductionPdfObject() { DeductionType = DeductionNames.IncomeTaxAmount, DeductionAmount = IncomeTaxAmount });
            catalogGroupChangeValues.Deductions.Add(new DeductionPdfObject() { DeductionType = DeductionNames.TotalDeductionsAmount, DeductionAmount = TotalDeductionsAmount });
            catalogGroupChangeValues.Deductions.Add(new DeductionPdfObject() { DeductionType = DeductionNames.TotalVatAmount, DeductionAmount = TotalVatAmount });

            catalogGroupChangeValues.AcademicYearString = CurrentCatalogGroup.Phase.AcademicYearString;
            catalogGroupChangeValues.SupplierAFM = CurrentCatalogGroup.Supplier.AFM;
            catalogGroupChangeValues.SupplierName = CurrentCatalogGroup.Supplier.Name;
            catalogGroupChangeValues.SupplierDOY = CurrentCatalogGroup.Supplier.DOY;
            catalogGroupChangeValues.SupplierIBAN = CurrentCatalogGroup.SupplierIBAN != null ? CurrentCatalogGroup.SupplierIBAN.IBAN : string.Empty;
            catalogGroupChangeValues.SupplierEmail = CurrentCatalogGroup.Supplier.Email;
            catalogGroupChangeValues.AcademicInstitutionName = CurrentCatalogGroup.Institution.Name;
            catalogGroupChangeValues.IncomeTaxPerc = (int)(IncomeTaxPerc * 100);
            catalogGroupChangeValues.PaymentAmount = ((TotalVatAmount + CurrentCatalogGroup.TotalAmount.Value) - TotalDeductionsAmount);
            catalogGroupChangeValues.StateLabel = CurrentCatalogGroup.State.GetLabel();
            catalogGroupChangeValues.GrandTotalAmount = (TotalVatAmount + CurrentCatalogGroup.TotalAmount.Value);

            log.CreatedAt = DateTime.Today;
            log.CreatedBy = User.Identity.Name;
            log.CatalogGroup = CurrentCatalogGroup;
            log.Comments = enCatalogGroupLogAction.PDFPrint.GetLabel();
            log.PdfNotes = Comments;
            log.Amount = (double?)CurrentCatalogGroup.TotalAmount;
            log.SetNewValues(catalogGroupChangeValues);
            UnitOfWork.MarkAsNew(log);
            UnitOfWork.Commit();

            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename=CatalogGroupPDF-{0}.pdf", CatalogGroupID.ToString()));
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

            parameters.Add(new ReportParameter("YearFromToLiteral", CurrentCatalogGroup.Phase.AcademicYearString));
            parameters.Add(new ReportParameter("AFM", CurrentCatalogGroup.Supplier.AFM));
            parameters.Add(new ReportParameter("SupplierName", CurrentCatalogGroup.Supplier.Name ));
            parameters.Add(new ReportParameter("DOY", CurrentCatalogGroup.Supplier.DOY));
            parameters.Add(new ReportParameter("IBAN", CurrentCatalogGroup.SupplierIBAN != null ? CurrentCatalogGroup.SupplierIBAN.IBAN: string.Empty));
            parameters.Add(new ReportParameter("Email",CurrentCatalogGroup.Supplier.Email));
            parameters.Add(new ReportParameter("Comments", Comments));
            parameters.Add(new ReportParameter("AcademicInstitution", CurrentCatalogGroup.Institution.Name));
            parameters.Add(new ReportParameter("IncomeTax", IncomeTaxAmount.ToString("c")));
            parameters.Add(new ReportParameter("IncomeTaxPerc", ((int)(IncomeTaxPerc * 100)).ToString()));
            parameters.Add(new ReportParameter("TotalDeductionsAmount", TotalDeductionsAmount.ToString()));
            parameters.Add(new ReportParameter("PaymentAmount", ((TotalVatAmount + CurrentCatalogGroup.TotalAmount.Value) - TotalDeductionsAmount).ToString("c")));
            if (CurrentCatalogGroup.State == enCatalogGroupState.New)
            {
                parameters.Add(new ReportParameter("StateLabel", string.Format("ΠΡΟΣΩΡΙΝΗ ΚΑΤΑΣΤΑΣΗ -  {0}", CurrentCatalogGroup.State.GetLabel())));
            }

            parameters.Add(new ReportParameter("TotalVatAmount", TotalVatAmount.ToString("c")));
            parameters.Add(new ReportParameter("GrandTotalAmount", (TotalVatAmount + CurrentCatalogGroup.TotalAmount.Value).ToString("c")));
            parameters.Add(new ReportParameter("CatalogGroupID", CurrentCatalogGroup.ID.ToString()));


            localReport.SetParameters(parameters);

            ReportDataSource ds = new ReportDataSource("BookInCatalogInfo", BooksInCatalog.ToDataTable());
            localReport.DataSources.Clear();
            localReport.DataSources.Add(ds);
            localReport.Refresh();
        }
    }


}