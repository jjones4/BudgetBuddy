using BudgetBuddyLibrary.BudgetComputations;
using BudgetBuddyLibrary.Models;
using BudgetBuddyLibrary;
using BudgetBuddyUI.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using BudgetBuddyUI.Models;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Transactions;

namespace BudgetBuddyUI.Controllers
{
    [Authorize]
    public class BudgetController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly ILogger<HomeController> _logger;

        public BudgetController(UserManager<ApplicationUser> userManager,
            IConfiguration config,
            ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _config = config;
            _logger = logger;
        }

        public IActionResult Create(int budgetId, string budgetName)
        {
            LineItemModel transaction = new LineItemModel();

            BudgetModel budget = new BudgetModel()
            {
                BudgetId = budgetId,
                BudgetName = budgetName,
                Transaction = transaction
            };

            return View(budget);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BudgetModel budget)
        {
            if (ModelState.IsValid == false)
            {
                return View();
            }

            SqlDataTranslator sqlDataTranslator = new SqlDataTranslator();

            // We need the UserBudgetId, DateId, AmountId, DescriptionId, and T/F

            // 1. We already have the UserBudgetId: budget.BudgetId

            // 2. We need to check the date table to see if the date already exists.
            //    If the date already exists, we need to get its Id, otherwise,
            //    we need to add the date to the date table and return the Id
            List<DateModel> dates = await sqlDataTranslator.GetDateRowsByDate(budget.Transaction.DateOfTransaction,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            // If the date is not found, we will add it to the dates table
            // and get the new Id
            if (dates.Count == 0)
            {
                await sqlDataTranslator.AddNewDateToDatesTable(budget.Transaction.DateOfTransaction,
                    _config.GetConnectionString("BudgetDataDbConnectionString"));

                // If we had to add a new date to the dates table, then we need its new Id
                dates = await sqlDataTranslator.GetDateRowsByDate(budget.Transaction.DateOfTransaction,
                _config.GetConnectionString("BudgetDataDbConnectionString"));
            }

            // 3. Same as #2 for AmountId
            List<AmountModel> amounts = await sqlDataTranslator.GetAmountRowsByAmount(budget.Transaction.AmountOfTransaction,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            // If the amount is not found, we will add it to the amounts table
            // and get the new Id
            if (amounts.Count == 0)
            {
                await sqlDataTranslator.AddNewAmountToAmountsTable(budget.Transaction.AmountOfTransaction,
                    _config.GetConnectionString("BudgetDataDbConnectionString"));

                // If we had to add a new amount to our amounts table
                amounts = await sqlDataTranslator.GetAmountRowsByAmount(budget.Transaction.AmountOfTransaction,
                _config.GetConnectionString("BudgetDataDbConnectionString"));
            }

            // 4. Same as #2 and #3 for the DesciptionId
            List<DescriptionModel> descriptions = await sqlDataTranslator.GetDescriptionRowsByDescription(budget.Transaction.DescriptionOfTransaction,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            // If the description is not found, we will add it to the descriptions table
            // and get the new Id
            if (descriptions.Count == 0)
            {
                await sqlDataTranslator.AddNewDescriptionToDescriptionsTable(budget.Transaction.DescriptionOfTransaction,
                    _config.GetConnectionString("BudgetDataDbConnectionString"));

                // If we had to add a new description to our descriptions table
                descriptions = await sqlDataTranslator.GetDescriptionRowsByDescription(budget.Transaction.DescriptionOfTransaction,
                _config.GetConnectionString("BudgetDataDbConnectionString"));
            }

            // 5. We already have T/F coming from budget.IsCredit

            // 6. Insert into the budget table
            await sqlDataTranslator.AddNewLineItemToBudgetsTable(
                budget.BudgetId,
                dates.First().Id,
                amounts.First().Id,
                descriptions.First().Id,
                budget.Transaction.IsCredit,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            return RedirectToAction("Read");
        }

        public async Task<IActionResult> Read()
        {
            // Figure out who is logged in, and get the AspNetUserId for identifying the user
            // in the BudgetDataDb
            var aspNetUserId = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(User));

            SqlDataTranslator sqlDataTranslator = new SqlDataTranslator();

            // Get the logged in user's Id from the BudgetDataDb
            int loggedInUserId = await sqlDataTranslator.GetUserIdByAspNetUserId(aspNetUserId,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            // Now, based on the logged in user Id, we need to get the default
            // budget, if any, from the UsersBudgetNames table
            List<UsersBudgetNamesModel> usersBudgetNames =
                await sqlDataTranslator.GetUsersBudgetNamesRowsByUserId(loggedInUserId,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            int defaultBudgetId = DefaultBudget.GetDefaultBudgetId(usersBudgetNames);

            BudgetViewModel budget = new BudgetViewModel();
            List<BudgetNameModel> budgetNames = await sqlDataTranslator.GetBudgetNamesByLoggedInUserId(loggedInUserId,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            budgetNames.ForEach(x =>
            {
                budget.BudgetNames.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.BudgetName
                });
            });

            // If there is a default (or first) budget, get the line items
            // If there is not a default (or first) budget, don't go to the database,
            // and also leave the list empty so we can check this later
            if (defaultBudgetId > 0)
            {
                List<BudgetNameModel> budgetNameRows =
                    await sqlDataTranslator.GetBudgetNameByDefaultBudgetId(defaultBudgetId,
                    _config.GetConnectionString("BudgetDataDbConnectionString"));

                // 1. Set the budget name
                budget.BudgetId = defaultBudgetId;
                budget.BudgetName = budgetNameRows.First().BudgetName;

                // 2. Populate the budget line items
                List<LineItemModel> defaultLineItems;

                defaultLineItems =
                    await sqlDataTranslator.GetLineItemsByUserBudgetId(defaultBudgetId,
                    _config.GetConnectionString("BudgetDataDbConnectionString"));

                budget.Transactions = defaultLineItems;

                
                return View(budget);
            }

            budget.BudgetName = "No budget was found.";

            if (usersBudgetNames.Count == 0)
            {
                return View();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Read(BudgetViewModel budgetViewModel)
        {
            // Figure out who is logged in, and get the AspNetUserId for identifying the user
            // in the BudgetDataDb
            var aspNetUserId = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(User));

            SqlDataTranslator sqlDataTranslator = new SqlDataTranslator();

            // Get the logged in user's Id from the BudgetDataDb
            int loggedInUserId = await sqlDataTranslator.GetUserIdByAspNetUserId(aspNetUserId,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            int.TryParse(budgetViewModel.BudgetName, out int budgetNameId);

            string budgetName = await sqlDataTranslator.GetBudgetNameById(budgetNameId,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            budgetViewModel.BudgetName = budgetName;

            int userBudgetNameId = await sqlDataTranslator.GetUserBudgetNameIdByLoggedInUserIdAndBudgetNameId(
                loggedInUserId, budgetNameId,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            budgetViewModel.BudgetId = userBudgetNameId;

            List<LineItemModel> budget;

            budget =
                await sqlDataTranslator.GetLineItemsByUserBudgetId(userBudgetNameId,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            budgetViewModel.Transactions = budget;

            List<BudgetNameModel> budgetNames = await sqlDataTranslator.GetBudgetNamesByLoggedInUserId(loggedInUserId,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            budgetNames.ForEach(x =>
            {
                budgetViewModel.BudgetNames.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.BudgetName
                });
            });

            return View(budgetViewModel);
        }

        public async Task<IActionResult> Edit(string budgetName,
            int lineItemId,
            int lineItemBudgetId,
            DateTime lineItemDate,
            Decimal lineItemAmount,
            string lineItemDescription)
        {
            EditViewModel editViewModel = new EditViewModel();

            editViewModel.BudgetName = budgetName;

            LineItemModel lineItem = new LineItemModel();

            lineItem.Id = lineItemId;
            lineItem.UserBudgetId = lineItemBudgetId;
            lineItem.DateOfTransaction = lineItemDate;
            lineItem.AmountOfTransaction = lineItemAmount;
            lineItem.DescriptionOfTransaction = lineItemDescription;

            SqlDataTranslator sqlDataTranslator = new SqlDataTranslator();

            LineItemModel tempLineItem = await sqlDataTranslator.GetIsCreditByLineItemId(
                lineItemId,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            lineItem.IsCredit = tempLineItem.IsCredit;

            editViewModel.Transaction = lineItem;

            return View(editViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int lineItemId, EditViewModel editViewModel)
        {
            SqlDataTranslator sqlDataTranslator = new SqlDataTranslator();

            // We need the lineItemId, DateId, AmountId, DescriptionId, and Credit (T/F)

            // 1. We already have the Id for the line item: lineItemId

            // 2. We need to check the date table to see if the date already exists.
            //    If the date already exists, we need to get its Id, otherwise,
            //    we need to add the date to the date table and return the Id
            List<DateModel> dates = await sqlDataTranslator.GetDateRowsByDate(editViewModel.Transaction.DateOfTransaction,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            // If the date is not found, we will add it to the dates table
            // and get the new Id
            if (dates.Count == 0)
            {
                await sqlDataTranslator.AddNewDateToDatesTable(editViewModel.Transaction.DateOfTransaction,
                    _config.GetConnectionString("BudgetDataDbConnectionString"));

                // If we had to add a new date to the dates table, then we need its new Id
                dates = await sqlDataTranslator.GetDateRowsByDate(editViewModel.Transaction.DateOfTransaction,
                _config.GetConnectionString("BudgetDataDbConnectionString"));
            }

            // 3. Same as #2 for AmountId
            List<AmountModel> amounts = await sqlDataTranslator.GetAmountRowsByAmount(editViewModel.Transaction.AmountOfTransaction,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            // If the amount is not found, we will add it to the amounts table
            // and get the new Id
            if (amounts.Count == 0)
            {
                await sqlDataTranslator.AddNewAmountToAmountsTable(editViewModel.Transaction.AmountOfTransaction,
                    _config.GetConnectionString("BudgetDataDbConnectionString"));

                // If we had to add a new amount to our amounts table
                amounts = await sqlDataTranslator.GetAmountRowsByAmount(editViewModel.Transaction.AmountOfTransaction,
                _config.GetConnectionString("BudgetDataDbConnectionString"));
            }

            // 4. Same as #2 and #3 for the DesciptionId
            List<DescriptionModel> descriptions = await sqlDataTranslator.GetDescriptionRowsByDescription(editViewModel.Transaction.DescriptionOfTransaction,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            // If the description is not found, we will add it to the descriptions table
            // and get the new Id
            if (descriptions.Count == 0)
            {
                await sqlDataTranslator.AddNewDescriptionToDescriptionsTable(editViewModel.Transaction.DescriptionOfTransaction,
                    _config.GetConnectionString("BudgetDataDbConnectionString"));

                // If we had to add a new description to our descriptions table
                descriptions = await sqlDataTranslator.GetDescriptionRowsByDescription(editViewModel.Transaction.DescriptionOfTransaction,
                _config.GetConnectionString("BudgetDataDbConnectionString"));
            }

            // 5. We already have Credit because it is a bit (no need to go to a different table for it)

            // 6. Update the line item in the budget table

            await sqlDataTranslator.UpdateLineItemInBudgetsTable(
                lineItemId,
                dates.First().Id,
                amounts.First().Id,
                descriptions.First().Id,
                editViewModel.Transaction.IsCredit,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            return RedirectToAction("Read");
        }

        public async Task<IActionResult> Delete(int lineItemId)
        {
            SqlDataTranslator sqlDataTranslator = new SqlDataTranslator();

            await sqlDataTranslator.DeleteLineItemById(lineItemId,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            return RedirectToAction("Read");
        }
    }
}
