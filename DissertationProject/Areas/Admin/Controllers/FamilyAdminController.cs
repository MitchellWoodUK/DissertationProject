using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DissertationProject.Areas.Admin.Models;
using DissertationProject.Data;
using DissertationProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DissertationProject.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class FamilyAdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<CustomUserModel> _userManager;
        private readonly ApplicationDbContext _db;

        public FamilyAdminController(RoleManager<IdentityRole> roleManager, UserManager<CustomUserModel> userManager, ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        public IActionResult Manage()
        {
            // Create an empty list to store FamilyAdminViewModel objects
            var FMlist = new List<FamilyAdminViewModel>();
            // Get all families from the database
            var families = _db.Families.ToList();

            foreach (var family in families)
            {
                // Get all family members associated with the current family
                var familyMembers = _db.FamilyMembers.Where(i => i.Family.Id == family.Id).ToList();

                // Create a new FamilyAdminViewModel object
                var familyViewModel = new FamilyAdminViewModel
                {
                    Family = family,
                    FamilyMembers = familyMembers
                };

                // Add the new FamilyAdminViewModel object to the FMlist
                FMlist.Add(familyViewModel);
            }
            return View(FMlist);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFamily(int id)
        {
            var family = await _db.Families.FindAsync(id);
            if (family == null)
            {
                // return an error message if the family is not found
                TempData["Danger"] = "Family not found.";
            }
            else
            {
                // remove the family from the database
                _db.Families.Remove(family);
                await _db.SaveChangesAsync();

                // return a success message
                TempData["Success"] = "Family removed successfully.";
            }

            // redirect back to the Manage page
            return RedirectToAction("Manage");
        }
    }
}