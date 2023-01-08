namespace BudgetBuddyUI.Models
{
    public class OverviewModel
    {
        public List<MonthlySummaryModel> MonthlySummaries { get; set; } = new List<MonthlySummaryModel>();

        public List<HighlightModel> Highlights { get; set; } = new List<HighlightModel>();
    }
}
