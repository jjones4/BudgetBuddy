using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BudgetBuddyUI.Models
{
    public class DashboardBudgetsTableModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [DisplayName("Budget Name")]
        [RegularExpression(@"^[a-zA-Z0-9""'\s-]*$")]
        [StringLength(255, MinimumLength = 2)]
        [Required]
        public string BudgetName { get; set; } = string.Empty;

        [DisplayName("Default Budget")]
        public bool IsDefaultBudget { get; set; }

        [Range(0, 999999999999.99)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Threshhold { get; set; }
    }
}
