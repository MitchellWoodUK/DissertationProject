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
    }
}
