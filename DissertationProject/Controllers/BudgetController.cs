using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DissertationProject.Data;
using DissertationProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DissertationProject.Controllers
{
    [Authorize]
    public class BudgetController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<CustomUserModel> _userManager;
        private readonly ApplicationDbContext _db;

        public BudgetController(RoleManager<IdentityRole> roleManager, UserManager<CustomUserModel> userManager, ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        public async Task<IActionResult> Manage()
        {
            //Need to return a list of the families budget model.
            var BudgetList = new List<BudgetViewModel>();
            CustomUserModel user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var budgets = _db.FamilyBudgets.Where(i => i.FamilyId == user.FamilyId).ToList();
                var transactions = _db.FamilyTransactions.Where(i => i.FamilyId == user.FamilyId).ToList();
                var bills = _db.FamilyBills.Where(i => i.FamilyId == user.FamilyId).ToList();

                float totalTransactions = transactions.Sum(item => item.Amount);
                float totalBills = bills.Sum(item => item.Amount);

                foreach (var budget in budgets)
                {
                    var BM = new BudgetViewModel()
                    {
                        Budget = budget,
                        Expenses = totalTransactions + totalBills,
                        Profit = budget.Budget - (totalTransactions + totalBills)
                    };
                    BudgetList.Add(BM);
                }
            }
            return View(BudgetList);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FamilyBudgetModel model)
        {
            //Need to post a familyBudgetModel.
            CustomUserModel user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                if (user.FamilyId != null)
                {
                    model.FamilyId = (int)user.FamilyId;

                    //Add and save to the database.
                    await _db.FamilyBudgets.AddAsync(model);
                    await _db.SaveChangesAsync();
                    TempData["Success"] = "Budget Created!";
                    return RedirectToAction("Manage");
                }
                else
                {
                    //If the familyId is null.
                    TempData["Danger"] = "You need to create or join a family account first!";
                    return RedirectToAction("Manage");
                }
            }
            return RedirectToAction("Manage");
        }

        [HttpPost]
        public async Task<IActionResult> Update(FamilyBudgetModel model)
        {
            //Need to post a familyBudgetModel.
            CustomUserModel user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                if (user.FamilyId != null)
                {
                    model.FamilyId = (int)user.FamilyId;

                    //Add and save to the database.
                    _db.FamilyBudgets.Update(model);
                    await _db.SaveChangesAsync();
                    TempData["Success"] = "Budget Updated!";
                    return RedirectToAction("Manage");
                }
                else
                {
                    //If the familyId is null.
                    TempData["Danger"] = "You need to create or join a family account first!";
                    return RedirectToAction("Manage");
                }
            }
            TempData["Danger"] = "Error Updating Budget!";
            return RedirectToAction("Manage");
        }
    }
}