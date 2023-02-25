using BudgetBuddyLibrary.Models;

namespace BudgetBuddyUI.Models
{
    public class EditViewModel
    {
        public string BudgetName { get; set; } = string.Empty;

        public LineItemModel Transaction { get; set; } = new LineItemModel();
    }
}
