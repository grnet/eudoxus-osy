using DevExpress.Web;
using EudoxusOsy.Portal.Controls;
using System;

namespace EudoxusOsy.Portal.UserControls.GridViews
{
    public partial class PriceVerificationGridView : BaseGridViewUserControl
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
            get { return gvPriceVerification; }
        }

        #endregion

        protected void Page_Load(object source, EventArgs e)
        {
            if (Mode == enPriceVerificationMode.Unexpected)
            {
                gvPriceVerification.Columns["PendingCommitteePriceVerification"].Visible = false;
            }
            else if (Mode == enPriceVerificationMode.Ministry)
            {
                gvPriceVerification.Columns["UnexpectedPriceVerified"].Visible = false;
            }
        }

        #region [ GridView Events ]

        #endregion

    }
}