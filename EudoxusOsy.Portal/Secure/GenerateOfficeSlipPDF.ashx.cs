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
    public class GenerateOfficeSlipPDF : BaseHttpHandler
    {
        protected DateTime OfficeSlipDate
        {
            get
            {
                return new DateTime(Convert.ToInt32(Request.QueryString["year"]), Convert.ToInt32(Request.QueryString["month"]), Convert.ToInt32(Request.QueryString["date"]));
            }
        }

        protected string ProtocolNumber
        {
            get
            {
                return Request.QueryString["protocol"];
            }
        }

        protected string Decision
        {
            get
            {
                return Request.QueryString["decision"];
            }
        }


        List<OfficeSlip> officeSlips = new List<OfficeSlip>();        
        decimal TotalAmount;

        protected override void DoProcessRequest()
        {
            if (OfficeSlipDate != null && OfficeSlipDate > DateTime.MinValue)
            {
                var paymentOrders = new PaymentOrderRepository(UnitOfWork).FindSentByOfficeSlipDate(OfficeSlipDate);
               
                paymentOrders.ForEach(x=>
                {
                    var os = new OfficeSlip()
                    {
                        SupplierName = x.CatalogGroup.Supplier.Name,
                        AFM = x.CatalogGroup.Supplier.AFM,
                        GroupID = x.GroupID,
                        PaymentOffice = !x.CatalogGroup.Supplier.PaymentPfoID.HasValue ? "ΕΦΟΡΙΑ" : (x.CatalogGroup.Supplier.PaymentPfoID == -1 ? x.CatalogGroup.Supplier.PaymentPfo : EudoxusOsyCacheManager<PublicFinancialOffice>.Current.Get(x.CatalogGroup.Supplier.PaymentPfoID.Value).Name),
                        AmountString = x.TotalAmount,
                        Amount = x.TotalAmountDecimal
                    };

                    officeSlips.Add(os);
                });
                TotalAmount = officeSlips.Sum(x => x.Amount);

                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename=OfficeSlip-{0}.pdf", OfficeSlipDate.ToShortDateString()));
                CreatePDF();
            }
        }

        private void CreatePDF()
        {
            using (LocalReport lr = new LocalReport())
            {
                ConfigureReport(lr);

                string deviceInfo = @"<DeviceInfo>
                    <OutputFormat>PDF</OutputFormat>
                    <PageHeight>29.7cm</PageHeight>
                    <PageWidth>21cm</PageWidth>
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
            localReport.ReportPath = HttpContext.Current.Server.MapPath("~/_rdlc/OfficeSlip.rdlc");

            List<ReportParameter> parameters = new List<ReportParameter>();

            parameters.Add(new ReportParameter("Amount", TotalAmount.ToString("c")));
            parameters.Add(new ReportParameter("AmountInWords",  BusinessHelper.NumberToWordsGenitive(TotalAmount)));
            parameters.Add(new ReportParameter("OfficeSlipDate", OfficeSlipDate.ToShortDateString()));
            parameters.Add(new ReportParameter("ProtocolNumber", ProtocolNumber));
            parameters.Add(new ReportParameter("Decision", Decision));
            parameters.Add(new ReportParameter("SupervisorName", "ΝΙΚΗ ΚΕΡΑΜΕΩΣ"));

            localReport.SetParameters(parameters);

            ReportDataSource ds = new ReportDataSource("OfficeSlipSchema_OfficeSlip", officeSlips.ToDataTable());            
            localReport.DataSources.Clear();
            localReport.DataSources.Add(ds);
            
            localReport.Refresh();
        }
    }

    
}