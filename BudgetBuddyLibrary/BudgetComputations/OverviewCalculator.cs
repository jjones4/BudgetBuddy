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

        public static List<HighlightModel> SumsByDescriptionOfExpenditure(List<LineItemModel> input)
        {
            List<HighlightModel> highlights = new List<HighlightModel>();
            List<string> repeatedDescriptions = new List<string>();
            bool itemEncounteredAlready = false;

            foreach (var item in input)
            {
                // Description has already been encountered
                foreach(var description in repeatedDescriptions)
                {
                    if (item.DescriptionOfTransaction == description)
                    {
                        itemEncounteredAlready = true;
                        break;
                    }
                }

                if (itemEncounteredAlready)
                {
                    itemEncounteredAlready = false;

                    continue;
                }
                else
                {
                    var appearsMoreThanOnce = input
                    .Where(x => x.DescriptionOfTransaction == item.DescriptionOfTransaction)
                    .Skip(1)
                    .Any();

                    if (appearsMoreThanOnce)
                    {
                        repeatedDescriptions.Add(item.DescriptionOfTransaction);

                        // Sum all transactions for the given description
                        decimal total = input.Where(x => x.DescriptionOfTransaction == item.DescriptionOfTransaction)
                            .Select(x => x.AmountOfTransaction)
                            .Sum();

                        highlights.Add(new HighlightModel()
                        {
                            NameOfExpenditure = item.DescriptionOfTransaction,
                            AmountOfExpenditure = total
                        });
                    }
                }
            }

            return highlights;
        }
    }
}
