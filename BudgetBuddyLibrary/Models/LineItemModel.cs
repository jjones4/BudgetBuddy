using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [DataType(DataType.Date)]
        public DateTime DateOfTransaction { get; set; } = DateTime.Now;

        [DisplayName("Amount")]
        [Range(0, 999999999999.99)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal AmountOfTransaction { get; set; }

        [DisplayName("Description")]
        [RegularExpression(@"^[a-zA-Z0-9""'\s-]*$")]
        [StringLength(255, MinimumLength = 2)]
        [Required]
        public string DescriptionOfTransaction { get; set; } = string.Empty;

        [DisplayName("Credit")]
        public bool IsCredit { get; set; }
    }
}
