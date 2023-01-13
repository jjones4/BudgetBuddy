using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddyLibrary.Models
{
    public class LineItemModel
    {
        public int Id { get; set; }

        public int UserBudgetId { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime DateOfTransaction { get; set; }

        [DisplayName("Amount")]
        [DataType(DataType.Currency)]
        public decimal AmountOfTransaction { get; set; }

        [DisplayName("Description")]
        public string DescriptionOfTransaction { get; set; } = string.Empty;

        [DisplayName("Credit")]
        public bool IsCredit { get; set; } = false;
    }
}
