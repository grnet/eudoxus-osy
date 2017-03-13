using DevExpress.Web;
using EudoxusOsy.Portal.Controls;
using System;

namespace EudoxusOsy.Portal.UserControls.GridViews
{
    public partial class BookPriceChangesGridView : BaseGridViewUserControl
    {
        #region [ Properties ]

        public override ASPxGridView Grid
        {
            get { return gvBookPriceChanges; }
        }

        #endregion

        protected void Page_Load(object source, EventArgs e)
        {
        }

        #region [ GridView Events ]

        #endregion
    }
}