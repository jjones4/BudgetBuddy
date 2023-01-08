﻿namespace BudgetBuddyUI.Models
{
    public class MonthlySummaryModel
    {
        public string MonthName { get; set; } = string.Empty;

        public decimal IncomeAmount { get; set; }

        public decimal ExpenseAmaount { get; set; }

        public decimal MarginAmount { get; set; }
    }
}