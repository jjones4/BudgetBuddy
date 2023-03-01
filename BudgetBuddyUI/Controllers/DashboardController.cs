using BudgetBuddyLibrary;
using BudgetBuddyLibrary.Models;
using BudgetBuddyUI.Areas.Identity.Data;
using BudgetBuddyUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddyUI.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly ILogger<HomeController> _logger;

        public DashboardController(UserManager<ApplicationUser> userManager,
            IConfiguration config,
            ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _config = config;
            _logger = logger;
        }

        [Authorize]
        public async Task<IActionResult> Index()
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

            List<DashboardBudgetsTableModel> dashboardBudgetsTableModel = new List<DashboardBudgetsTableModel>();

            foreach (UsersBudgetNamesModel ubnm in usersBudgetNames)
            {
                string budgetName = await sqlDataTranslator.GetBudgetNameById(ubnm.BudgetNameId,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

                dashboardBudgetsTableModel.Add(
                    new DashboardBudgetsTableModel
                    {
                        Id = ubnm.Id,
                        UserId = ubnm.UserId,
                        BudgetName = budgetName,
                        IsDefaultBudget = ubnm.IsDefaultBudget,
                        Threshhold = ubnm.Threshhold
                    });
            }

            return View(dashboardBudgetsTableModel);
        }

        public IActionResult Create(int userId)
        {
            DashboardBudgetsTableModel dashboardBudgetsTableModel = new DashboardBudgetsTableModel()
            {
                UserId = userId
            };

            return View(dashboardBudgetsTableModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DashboardBudgetsTableModel dashboardBudgetsTableModel)
        {
            // Once the user has entered the name of their new budget, we need to check two things:
            // 1. If the user already has a budget with the same name (not allowed)
            // 2. If the new budget name already exists in the BudgetNames table (we don't need to add it)
            //    We just need to know the Id of the budget name that already exists.

            SqlDataTranslator sqlDataTranslator = new SqlDataTranslator();

            List<BudgetNameModel> userBudgetNames = await sqlDataTranslator.GetBudgetNamesByLoggedInUserId(
                dashboardBudgetsTableModel.UserId,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            // 1. If the user already has a budget with the same name (not allowed)
            foreach (BudgetNameModel budget in userBudgetNames)
            {
                // If the user already has a budget with the same name as the one they are trying to create,
                // we won't add it to the database. We just return to the Dashboard.
                if (budget.BudgetName == dashboardBudgetsTableModel.BudgetName)
                {
                    return RedirectToAction("Index");
                }
            }

            // 2. If the new budget name already exists in the BudgetNames table (we don't need to add it)
            //    We just need to know the Id of the budget name that already exists.
            List<BudgetNameModel> allBudgetNames = await sqlDataTranslator.GetAllBudgetNames(
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            foreach (BudgetNameModel budget in allBudgetNames)
            {
                // If the user tries to add a new budget with a name that already exists in the BudgetNames
                // table, we just need to get the Id of that budget and add a new row to the
                // UsersBudgetNames table

                // We also need to check if the user has selected this new budget as the default budget,
                // then we need to remove the old default budget (if it exists) from the
                // UsersBudgetNames table
                if (budget.BudgetName == dashboardBudgetsTableModel.BudgetName)
                {
                    return RedirectToAction("Index");
                }
                // If the user tries to add a new budget that has not been seen by the app before,
                // we need to add a new entry to the BudgetNames table for it, get its new Id,
                // and then add a new entry to the UsersBudgetNames table
                else
                {
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }
    }
}
