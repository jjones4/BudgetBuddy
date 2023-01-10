using BudgetBuddyLibrary.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddyLibrary.BudgetComputations
{
    public static class DefaultBudget
    {
        public static int GetDefaultBudgetId(List<UsersBudgetNamesModel> usersBudgetNames)
        {
            bool foundDefaultBudget = false;
            int id = 0;

            if (usersBudgetNames.IsNullOrEmpty())
            {
                return 0;
            }
            else
            {
                foreach (UsersBudgetNamesModel budget in usersBudgetNames)
                {
                    // Means we found a default budget
                    if (budget.IsDefaultBudget == true)
                    {
                        foundDefaultBudget = true;

                        id = budget.Id;

                        break;
                    }
                }

                // No default budget was found
                // We will set first budget in List to default
                if (foundDefaultBudget == false)
                {
                    id = usersBudgetNames.First().Id;
                }

                return id;
            }
        }
    }
}
