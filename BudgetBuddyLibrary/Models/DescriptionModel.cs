using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddyLibrary.Models
{
    public class DescriptionModel
    {
        public int Id { get; set; }

        public string DescriptionOfTransaction { get; set; } = string.Empty;
    }
}
