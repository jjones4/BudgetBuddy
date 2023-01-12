using BudgetBuddyLibrary.BudgetComputations;

namespace BudgetBuddyUI.Models
{
    public class OverviewModel
    {
        private List<MonthlySummaryModel> _monthlySummaries;

        public OverviewModel(List<MonthlySummaryModel> monthlySummaries)
        {
            _monthlySummaries = monthlySummaries;
            MonthlySummaries= _monthlySummaries;

            // Calculate values for the Total and Average lines
            // at the bottom of the table in the UI
            decimal totalIncome = 0, totalExpense = 0, totalMargin = 0;
            decimal averageIncome, averageExpense, averageMargin;
            int counter = 0;

            foreach (var monthlySummary in monthlySummaries)
            {
                totalIncome += monthlySummary.IncomeAmount;
                totalExpense += monthlySummary.ExpenseAmaount;
                totalMargin += monthlySummary.MarginAmount;

                counter++;
            }

            averageIncome = totalIncome / counter;
            averageExpense = totalExpense / counter;
            averageMargin = totalMargin / counter;

            Totals.MonthName = "Total";
            Totals.IncomeAmount = totalIncome;
            Totals.ExpenseAmaount = totalExpense;
            Totals.MarginAmount = totalMargin;

            Averages.MonthName = "Average";
            Averages.IncomeAmount = averageIncome;
            Averages.ExpenseAmaount = averageExpense;
            Averages.MarginAmount = averageMargin;
        }

        public List<MonthlySummaryModel> MonthlySummaries { get; set; } = new List<MonthlySummaryModel>();

        public MonthlySummaryModel Totals { get; private set; } = new MonthlySummaryModel();

        public MonthlySummaryModel Averages { get; private set; } = new MonthlySummaryModel();

        public List<HighlightModel> Highlights { get; set; } = new List<HighlightModel>();
    }
}
