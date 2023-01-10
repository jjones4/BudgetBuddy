using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddyLibrary.Models
{
    public class PartialOverviewLineModel
    {
        public int Month { get; set; }

        public int YearOfTransaction { get; set; }

        public decimal AmountOfTransactions { get; set; }
    }
}
