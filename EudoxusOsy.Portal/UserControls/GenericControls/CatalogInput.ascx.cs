using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;

namespace EudoxusOsy.Portal.UserControls.GenericControls
{
    public partial class CatalogInput : BaseEntityUserControl<NewCatalogInfo>
    {
        #region [ Control Inits ]

        protected void ddlPhase_Init(object sender, EventArgs e)
        {
            ddlPhase.FillPhasesAllowedForManualCatalogCreation(true);
        }

        #endregion

        #region [ Extract - Bind ]

        public override void Bind()
        {
            ddlPhase.SelectedItem = ddlPhase.Items.FindByValue(Entity.PhaseID);
            txtBookKpsID.Value = Entity.BookKpsID;
            txtSupplierKpsID.Value = Entity.SupplierKpsID;
            txtSecretaryKpsID.Value = Entity.SecretaryKpsID;
            txtBookCount.Value = Entity.BookCount;
        }

        public override NewCatalogInfo Fill(NewCatalogInfo entity)
        {
            entity.PhaseID = ddlPhase.GetSelectedInteger().Value;
            entity.BookKpsID = txtBookKpsID.GetInteger().Value;
            entity.SupplierKpsID = txtSupplierKpsID.GetInteger().Value;
            entity.SecretaryKpsID = txtSecretaryKpsID.GetInteger().Value;
            entity.BookCount = txtBookCount.GetInteger().Value;

            return entity;
        }

        #endregion

        #region [ Validation ]

        public string ValidationGroup
        {
            get { return txtBookKpsID.ValidationSettings.ValidationGroup; }
            set
            {
                foreach (var control in this.RecursiveOfType<ASPxEdit>())
                    control.ValidationSettings.ValidationGroup = value;
            }
        }

        #endregion

        #region [ Properties ]

        public bool ReadOnly
        {
            get { return txtBookKpsID.ReadOnly; }
            set
            {
                foreach (var control in this.RecursiveOfType<ASPxEdit>())
                {
                    control.ReadOnly = value;
                    control.ClientEnabled = !value;
                }
            }
        }

        #endregion
    }
}