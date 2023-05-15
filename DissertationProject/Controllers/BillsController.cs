using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DissertationProject.Data;
using DissertationProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DissertationProject.Controllers
{
    [Authorize]
    public class BillsController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<CustomUserModel> _userManager;
        private readonly ApplicationDbContext _db;

        public BillsController(RoleManager<IdentityRole> roleManager, UserManager<CustomUserModel> userManager, ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        public async Task<IActionResult> ViewAll()
        {
            //Needs to return a list of all the bills linked to the current logged in users family id.
            var bills = new List<FamilyBillModel>();
            CustomUserModel user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var billsList = _db.FamilyBills.Where(i => i.FamilyId == user.FamilyId).ToList();
                bills = billsList;
            }

            return View(bills);
        }


        public IActionResult AddBill()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddBill(FamilyBillModel model)
        {
            //Need to create a method that adds a bill to the database.
            //Need to add the familymodel
            //Need to workout the time remaining left to pay based on when it is due and the current date.
            CustomUserModel user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                if (user.FamilyId != null)
                {
                    var familyId = (int)user.FamilyId;
                    var date = DateTime.Now;
                    var day = date.Day;
                    model.FamilyId = familyId;

                    _db.FamilyBills.Add(model);
                    _db.SaveChanges();
                }
                else
                {
                    TempData["Danger"] = "You need to be a part of a family first! - Please do this in the settings.";
                    return RedirectToAction("ViewAll");
                }
            }
            TempData["Success"] = "Bill successfully added!";
            return RedirectToAction("ViewAll");
        }

        [HttpPost]
        public IActionResult DeleteBill(int id)
        {
            //Need to delete a bill from the database.
            var bill = _db.FamilyBills.Where(i => i.Id == id).FirstOrDefault();
            if (bill != null)
            {
                _db.FamilyBills.Remove(bill);
                _db.SaveChanges();
            }
            TempData["Success"] = "Bill Deleted!";
            return RedirectToAction("ViewAll");
        }
    }
}