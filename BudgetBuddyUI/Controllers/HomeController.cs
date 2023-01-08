using BudgetBuddyLibrary;
using BudgetBuddyLibrary.Models;
using BudgetBuddyUI.Areas.Identity.Data;
using BudgetBuddyUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BudgetBuddyUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly ILogger<HomeController> _logger;

        public HomeController(UserManager<ApplicationUser> userManager,
            IConfiguration config,
            ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _config = config;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            // Figure out who is logged in, and get the AspNetUserId for identifying the user
            // in the BudgetDataDb
            var aspNetUserId = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(User));

            SqlDataTranslator sqlDataTranslator = new SqlDataTranslator();

            // Get the logged in user's Id from the BudgetDataDb
            int loggedInUserId = sqlDataTranslator.GetUserIdByAspNetUserId(aspNetUserId,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            // Now, based on the logged in user Id, we need to get the default
            // budget, if any, from the UsersBudgetNames table
            List<UsersBudgetNamesModel> usersBudgetNames =
                sqlDataTranslator.GetUsersBudgetNamesRowsByUserId(loggedInUserId,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}