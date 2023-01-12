using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BudgetBuddyLibrary.BudgetComputations
{
    public class HighlightModel
    {
        [DisplayName("Transaction Description")]
        public string NameOfExpenditure { get; set; } = string.Empty;

        [DisplayName("Amount")]
        [DataType(DataType.Currency)]
        public decimal AmountOfExpenditure { get; set; }
    }
}
