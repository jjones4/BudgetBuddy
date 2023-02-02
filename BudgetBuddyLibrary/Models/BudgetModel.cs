using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BudgetBuddyLibrary.Models
{
    public class BudgetModel
    {
        public int BudgetId { get; set; }

        public string BudgetName { get; set; } = string.Empty;

        public LineItemModel Transaction { get; set; } = new LineItemModel();
    }
}
