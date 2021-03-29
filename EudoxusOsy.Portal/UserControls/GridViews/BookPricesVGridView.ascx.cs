using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using EudoxusOsy.Portal.Controls;

namespace EudoxusOsy.Portal.UserControls.GridViews
{
    public partial class BookPricesVGridView : BaseGridViewUserControl
    {
        #region [ Properties ]

        public enum enPriceVerificationMode
        {
            Ministry = 0,
            Unexpected = 1
        }

        public enPriceVerificationMode Mode { get; set; }

        public override ASPxGridView Grid
        {
            get { return gvPriceChangeV; }
        }

        #endregion

        protected void Page_Load(object source, EventArgs e)
        {
            if (Mode == enPriceVerificationMode.Unexpected)
            {
                gvPriceChangeV.Columns["PendingCommitteePriceVerification"].Visible = false;
            }
            else if (Mode == enPriceVerificationMode.Ministry)
            {
                gvPriceChangeV.Columns["UnexpectedPriceVerified"].Visible = false;
            }
        }

        #region [ GridView Events ]

        #endregion
    }
}