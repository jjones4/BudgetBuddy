using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddyLibrary.Models
{
    public class UsersBudgetNamesModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int BudgetNameId { get; set; }

        public bool IsDefaultBudget { get; set; }

        public decimal? Threshhold { get; set; }
    }
}
