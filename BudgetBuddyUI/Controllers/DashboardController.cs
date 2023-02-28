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
    }
}
