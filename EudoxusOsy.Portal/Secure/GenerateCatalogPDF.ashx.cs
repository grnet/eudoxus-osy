using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Web;

namespace EudoxusOsy.Portal.Secure
{
    public class GenerateCatalogPDF : BaseHttpHandler
    {
        decimal IncomeTaxPerc;
        decimal IncomeTaxAmount;
        decimal TotalDeductionsAmount;
        decimal TotalVatAmount;
        decimal deductionStamp = 0m;
        decimal deductionOGA = 0m;

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

        protected bool SaveLog
        {
            get
            {
                return Request.QueryString["saveLog"] != null && Convert.ToBoolean(Request.QueryString["saveLog"]);
            }
        }

        List<BookInCatalogInfo> BooksInCatalog = new List<BookInCatalogInfo>();

        protected CatalogGroup CurrentCatalogGroup;


        protected override void DoProcessRequest()
        {
            CurrentCatalogGroup = new CatalogGroupRepository(UnitOfWork).Load(CatalogGroupID,                
                x => x.Supplier,
                x => x.Supplier.SupplierDetail,
                x => x.Supplier.SupplierIBANs,
                x => x.Phase,
                x => x.Institution,
                x => x.Deduction);

            BooksInCatalog = CurrentCatalogGroup.GetBooksInCatalogGroupByBook(CurrentCatalogGroup.ID, UnitOfWork);
            
            var hasLogisticBooks = CurrentCatalogGroup.Supplier.HasLogisticBooks ?? false;

            decimal stamp; // χαρτόσημο                        

            if (hasLogisticBooks)
            {
                IncomeTaxPerc = 0.04m;                
                IncomeTaxAmount = Math.Round(CurrentCatalogGroup.TotalAmount.Value * IncomeTaxPerc, 2, MidpointRounding.AwayFromZero);

                TotalDeductionsAmount = IncomeTaxAmount;

                if (CurrentCatalogGroup.Deduction != null)
                {                                    
                    TotalVatAmount = Math.Round((CurrentCatalogGroup.Deduction.Vat / 100) * CurrentCatalogGroup.TotalAmount.Value, 2, MidpointRounding.AwayFromZero);
                }
                else
                {                    
                    TotalVatAmount = CurrentCatalogGroup.Vat.HasValue ? CurrentCatalogGroup.Vat.Value : 0m;
                }
            }
            else
            {
                stamp = 0.03m;
                IncomeTaxPerc = 0.04m;

                deductionStamp = stamp * CurrentCatalogGroup.TotalAmount.Value;
                deductionOGA = deductionStamp * 0.2m;

                IncomeTaxAmount = Math.Round(CurrentCatalogGroup.TotalAmount.Value * IncomeTaxPerc, 2, MidpointRounding.AwayFromZero);
                TotalDeductionsAmount = Math.Round(deductionStamp + deductionOGA + IncomeTaxAmount, 2, MidpointRounding.AwayFromZero);                
            }
                        
            var log = new CatalogGroupLog();

            var catalogGroupChangeValues = new CatalogGroupChangeValues();
            catalogGroupChangeValues.Action = enCatalogLogAction.PDFPrint;
            catalogGroupChangeValues.Amount = CurrentCatalogGroup.TotalAmount;
            catalogGroupChangeValues.ReporterID = CurrentCatalogGroup.SupplierID;
            catalogGroupChangeValues.IncludedCatalogs = BooksInCatalog;    
            catalogGroupChangeValues.Deductions = new List<DeductionPdfObject>();
            catalogGroupChangeValues.Deductions.Add(new DeductionPdfObject() { DeductionType = DeductionNames.Mtpy, DeductionAmount = 0 });
            catalogGroupChangeValues.Deductions.Add(new DeductionPdfObject() { DeductionType = DeductionNames.Oga, DeductionAmount = deductionOGA });
            catalogGroupChangeValues.Deductions.Add(new DeductionPdfObject() { DeductionType = DeductionNames.Stamp, DeductionAmount = deductionStamp });
            catalogGroupChangeValues.Deductions.Add(new DeductionPdfObject() { DeductionType = DeductionNames.IncomeTaxAmount, DeductionAmount = IncomeTaxAmount });
            catalogGroupChangeValues.Deductions.Add(new DeductionPdfObject() { DeductionType = DeductionNames.TotalDeductionsAmount, DeductionAmount = TotalDeductionsAmount });
            catalogGroupChangeValues.Deductions.Add(new DeductionPdfObject() { DeductionType = DeductionNames.TotalVatAmount, DeductionAmount = TotalVatAmount });

            catalogGroupChangeValues.AcademicYearString = CurrentCatalogGroup.Phase.AcademicYearString;
            catalogGroupChangeValues.SupplierAFM = CurrentCatalogGroup.Supplier.AFM;
            catalogGroupChangeValues.SupplierName = CurrentCatalogGroup.Supplier.Name;
            catalogGroupChangeValues.SupplierDOY = CurrentCatalogGroup.Supplier.GetDOY();
            catalogGroupChangeValues.SupplierIBAN = CurrentCatalogGroup.Supplier.GetLatestIBAN(true);
            catalogGroupChangeValues.SupplierEmail = CurrentCatalogGroup.Supplier.Email;
            catalogGroupChangeValues.AcademicInstitutionName = CurrentCatalogGroup.Institution.Name;
            catalogGroupChangeValues.IncomeTaxPerc = (int)(IncomeTaxPerc * 100);
            catalogGroupChangeValues.PaymentAmount = ((TotalVatAmount + CurrentCatalogGroup.TotalAmount.Value) - TotalDeductionsAmount);

            if (CurrentCatalogGroup.State == enCatalogGroupState.Selected)
            {
                catalogGroupChangeValues.StateLabel = "Επιλεχθείσα";
            }
            else if (CurrentCatalogGroup.State == enCatalogGroupState.Approved)
            {
                catalogGroupChangeValues.StateLabel = "Εγκεκριμένη";
            }
            else if (CurrentCatalogGroup.State < enCatalogGroupState.Sent)
            {                
                catalogGroupChangeValues.StateLabel = CurrentCatalogGroup.State.GetLabel();
            }

            catalogGroupChangeValues.GrandTotalAmount = (TotalVatAmount + CurrentCatalogGroup.TotalAmount.Value);

            catalogGroupChangeValues.HideVat = CurrentCatalogGroup.Supplier.ZeroVatEligible;             

            log.CreatedAt = DateTime.Now;
            log.CreatedBy = User.Identity.Name;
            log.CatalogGroup = CurrentCatalogGroup;
            log.Comments = enCatalogGroupLogAction.PDFPrint.GetLabel();
            log.PdfNotes = Comments;
            log.Amount = (double?)CurrentCatalogGroup.TotalAmount;
            log.SetNewValues(catalogGroupChangeValues);
            log.OldState = CurrentCatalogGroup.State;
            log.NewState = CurrentCatalogGroup.State;
            log.PdfTypeInt = (int) enPdfType.CatalogGroupLogByBook;

            if (SaveLog)
            {
                UnitOfWork.MarkAsNew(log);
                UnitOfWork.Commit();
            }

            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename=CatalogGroupPDF-{0}.pdf", CatalogGroupID));

            log.ConvertLogInfoToDto().CreatePDF(Response, "~/_rdlc/CatalogInvoiceByBook.rdlc");
        }   
    }
}