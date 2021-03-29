using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using EudoxusOsy.Portal.Utils;
using EudoxusOsy.Utils;

namespace EudoxusOsy.Portal.Secure.EditorPopups
{
    public partial class ViewSupplier : BaseSecureEntityPortalPage<Supplier>
    {
        protected FileSelfPublisher CurrentSelfPublisherFile { get; set; }
        protected File CurrentFile { get; set; }

        #region [ Entity Fill ]

        protected override void Fill()
        {
            int SupplierID;
            if (int.TryParse(Request.QueryString["id"], out SupplierID) && SupplierID > 0)
            {
                Entity = new SupplierRepository(UnitOfWork).Load(SupplierID,
                                                                    x => x.Reporter,
                                                                    x => x.SupplierDetail,
                                                                    x => x.SupplierIBANs,
                                                                    x => x.FileSelfPublishers);
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "hidePopup", "window.parent.popUp.hide();", true);
            }
        }

        protected override bool Authorize()
        {
            return EudoxusOsyRoleProvider.IsAuthorizedMinistryUserForView()
                || Entity.ReporterID == User.Identity.ReporterID; 
        }

        #endregion

        #region [ Page Inits ]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsAuthorized)
            {
                ClientScript.RegisterStartupScript(GetType(), "hidePopup", "window.parent.popUp.hide();", true);
            }
            else
            {
                if (!IsPostBack)
                {
                    ucSupplierMinistryView.Entity = Entity;
                    ucSupplierMinistryView.Bind();
                    //CurrentSelfPublisherFile = Entity.FileSelfPublishers.FirstOrDefault(x => x.PhaseID == PhaseID && x.IsActive);
                    //if (CurrentSelfPublisherFile != null)
                    //{
                    //    CurrentFile = new FileRepository(UnitOfWork).Load(CurrentSelfPublisherFile.FileID);
                    //    ucSupplierMinistryView.BindFile(CurrentFile);
                    //}

                    if (Entity.SupplierType != enSupplierType.SelfPublisher)
                    {
                        ClientScript.RegisterStartupScript(GetType(), "hideSubmitButton", "window.parent.btnSubmit.SetVisible(false);", true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(GetType(), "showSubmitButton", "window.parent.btnSubmit.SetVisible(true);", true);
                    }
                }
            }
        }

        #endregion

        protected void btnSubmitHidden_Click(object sender, EventArgs e)
        {
            var changesValue = ucSupplierMinistryView.Extract();

            if (IsAuthorized)
            {
                Entity.HasLogisticBooks = !changesValue.NoLogisticBooks;

                UnitOfWork.Commit();
                ClientScript.RegisterStartupScript(GetType(), "hidePopup", "window.parent.popUp.hide();", true);
            }
        }
    }
}