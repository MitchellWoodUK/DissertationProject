using DissertationProject.Data;
using DissertationProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
            var transactionList = new List<FamilyTransactionModel>();
            CustomUserModel user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var transaction =  _db.FamilyTransactions.Where(i => i.FamilyId == user.FamilyId).ToList();
                transactionList = transaction;
            }
            return View(transactionList);
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
                var familyId = (int)user.FamilyId;
                transaction.FamilyId = familyId;
                transaction.Date = DateTime.Now;
                _db.FamilyTransactions.Add(transaction);
                _db.SaveChanges();
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
