using System.Text;
using System;
using System.Linq;
using System.Collections.Generic;
using EudoxusOsy.BusinessModel;
using DevExpress.Web;
using System.Web;
using System.Web.Security;

namespace EudoxusOsy.Portal
{
    public static class GridViewExtensions
    {
        #region [ Reporter Extensions ]

        public static string GetReporterTypeDetails(this Reporter reporter)
        {
            if (reporter == null)
                return string.Empty;

            string reporterTypeDetails = string.Empty;
            
            reporterTypeDetails = string.Format("{0}", reporter.ReporterType.GetLabel());
            

            return reporterTypeDetails;
        }

        public static string GetCreationDate(this Reporter reporter)
        {
            if (reporter == null)
                return string.Empty;

            return string.Format("{0:dd/MM/yyyy HH:mm}", reporter.CreatedAt);
        }

        public static string GetContactDetails(this Reporter reporter)
        {
            if (reporter == null)
                return string.Empty;

            List<string> contactDetails = new List<string>();

            contactDetails.Add(reporter.ContactName);
            //contactDetails.Add(reporter.ContactPhone);

            contactDetails.Add(reporter.Username);            
            contactDetails.Add(reporter.Email);

            contactDetails.RemoveAll(x => x == null);

            return string.Join("<br/>", contactDetails);
        }

        public static string GetAccountDetails(this Reporter reporter)
        {
            if (reporter == null)
                return string.Empty;

            return string.Join("<br/>", reporter.Username, reporter.Email);
        }

        #endregion

        #region [ Helper Methods ]

        public static List<int> GetSelectedIds(this ASPxGridView grid)
        {
            return grid.GetSelectedFieldValues("ID").OfType<int>().ToList();
        }

        #endregion
    }
}