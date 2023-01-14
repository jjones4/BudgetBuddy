using BudgetBuddyLibrary.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BudgetBuddyUI.Models
{
    public class BudgetViewModel
    {
        public string BudgetName { get; set; } = string.Empty;

        [DisplayName("Budgets")]
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> BudgetNames { get; set; } = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

        public List<LineItemModel> Transactions { get; set; } = new List<LineItemModel>();
    }
}
