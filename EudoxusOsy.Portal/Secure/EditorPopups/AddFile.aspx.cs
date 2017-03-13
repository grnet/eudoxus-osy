using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Imis.Domain;
using EudoxusOsy.Portal.Controls;
using System.Web.Security;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Utils;
using EudoxusOsy.Portal.Utils;
using DevExpress.Web;

namespace EudoxusOsy.Portal.Secure.EditorPopups
{
    public partial class AddFile : BaseEntityPortalPage<Supplier>
    {
        #region [ Entity Fill ]

        protected override void Fill()
        {
            int supplierID;
            if (int.TryParse(Request.QueryString["id"], out supplierID) && supplierID > 0)
            {
                Entity = new SupplierRepository(UnitOfWork).Load(supplierID);
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "hidePopup", "window.parent.popUp.hide();", true);
            }
        }

        #endregion

        #region [ Button Handlers ]

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            FileSelfPublisher newFileSelfPublisher = new FileSelfPublisher()
            {
                SupplierID = Entity.ID,
                FileID = (int)ucFileUpload.ExtractValue(),
                CreatedAt = DateTime.Today,
                CreatedBy = User.Identity.Name,
                PhaseID = ddlSelectPhase.GetSelectedInteger().Value,
                IsActive = true
            };

            UnitOfWork.MarkAsNew(newFileSelfPublisher);
            UnitOfWork.Commit();
            //Delete File

            //else
            //{
            //    CurrentSelfPublisherFile = Entity.FileSelfPublishers.FirstOrDefault(x => x.PhaseID == PhaseID && x.IsActive);
            //    if (CurrentSelfPublisherFile != null)
            //    {
            //        CurrentFile = new FileRepository(UnitOfWork).Load(CurrentSelfPublisherFile.FileID);
            //        UnitOfWork.MarkAsDeleted(CurrentSelfPublisherFile);
            //        UnitOfWork.MarkAsDeleted(CurrentFile);
            //    }
            //}
            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }

        #endregion

        protected void ddlSelectPhase_Init(object sender, EventArgs e)
        {
            ddlSelectPhase.FillPhases(true);
        }
    }
}