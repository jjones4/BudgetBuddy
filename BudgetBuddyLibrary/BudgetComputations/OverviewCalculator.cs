using BudgetBuddyLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddyLibrary.BudgetComputations
{
    public static class OverviewCalculator
    {
        public static List<PartialOverviewLineModel> SumsByMonth(List<LineItemModel> input)
        {
            List<PartialOverviewLineModel> overviewLines = new List<PartialOverviewLineModel>();

            // Get the unique month/year date combinations found in user's budget
            List<DateTime> uniqueMonthsYears =
                input.Select(d => new DateTime(d.DateOfTransaction.Year, d.DateOfTransaction.Month, 1))
                .Distinct()
                .ToList();

            foreach (var item in uniqueMonthsYears)
            {
                // Sume all transactions for the given month/year combination
                decimal total = input.Where(x => x.DateOfTransaction.Month == item.Month 
                    && x.DateOfTransaction.Year == item.Year)
                    .Select(x => x.AmountOfTransaction)
                    .Sum();

                overviewLines.Add(new PartialOverviewLineModel()
                {
                    Month = item.Month,
                    YearOfTransaction = item.Year,
                    AmountOfTransactions = total
                });
            }

            return overviewLines;
        }
    }
}
