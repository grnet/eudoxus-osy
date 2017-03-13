using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using Imis.Domain;
using System.Drawing;
using DevExpress.Web;
using EudoxusOsy.Utils;
using System.Web.Security;
using System.IO;

namespace EudoxusOsy.Portal.Secure.EditorPopups
{
    public partial class ManageFiles : BaseEntityPortalPage<Supplier>
    {
        #region [ Entity Fill ]

        protected int CurrentSupplierID;

        protected override void Fill()
        {
            if (int.TryParse(Request.QueryString["id"], out CurrentSupplierID) && CurrentSupplierID > 0)
            {
                Entity = new SupplierRepository(UnitOfWork).Load(CurrentSupplierID, x => x.FileSelfPublishers);

                if (Entity == null)
                {
                    ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
                }
            }
        }

        #endregion

        #region [ Page Inits ]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Roles.IsUserInRole(RoleNames.Supplier))
            {
                btnAddFile.Visible = false;
                gvFiles.Columns.Where(x => x.Name == "Actions").First().Visible = false;
            }
            else
            {
                btnAddFile.ClientSideEvents.Click = string.Format("function (s,e) {{ showAddFilePopup({0}); }}", CurrentSupplierID);
            }
        }


        #endregion

        #region [ Button Handlers ]

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            gvFiles.DataBind();
        }

        protected void btnDeleteFile_Click(object sender, EventArgs e)
        {
            var fileID = int.Parse(hfFileID.Value);

            var CurrentSelfPublisherFile = Entity.FileSelfPublishers.FirstOrDefault(x => x.FileID == fileID);
            if (CurrentSelfPublisherFile != null)
            {
                var CurrentFile = new FileRepository(UnitOfWork).Load(fileID);
                UnitOfWork.MarkAsDeleted(CurrentSelfPublisherFile);
                UnitOfWork.MarkAsDeleted(CurrentFile);

                var filePath = Path.Combine(Config.FileUpload.UploadPath, CurrentFile.PathName);
                string fileName = CurrentFile.FileName;
                string prefix = string.Empty;
                fileName = string.Format("{0}", fileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            UnitOfWork.Commit();
            gvFiles.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }

        #endregion

        #region [ DataSource Events ]


        #endregion

        #region [ GridView Events ]


        #endregion

        protected void odsFiles_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<FileSelfPublisher> criteria = new Criteria<FileSelfPublisher>();

            criteria.Expression =
                criteria.Expression.Where(x => x.SupplierID, Entity.ID)
                                    .Where(x => x.IsActive, true);

            var selectedPhaseID = cmbPhaseID.GetSelectedInteger();
            if (selectedPhaseID.HasValue)
            {
                criteria.Expression = criteria.Expression.Where(x => x.PhaseID, selectedPhaseID);
            }
            criteria.Include(x => x.File);

            e.InputParameters["criteria"] = criteria;
        }

        protected void gvFiles_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();

            if (action == "refresh")
            {
                gvFiles.DataBind();
                return;
            }
        }

        protected void cmbPhaseID_Init(object sender, EventArgs e)
        {
            cmbPhaseID.FillPhases(true);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvFiles.DataBind();
        }
    }
}
