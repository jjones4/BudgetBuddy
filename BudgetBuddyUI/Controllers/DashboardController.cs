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

            if (usersBudgetNames.Count == 0)
            {
                return View();
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
            // Figure out who is logged in, and get the AspNetUserId for identifying the user
            // in the BudgetDataDb
            var aspNetUserId = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(User));

            SqlDataTranslator sqlDataTranslator = new SqlDataTranslator();

            // Get the logged in user's Id from the BudgetDataDb
            int loggedInUserId = await sqlDataTranslator.GetUserIdByAspNetUserId(aspNetUserId,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            // Once the user has entered the name of their new budget, we need to check two things:
            // 1. If the user already has a budget with the same name (not allowed)
            // 2. If the new budget name already exists in the BudgetNames table (we don't need to add it)
            //    We just need to know the Id of the budget name that already exists.

            List<BudgetNameModel> userBudgetNames = await sqlDataTranslator.GetBudgetNamesByLoggedInUserId(
                loggedInUserId,
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
            //    We just need to know the Id of the budget name that already exists so we can add the Id
            //    to the UsersBudgetNames table
            List<BudgetNameModel> allBudgetNames = await sqlDataTranslator.GetAllBudgetNames(
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            foreach (BudgetNameModel budget in allBudgetNames)
            {
                // If the user tries to add a new budget with a name that already exists in the BudgetNames
                // table, we just need to get the Id of that budget and add a new row to the
                // UsersBudgetNames table
                if (budget.BudgetName == dashboardBudgetsTableModel.BudgetName)
                {
                    // If the user is trying to set this new budget as their default budget,
                    // we need to remove the default value from the other budget and set this
                    // one as the default
                    if (dashboardBudgetsTableModel.IsDefaultBudget == true)
                    {
                        await sqlDataTranslator.ClearDefaultBudgetFlagsByUserId(
                            loggedInUserId,
                            _config.GetConnectionString("BudgetDataDbConnectionString"));
                    }

                    await sqlDataTranslator.AddNewBudgetToUsersBudgetNamesTable(
                        loggedInUserId,
                        budget.Id,
                        dashboardBudgetsTableModel.IsDefaultBudget,
                        dashboardBudgetsTableModel.Threshhold,
                        _config.GetConnectionString("BudgetDataDbConnectionString"));

                    return RedirectToAction("Index");
                }
            }

            // If we reached this point and have not been redirected to the budget dashboard,
            // then the user has entered a brand new budget name that does not exist in the BudgetNames table
            // We need to:
            // 1. Add a new entry to the BudgetNames table for the new budget name
            // 2. Get the Id of the new budget name
            // 3. Add a new entry to the UsersBudgetNames table for the new budget
            // 4. Check if the user has selected this new budget as the default budget,
            // 5. Set the IsDefault flag to false for the old budget from the
            //    UsersBudgetNames table
            await sqlDataTranslator.AddNewBudgetNameToBudgetNamesTable(
                dashboardBudgetsTableModel.BudgetName,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            // Get the Id of the newly added budget name
            List<BudgetNameModel> budgetNames = await sqlDataTranslator.GetIdByBudgetName(
                dashboardBudgetsTableModel.BudgetName,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            int newBudgetId = budgetNames.First().Id;

            // If the user is trying to set this new budget as their default budget,
            // we need to remove the default value from the other budget and set this
            // one as the default
            if (dashboardBudgetsTableModel.IsDefaultBudget == true)
            {
                await sqlDataTranslator.ClearDefaultBudgetFlagsByUserId(
                    dashboardBudgetsTableModel.UserId,
                    _config.GetConnectionString("BudgetDataDbConnectionString"));
            }

            await sqlDataTranslator.AddNewBudgetToUsersBudgetNamesTable(
                dashboardBudgetsTableModel.UserId,
                newBudgetId,
                dashboardBudgetsTableModel.IsDefaultBudget,
                dashboardBudgetsTableModel.Threshhold,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditBudgetName(
            int budgetId,
            int userId,
            string budgetName,
            decimal threshhold)
        {
            SqlDataTranslator sqlDataTranslator = new SqlDataTranslator();

            List<UsersBudgetNamesModel> usersBudgetNamesModels = await
                sqlDataTranslator.GetUsersBudgetNamesRowsByUserId(
                    userId,
                    _config.GetConnectionString("BudgetDataDbConnectionString"));

            bool isDefaultBudget = usersBudgetNamesModels.Where(x => x.Id == budgetId).First().IsDefaultBudget;

            DashboardBudgetsTableModel dashboardBurdgetsTableModel = new DashboardBudgetsTableModel()
            {
                Id = budgetId,
                UserId = userId,
                BudgetName = budgetName,
                IsDefaultBudget = isDefaultBudget,
                Threshhold = threshhold
            };

            return View(dashboardBurdgetsTableModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditBudgetName(int budgetId, int userId, DashboardBudgetsTableModel dashboardBudgetsTableModel)
        {
            // Once the user has posted an edit to their budget dashboard, we need to check two things:
            // 1. If the user already has a budget with the same name, then we don't need to change it
            // 2. If the posted budget name already exists in the BudgetNames table (we don't need to add it)
            //    We just need to know the Id of the budget name that already exists.

            SqlDataTranslator sqlDataTranslator = new SqlDataTranslator();

            List<BudgetNameModel> userBudgetNames = await sqlDataTranslator.GetBudgetNamesByLoggedInUserId(
                dashboardBudgetsTableModel.UserId,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            // 2. If the new budget name already exists in the BudgetNames table (we don't need to add it)
            //    We just need to know the Id of the budget name that already exists so we can add the Id
            //    to the UsersBudgetNames table
            List<BudgetNameModel> allBudgetNames = await sqlDataTranslator.GetAllBudgetNames(
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            foreach (BudgetNameModel budget in allBudgetNames)
            {
                // If the user tries to add a new budget with a name that already exists in the BudgetNames
                // table, we just need to get the Id of that budget and add a new row to the
                // UsersBudgetNames table
                if (budget.BudgetName == dashboardBudgetsTableModel.BudgetName)
                {
                    // If the user is trying to set this new budget as their default budget,
                    // we need to remove the default value from the other budget and set this
                    // one as the default
                    if (dashboardBudgetsTableModel.IsDefaultBudget == true)
                    {
                        await sqlDataTranslator.ClearDefaultBudgetFlagsByUserId(
                            userId,
                            _config.GetConnectionString("BudgetDataDbConnectionString"));
                    }

                    // If we find the name of the budget in the budget names table,
                    // we need its Id
                    await sqlDataTranslator.UpdateUsersBudgetNameById(
                        budgetId,
                        budget.Id,
                        dashboardBudgetsTableModel.IsDefaultBudget,
                        dashboardBudgetsTableModel.Threshhold,
                        _config.GetConnectionString("BudgetDataDbConnectionString"));

                    return RedirectToAction("Index");
                }
            }

            // If we reached this point and have not been redirected to the budget dashboard,
            // then the user has entered a brand new budget name that does not exist in the BudgetNames table
            // We need to:
            // 1. Add a new entry to the BudgetNames table for the new budget name
            // 2. Get the Id of the new budget name
            // 3. Add a new entry to the UsersBudgetNames table for the new budget
            // 4. Check if the user has selected this new budget as the default budget,
            // 5. Set the IsDefault flag to false for the old budget from the
            //    UsersBudgetNames table
            await sqlDataTranslator.AddNewBudgetNameToBudgetNamesTable(
                dashboardBudgetsTableModel.BudgetName,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            // Get the Id of the newly added budget name
            List<BudgetNameModel> budgetNames = await sqlDataTranslator.GetIdByBudgetName(
                dashboardBudgetsTableModel.BudgetName,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            int newBudgetId = budgetNames.First().Id;

            // If the user is trying to set this new budget as their default budget,
            // we need to remove the default value from the other budget and set this
            // one as the default
            if (dashboardBudgetsTableModel.IsDefaultBudget == true)
            {
                await sqlDataTranslator.ClearDefaultBudgetFlagsByUserId(
                    userId,
                    _config.GetConnectionString("BudgetDataDbConnectionString"));
            }

            await sqlDataTranslator.UpdateUsersBudgetNameById(
                budgetId,
                newBudgetId,
                dashboardBudgetsTableModel.IsDefaultBudget,
                dashboardBudgetsTableModel.Threshhold,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int budgetId)
        {
            SqlDataTranslator sqlDataTranslator = new SqlDataTranslator();

            await sqlDataTranslator.DeleteBudgetById(
                budgetId,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            return RedirectToAction("Index");
        }
    }
}
