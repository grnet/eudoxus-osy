using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using DevExpress.Web;
using EudoxusOsy.Portal.Utils;
using Imis.Domain;

namespace EudoxusOsy.Portal.UserControls.GenericControls
{
    public partial class DateRangeInput : UserControl
    {
        #region [ Extract - Bind ]

        public List<DateTime?> GetDates()
        {
            List<DateTime?> dates = new List<DateTime?>();

            dates.Add(txtDateFrom.GetDate());
            dates.Add(txtDateTo.GetDate());

            return dates;
        }

        public void Bind(DateTime? dateFrom, DateTime? dateTo)
        {
            txtDateFrom.Value = dateFrom;
            txtDateTo.Value = dateTo;
        }

        #endregion

        #region [ Validation ]

        public bool IsRequired
        {
            get { return txtDateFrom.ValidationSettings.RequiredField.IsRequired; }
            set
            {
                txtDateFrom.ValidationSettings.RequiredField.IsRequired =
                    txtDateTo.ValidationSettings.RequiredField.IsRequired = value;
            }
        }

        public bool ReadOnly
        {
            get { return txtDateFrom.ReadOnly; }
            set
            {
                txtDateFrom.ReadOnly = txtDateTo.ReadOnly = value;
                txtDateFrom.ClientEnabled = txtDateTo.ClientEnabled = !value;
            }
        }

        public string ValidationGroup
        {
            get { return txtDateFrom.ValidationSettings.ValidationGroup; }
            set
            {
                txtDateFrom.ValidationSettings.ValidationGroup = value;
            }
        }

        #endregion
    }
}