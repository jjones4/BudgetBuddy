namespace BudgetBuddyUI.Models
{
    public class OverviewPageModel
    {
        public decimal InflationPercentage { get; set; }

        public List<OverviewModel> OverviewModels { get; set; } = new List<OverviewModel>();
    }
}
