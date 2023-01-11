using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetBuddyUI.Models
{
    public class MonthlySummaryModel
    {
        [DisplayName("Month")]
        public string MonthName { get; set; } = string.Empty;

        [DisplayName("Year")]
        public int YearOfTransaction { get; set; }

        [DisplayName("Income")]
        [DataType(DataType.Currency)]
        public decimal IncomeAmount { get; set; }

        [DisplayName("Expense")]
        [DataType(DataType.Currency)]
        public decimal ExpenseAmaount { get; set; }

        [DisplayName("Margin")]
        [DataType(DataType.Currency)]
        public decimal MarginAmount { get; set; }
    }
}
