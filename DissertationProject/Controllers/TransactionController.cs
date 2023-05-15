using DissertationProject.Data;
using DissertationProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DissertationProject.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<CustomUserModel> _userManager;
        private readonly ApplicationDbContext _db;

        public TransactionController(RoleManager<IdentityRole> roleManager, UserManager<CustomUserModel> userManager, ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        public async Task<IActionResult> ViewAll()
        {
            //Need to return a list of all transactions that are linked to the current family id of the user that is logged in.
            var TMList = new List<TransactionUserViewModel>();
            CustomUserModel user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var transactions = _db.FamilyTransactions.Where(i => i.FamilyId == user.FamilyId).ToList();
                foreach (var transaction in transactions)
                {
                    var TM = new TransactionUserViewModel()
                    {
                        Transaction = transaction,
                        //Find the member that made the transaction by the transaction userid.
                        Member = await _userManager.FindByIdAsync(transaction.UserId)
                    };
                    TMList.Add(TM);
                }
            }
            return View(TMList);
        }

        public IActionResult AddTransaction()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTransaction(FamilyTransactionModel transaction)
        {
            CustomUserModel user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                if (user.FamilyId != null)
                {
                    var familyId = (int)user.FamilyId;

                    transaction.FamilyId = familyId;
                    transaction.Date = DateTime.Now;
                    transaction.UserId = user.Id;
                    transaction.CustomUser = user;
                    _db.FamilyTransactions.Add(transaction);
                    _db.SaveChanges();
                }
                else
                {
                    TempData["Danger"] = "You need to be a part of a family first! - Please do this in the settings.";
                    return RedirectToAction("ViewAll");
                }
            }
            return RedirectToAction("ViewAll");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveMonth()
        {
            //Need to remove the past month from the transactions in the database.
            var date = DateTime.Now;
            var month = date.Month;
            var year = date.Year;
            CustomUserModel user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var familyId = user.FamilyId;
                var transaction = _db.FamilyTransactions.Where(i => i.FamilyId == familyId && i.Date.Month < month).ToList();
                if (transaction != null)
                {
                    _db.FamilyTransactions.RemoveRange(transaction);
                    _db.SaveChanges();
                }
            }
            TempData["Success"] = "Past Month Deleted!";
            return RedirectToAction("ViewAll");
        }

        [HttpPost]
        public IActionResult DeleteTransaction(int id)
        {
            //Need to delete a transaction from the database.
            var transaction = _db.FamilyTransactions.Where(i => i.Id == id).FirstOrDefault();
            if (transaction != null)
            {
                _db.FamilyTransactions.Remove(transaction);
                _db.SaveChanges();
            }
            TempData["Success"] = "Transaction Deleted!";
            return RedirectToAction("ViewAll");
        }
    }
}