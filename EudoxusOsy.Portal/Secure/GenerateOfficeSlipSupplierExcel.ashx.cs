using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using Microsoft.Reporting.WebForms;

namespace EudoxusOsy.Portal.Secure
{
    public class GenerateOfficeSlipSupplierExcel : BaseHttpHandler
    {
        const decimal IncomeTaxPerc = 0.04m;
        const decimal stamp = 0.03m;

        protected DateTime OfficeSlipDate
        {
            get
            {
                return new DateTime(Convert.ToInt32(Request.QueryString["year"]),
                    Convert.ToInt32(Request.QueryString["month"]), Convert.ToInt32(Request.QueryString["date"]));
            }
        }

        protected int SupplierKpsID
        {
            get { return Convert.ToInt32(Request.QueryString["SupplierKpsID"]); }
        }

        protected override void DoProcessRequest()
        {
            if (OfficeSlipDate != null && OfficeSlipDate > DateTime.MinValue && SupplierKpsID > 0)
            {
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition",
                    string.Format("attachment; filename=OfficeSlipSupplier-{0}_{1}.xls",
                        OfficeSlipDate.ToShortDateString(), SupplierKpsID));
                CreatePDF();
            }
        }

        private void CreatePDF()
        {
            using (LocalReport lr = new LocalReport())
            {
                if (!ConfigureReport(lr))
                {
                    return;
                }

                string deviceInfo = @"<DeviceInfo>
                    <OutputFormat>Excel</OutputFormat>
                    <PageHeight>29.7cm</PageHeight>
                    <PageWidth>21cm</PageWidth>
                    <MarginTop>0.5in</MarginTop>
                    <MarginLeft>0.0in</MarginLeft>
                    <MarginRight>0.0in</MarginRight>
                    <MarginBottom>0.5in</MarginBottom>
                    </DeviceInfo>";

                string reportType = "Excel";
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

        private bool ConfigureReport(LocalReport localReport)
        {
            localReport.ReportPath = HttpContext.Current.Server.MapPath("~/_rdlc/OfficeSlipSupplier.rdlc");
            List<ReportParameter> parameters = new List<ReportParameter>();

            var currentSupplier = new SupplierRepository().FindByKpsID(SupplierKpsID, x => x.SupplierIBANs);

            if (currentSupplier == null)
            {
                return false;
            }

            var paymentOrders = new PaymentOrderRepository(UnitOfWork).FindSentByOfficeSlipDateWithInvoices(OfficeSlipDate, currentSupplier.ID);

            if (paymentOrders == null || !paymentOrders.Any())
            {
                return false;
            }

            List<OfficeSlipSupplier> oss = new List<OfficeSlipSupplier>();
            List<OfficeSlipSupplier> totalOss = new List<OfficeSlipSupplier>();
            int currentPhaseID = paymentOrders.FirstOrDefault().CatalogGroup.PhaseID;
            paymentOrders.ForEach(x =>
            {
                var totalOssResult = new OfficeSlipSupplier();
                List<OfficeSlipSupplier> cs = ProcessGroup(x.CatalogGroup, out totalOssResult);
                oss.AddRange(cs);
                totalOss.Add(totalOssResult);
            });

            if (oss.Count == 0)
            {
                return false;
            }

            var totalInvoiceValue = totalOss.Sum(x => x.InvoiceValue);
            var totalDeductionIncomeTax = Math.Round(totalInvoiceValue * IncomeTaxPerc, 2, MidpointRounding.AwayFromZero);
            decimal totalDeduction = 0m;
            if (currentSupplier.HasLogisticBooks == true)
            {
                totalDeduction = totalDeductionIncomeTax;
            }
            else
            {

                var totalDeductionStamp = stamp * totalInvoiceValue;
                var totalDeductionOGA = totalDeductionStamp * 0.2m;
                totalDeduction = Math.Round(totalDeductionIncomeTax + totalDeductionOGA + totalDeductionStamp, 2);
            }

            parameters.Add(new ReportParameter("TotalIncomeTax", totalDeductionIncomeTax.ToString("c")));
            parameters.Add(new ReportParameter("TotalDeduction", totalDeduction.ToString("c")));
            parameters.Add(new ReportParameter("TotalPayable", ((totalOss.Sum(x => x.Vat) + totalOss.Sum(x => x.InvoiceValue) - totalDeduction)).ToString("c")));
            parameters.Add(new ReportParameter("TotalValue", totalInvoiceValue.ToString("c")));
            parameters.Add(new ReportParameter("TotalVat", totalOss.Sum(x => x.Vat).ToString("c")));
            parameters.Add(new ReportParameter("TotalFinalAmount", (totalOss.Sum(x => x.Vat) + totalOss.Sum(x => x.InvoiceValue)).ToString("c")));

            //parameters.Add(new ReportParameter("TotalIncomeTax", totalOss.Sum(x=> x.DeductionIncomeTax).ToString("c")));
            //parameters.Add(new ReportParameter("TotalDeduction", totalOss.Sum(x => x.Deduction).ToString("c")));
            //parameters.Add(new ReportParameter("TotalPayable", totalOss.Sum(x => x.PayableAmount).ToString("c")));
            //parameters.Add(new ReportParameter("TotalValue", totalOss.Sum(x => x.InvoiceValue).ToString("c")));
            //parameters.Add(new ReportParameter("TotalVat", totalOss.Sum(x => x.Vat).ToString("c")));
            //parameters.Add(new ReportParameter("TotalFinalAmount", (totalOss.Sum(x=> x.Vat)+ totalOss.Sum(x=> x.InvoiceValue)).ToString("c")));

            parameters.Add(new ReportParameter("SupplierName", currentSupplier.Name));
            parameters.Add(new ReportParameter("Afm", currentSupplier.AFM));
            parameters.Add(new ReportParameter("DateToYDE", OfficeSlipDate.ToShortDateString()));
            parameters.Add(new ReportParameter("SupervisorName", "ΑΠΟΣΤΟΛΟΣ ΔΗΜΗΤΡΟΠΟΥΛΟΣ"));
            parameters.Add(new ReportParameter("IBAN", currentSupplier.GetLatestIBAN(false)));
            parameters.Add(new ReportParameter("Email", currentSupplier.Email));
            parameters.Add(new ReportParameter("DOY", string.IsNullOrEmpty(currentSupplier.PaymentPfo) ? (currentSupplier.PaymentPfoID.HasValue ? EudoxusOsyCacheManager<PublicFinancialOffice>.Current.Get(currentSupplier.PaymentPfoID.Value).Name : currentSupplier.DOY) : currentSupplier.PaymentPfo));
            parameters.Add(new ReportParameter("AcademicYear", EudoxusOsyCacheManager<Phase>.Current.Get(currentPhaseID).AcademicYearString));



            localReport.SetParameters(parameters);

            ReportDataSource ds = new ReportDataSource("OfficeSlipSupplier", oss.ToDataTable());
            localReport.DataSources.Clear();
            localReport.DataSources.Add(ds);

            localReport.Refresh();

            return true;
        }

        private List<OfficeSlipSupplier> ProcessGroup(CatalogGroup catalogGroup, out OfficeSlipSupplier totalCalculatedAmounts)
        {
            List<OfficeSlipSupplier> lst = new List<OfficeSlipSupplier>();
            var count = catalogGroup.Invoices.Count;
            int groupCounter = 0;
            totalCalculatedAmounts = new OfficeSlipSupplier();

            foreach (Invoice invoice in catalogGroup.Invoices)
            {
                groupCounter++;
                var amount1123 = CalculateAmount1123(catalogGroup, invoice);

                var oss = new OfficeSlipSupplier()
                {
                    GroupID = catalogGroup.ID,
                    InvoiceNumber = invoice.InvoiceNumber,
                    InvoiceDate = invoice.InvoiceDate.ToShortDateString(),
                    InvoiceValue = invoice.InvoiceValue,
                    Vat = amount1123 - invoice.InvoiceValue,
                    FinalAmount = amount1123
                };

                var hasLogisticBooks = catalogGroup.Supplier.HasLogisticBooks ?? false;

                if (hasLogisticBooks)
                {
                    var IncomeTaxAmount = Math.Round(invoice.InvoiceValue * IncomeTaxPerc, 2, MidpointRounding.AwayFromZero);

                    var TotalDeductionsAmount = IncomeTaxAmount;


                }
                else
                {
                    var deductionStamp = stamp * invoice.InvoiceValue;
                    var deductionOGA = deductionStamp * 0.2m;

                    var IncomeTaxAmount = Math.Round(invoice.InvoiceValue * IncomeTaxPerc, 2, MidpointRounding.AwayFromZero);
                    var TotalDeductionsAmount = Math.Round(deductionStamp + deductionOGA + IncomeTaxAmount, 2, MidpointRounding.AwayFromZero);

                }


                lst.Add(oss);

                if (groupCounter == count)
                {

                    var totalAmountWithVAT = CalculateAmount1123ForCatalogGroup(catalogGroup, catalogGroup.InvoiceSum.Value);
                    var totalOss = new OfficeSlipSupplier()
                    {
                        GroupID = catalogGroup.ID,
                        InvoiceNumber = "Κρατήσεις και πληρωτέο ποσό",
                        InvoiceDate = "",
                        InvoiceValue = catalogGroup.InvoiceSum.Value,
                        Vat = 0m,
                        FinalAmount = 0m
                    };


                    if (hasLogisticBooks)
                    {
                        var IncomeTaxPerc = 0.04m;
                        var IncomeTaxAmount = Math.Round((decimal)catalogGroup.InvoiceSum.Value * IncomeTaxPerc, 2, MidpointRounding.AwayFromZero);

                        var TotalDeductionsAmount = IncomeTaxAmount;
                        decimal TotalVatAmount = 0m;
                        if (catalogGroup.Deduction != null)
                        {
                            TotalVatAmount = Math.Round((catalogGroup.Deduction.Vat / 100) * (decimal)catalogGroup.InvoiceSum.Value, 2, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            TotalVatAmount = catalogGroup.Vat.HasValue ? catalogGroup.Vat.Value : 0m;
                        }
                        totalOss.PayableAmount = totalAmountWithVAT - TotalDeductionsAmount;
                        totalOss.DeductionIncomeTax = IncomeTaxAmount;
                        totalOss.Deduction = TotalDeductionsAmount;
                        totalOss.Vat = TotalVatAmount;
                    }
                    else
                    {
                        var IncomeTaxPerc = 0.04m;

                        var deductionStamp = stamp * (decimal)catalogGroup.InvoiceSum.Value;
                        var deductionOGA = deductionStamp * 0.2m;

                        var IncomeTaxAmount = Math.Round((decimal)catalogGroup.InvoiceSum.Value * IncomeTaxPerc, 2, MidpointRounding.AwayFromZero);
                        var TotalDeductionsAmount = Math.Round(deductionStamp + deductionOGA + IncomeTaxAmount, 2, MidpointRounding.AwayFromZero);
                        totalOss.PayableAmount = totalAmountWithVAT - TotalDeductionsAmount;
                        totalOss.DeductionIncomeTax = IncomeTaxAmount;
                        totalOss.Deduction = TotalDeductionsAmount;
                    }

                    totalCalculatedAmounts = totalOss;
                    //lst.Add(totalOss);
                }
            }
            return lst;
        }

        private decimal CalculateAmount1123(CatalogGroup group, Invoice invoice)
        {
            decimal result;

            if (group.Vat.HasValue)
            {
                result = invoice.InvoiceValue + group.Vat.Value * invoice.InvoiceValue / group.InvoiceSum.Value;
            }
            else if (group.Deduction == null)
            {
                result = invoice.InvoiceValue;
            }
            else
            {
                result = invoice.InvoiceValue * (1 + group.Deduction.Vat / 100);
            }

            return Math.Round(result, 2, MidpointRounding.AwayFromZero);
        }

        private decimal CalculateAmount1123ForCatalogGroup(CatalogGroup group, decimal totalValue)
        {
            decimal result;

            if (group.Vat.HasValue)
            {
                result = totalValue + group.Vat.Value;
            }
            else if (group.Deduction == null)
            {
                result = totalValue;
            }
            else
            {
                result = totalValue * (1 + group.Deduction.Vat / 100);
            }

            return Math.Round(result, 2);
        }
    }
}