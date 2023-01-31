using BudgetBuddyLibrary;
using BudgetBuddyLibrary.BudgetComputations;
using BudgetBuddyLibrary.Models;
using BudgetBuddyUI.Areas.Identity.Data;
using BudgetBuddyUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
            int loggedInUserId = await sqlDataTranslator.GetUserIdByAspNetUserId(aspNetUserId,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            // Now, based on the logged in user Id, we need to get the default
            // budget, if any, from the UsersBudgetNames table
            List<UsersBudgetNamesModel> usersBudgetNames =
                await sqlDataTranslator.GetUsersBudgetNamesRowsByUserId(loggedInUserId,
                _config.GetConnectionString("BudgetDataDbConnectionString"));

            int defaultBudgetId = DefaultBudget.GetDefaultBudgetId(usersBudgetNames);
            
            // If there is a default (or first) budget, get the line items
            // If there is not a default (or first) budget, don't go to the database,
            // and also leave the list empty so we can check this later
            if (defaultBudgetId > 0)
            {
                List<LineItemModel> defaultLineItems;

                defaultLineItems =
                    await sqlDataTranslator.GetLineItemsByUserBudgetId(defaultBudgetId,
                    _config.GetConnectionString("BudgetDataDbConnectionString"));

                List<LineItemModel> creditLineItems = 
                    defaultLineItems.Where(x => x.IsCredit == true).ToList();
                List<PartialOverviewLineModel> creditPartialOverview = 
                    OverviewCalculator.SumsByMonth(creditLineItems);

                List<LineItemModel> debitLineItems = 
                    defaultLineItems.Where(x => x.IsCredit == false).ToList();
                List<PartialOverviewLineModel> debitPartialOverview = 
                    OverviewCalculator.SumsByMonth(debitLineItems);

                // Get the unique month/year date combinations found in user's budget
                List<DateTime> uniqueMonthsYears =
                    defaultLineItems.Select(d => new DateTime(d.DateOfTransaction.Year, d.DateOfTransaction.Month, 1))
                    .Distinct()
                    .ToList();

                List<MonthlySummaryModel> monthlySummaries = new List<MonthlySummaryModel>();

                foreach (DateTime monthYear in uniqueMonthsYears)
                {
                    MonthlySummaryModel tempMonthlySummary = new MonthlySummaryModel();

                    tempMonthlySummary.MonthName = monthYear.ToString( "MMMM" );
                    tempMonthlySummary.YearOfTransaction = monthYear.Year;

                    if (creditPartialOverview
                        .Where(x => x.Month == monthYear.Month && x.YearOfTransaction == monthYear.Year)
                        .FirstOrDefault() == null)
                    {
                        tempMonthlySummary.IncomeAmount = 0;
                    }
                    else
                    {
                        tempMonthlySummary.IncomeAmount = 
                            creditPartialOverview
                            .Where(x => x.Month == monthYear.Month && x.YearOfTransaction == monthYear.Year)
                            .FirstOrDefault()
                            .AmountOfTransactions;
                    }

                    if (debitPartialOverview
                        .Where(x => x.Month == monthYear.Month && x.YearOfTransaction == monthYear.Year)
                        .FirstOrDefault() == null)
                    {
                        tempMonthlySummary.ExpenseAmaount = 0;
                    }
                    else
                    {
                        tempMonthlySummary.ExpenseAmaount =
                            debitPartialOverview.Where(x => x.Month == monthYear.Month && x.YearOfTransaction == monthYear.Year)
                            .FirstOrDefault()
                            .AmountOfTransactions;
                    }

                    tempMonthlySummary.MarginAmount 
                        = tempMonthlySummary.IncomeAmount - tempMonthlySummary.ExpenseAmaount;

                    monthlySummaries.Add(tempMonthlySummary);
                }

                // Create a new list to hold the values after inflation is calculated
                List<MonthlySummaryModel> newMonthlySummaries = new List<MonthlySummaryModel>();
                foreach (var item in monthlySummaries)
                {
                    newMonthlySummaries.Add(new MonthlySummaryModel()
                    {
                        MonthName = item.MonthName,
                        YearOfTransaction = item.YearOfTransaction,
                        IncomeAmount = item.IncomeAmount,
                        ExpenseAmaount = item.ExpenseAmaount,
                        MarginAmount = item.MarginAmount
                    });
                }

                // Multiply the values in monthly summaries by an amount of inflation (8%)
                foreach (var item in newMonthlySummaries)
                {
                    item.IncomeAmount += item.IncomeAmount * 0.08M;
                    item.ExpenseAmaount += item.ExpenseAmaount * 0.08M;
                    item.MarginAmount += item.MarginAmount * 0.08M;
                }

                OverviewModel overviewModel = new OverviewModel(monthlySummaries);
                OverviewModel projectionModel = new OverviewModel(newMonthlySummaries);

                // Now we will calculate the highlights (sum of amount spent on any item
                // that appears more than one time in the user's budget)
                overviewModel.Highlights = 
                    OverviewCalculator.SumsByDescriptionOfExpenditure(defaultLineItems);

                return View(new List<OverviewModel>()
                {
                    overviewModel,
                    projectionModel
                });
            }

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