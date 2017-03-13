using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EudoxusOsy.Portal
{
    public partial class EmailVerificationInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ltSubject.Text = "Επιβεβαίωση Email Λογαριασμού";

            var type = Request.QueryString["t"];

            if (type == "3")
            {
                ltSubject.Text = "Επιβεβαίωση Email Λογαριασμού Παρόχου/Προμηθευτή";
            }
            else if (type == "4")
            {
                ltSubject.Text = "Επιβεβαίωση Email Λογαριασμού Συμβούλου/Υπαλλήλου";
            }
        }
    }
}