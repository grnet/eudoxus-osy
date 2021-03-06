﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.Portal.Controls;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Services;
using EudoxusOsy.Utils;
using log4net;

namespace EudoxusOsy.Portal.Admin
{
    public partial class EndOfPhaseProcedures : BaseEntityPortalPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnInsertCorrectedFiles_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateReceiptsHelper.InsertCorrectedKPSOnlyFile(Config.KpsReceiptsOnlyFileName);
                UpdateReceiptsHelper.InsertCorrectedOSYOnlyFile(Config.LocalReceiptsOnlyFileName);
            }
            catch (Exception ex)
            {
                Notify(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }

            Notify("Η εισαγωγή των διορθωμένων αρχείων ολοκληρώθηκε επιτυχώς");
        }

        protected void btnComplementReceipts_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateReceiptsHelper.ComplementReceipts();
            }
            catch (Exception ex)
            {
                Notify(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }

            Notify("Η διαδικασία ενημέρωσης των παραδόσεων (Receipt) ολοκληρώθηκε επιτυχώς");
        }

        protected void btnCompareReceipts_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateReceiptsHelper.CompareXmlReceipts();
            }
            catch (Exception ex)
            {
                Notify(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }

            Notify("Η διαδικασία σύγκρισης των παραδόσεων ολοκληρώθηκε επιτυχώς");
        }

        protected void btnInsertXml_OnClick(object sender, EventArgs e)
        {            
            if (string.IsNullOrEmpty(txtXmlPath.Text))
            {
                string kpsFileName = Path.Combine(Config.XmlFilesPath, "KpsReceiptsOnly.xml");
                SQLXMLBulkLoad.doBulkLoad(kpsFileName);
            }
            else
            {
                SQLXMLBulkLoad.doBulkLoad(txtXmlPath.Text);
            }            
        }
    }
}
