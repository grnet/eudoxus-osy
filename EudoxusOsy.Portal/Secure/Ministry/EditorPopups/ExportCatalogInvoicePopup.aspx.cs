﻿using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EudoxusOsy.Portal.Secure.Ministry.EditorPopups
{
    public partial class ExportCatalogInvoicePopup : BaseSecureEntityPortalPage<Catalog>
    {
        protected int CatalogGroupID
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["id"]);
            }
        }

        protected override void Fill()
        {
        }

        protected override bool Authorize()
        {
            return User.IsInRole(RoleNames.Helpdesk) || User.IsInRole(RoleNames.SuperHelpdesk) || User.IsInRole(RoleNames.SystemAdministrator) || User.IsInRole(RoleNames.MinistryPayments);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsAuthorized)
            {
                ClientScript.RegisterStartupScript(GetType(), "closePopup", "window.parent.popUp.hide();", true);
            }
       }

        protected void btnSubmitHidden_Click(object sender, EventArgs e)
        {
            CatalogGroup currentCatalogGroup = new CatalogGroupRepository(UnitOfWork).Load(CatalogGroupID, x => x.Catalogs);
            CatalogGroupLog log = new CatalogGroupLog();
            
            /**
                Moved Log inside the handler
            */ 
            //TODO: Update OfficeSlipNumber and OfficeSlipDate when producing the pdf file.

            Response.Redirect(string.Format("~/Secure/GenerateCatalogPDF.ashx?id={0}&comments={1}", CatalogGroupID,  HttpUtility.UrlEncode(txtComments.Text)));
        }
    }
}