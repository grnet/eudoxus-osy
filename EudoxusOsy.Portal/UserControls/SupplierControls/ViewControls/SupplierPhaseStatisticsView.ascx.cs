using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;

namespace EudoxusOsy.Portal.UserControls.SupplierControls.ViewControls
{
    public partial class SupplierPhaseStatisticsView : BaseEntityUserControl<SupplierPhaseStatistics>
    {
        public override void Bind()
        {
            if (Entity == null)
                return;

            spanOwedAmount.InnerText = Entity.OwedAmount.ToString("c");
            spanAllocatedAmount.InnerText = Entity.AllocatedAmount.ToString("c");
            spanRemainingAmount.InnerText = Entity.RemainingAmount.ToString("c");
            spanPaidAmount.InnerText = Entity.PaidAmount.ToString("c");

            if (Entity.RemainingAmount < 0)
            {
                tableStatistics.Attributes["class"] += " red";
            }
            else if (Entity.RemainingAmount == 0)
            {
                tableStatistics.Attributes["class"] = tableStatistics.Attributes["class"].Replace(" red", "");
                tableStatistics.Attributes["class"] += " green";
            }
            else
            {
                tableStatistics.Attributes["class"] = tableStatistics.Attributes["class"].Replace(" red", "");
                tableStatistics.Attributes["class"] = tableStatistics.Attributes["class"].Replace(" green", "");
            }
        }
    }
}