using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BudgetBuddyUI.Models
{
    public class DashboardTemplatesTableModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [DisplayName("Template Name")]
        [RegularExpression(@"^[a-zA-Z0-9""'\s-]*$")]
        [StringLength(255, MinimumLength = 2)]
        [Required]
        public string TemplateName { get; set; } = string.Empty;
    }
}
