using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddyLibrary.Models
{
    public class BudgetModel
    {
        public string BudgetName { get; set; } = string.Empty;

        public List<LineItemModel> Transactions { get; set; } = new List<LineItemModel>();
    }
}
