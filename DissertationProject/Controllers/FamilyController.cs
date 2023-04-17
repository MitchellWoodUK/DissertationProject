using System;
using DissertationProject.Data;
using DissertationProject.Migrations;
using DissertationProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DissertationProject.Controllers
{
    [Authorize]
    public class FamilyController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<CustomUserModel> _userManager;
        private readonly ApplicationDbContext _db;

        public FamilyController(RoleManager<IdentityRole> roleManager, UserManager<CustomUserModel> userManager, ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }


        public async Task<IActionResult> Manage()
        {
            //Need to return a list of all users that are linked to the current family id of the user that is logged in.
            var FMlist = new List<UserRolesViewModel>();
            CustomUserModel user = await _userManager.GetUserAsync(User);

            var users = await _userManager.Users.ToListAsync();

            if (user != null)
            {

                foreach (var item in users)
                {
                    if (item.FamilyId == user.FamilyId)
                    {
                        //Check for any family members that have the same family id and add them to the list.
                        //Find the family members model using the current iteration of user.
                        var family = await _db.FamilyMembers.FirstOrDefaultAsync(i => i.FamilyMember.Email == item.Email);

                        var accountDetails = new UserRolesViewModel()
                        {
                            FamilyMembers = family,
                            Roles = new List<string>(await _userManager.GetRolesAsync(family.FamilyMember)),
                        };
                        FMlist.Add(accountDetails);
                    }
                }
            }
            //Return the list to the view.
            return View(FMlist);
        }

        public IActionResult AddRole()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddRole()
        {

        }





    }
}

