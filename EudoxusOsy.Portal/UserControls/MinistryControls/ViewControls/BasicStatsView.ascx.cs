using EudoxusOsy.BusinessModel;
using System;
using System.Linq;

namespace EudoxusOsy.Portal.UserControls.MinistryControls.ViewControls
{
    public partial class BasicStatsView : System.Web.UI.UserControl
    {
        public int PhaseID { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Bind()
        {
            using (var ctx = UnitOfWorkFactory.Create())
            {
                if (PhaseID == 0)
                {
                    PhaseID = new PhaseRepository().GetCurrentPhase().ID;
                }

                var stats = ((DBEntities)ctx).GetBasicStats(PhaseID);
                var stat = stats.FirstOrDefault();
                if (stat != null)
                {
                    lblTotalBooks.Text = stat.totalbooks.ToString();
                    lblAvgBookPrice.Text = stat.avgPricedBooks.HasValue ? stat.avgPricedBooks.Value.ToString("c") : string.Empty;
                    lblTotalCostReceivedBooks.Text = stat.TotalCost.HasValue ? stat.TotalCost.Value.ToString("c") : string.Empty;
                    lblTotalToYDEBooks.Text = stat.TotalToYDE.HasValue ? stat.TotalToYDE.Value.ToString("c") : string.Empty;
                    lblTotalPricedBooks.Text = stat.pricedBooks.ToString();
                }
            }
        }
    }
}