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
        public IActionResult Create(BudgetModel budget)
        {
            if (ModelState.IsValid == false)
            {
                return View();
            }

            BudgetModel tempBudget = budget;

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

            return View(budget);
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
    }
}
