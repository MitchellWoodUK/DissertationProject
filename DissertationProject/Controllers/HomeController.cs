using DissertationProject.Data;
using DissertationProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;

namespace DissertationProject.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<CustomUserModel> _userManager;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager, UserManager<CustomUserModel> userManager, ApplicationDbContext db)
        {
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        public IActionResult Index()
        {
            //Need to add a function that checks if the user is assigned to a family, if not then they need to create one.

            return View();
        }

        public async Task<IActionResult> Settings()
        {
            //Need to return a list that includes the user information, family information.
            var FMlist = new List<FamilyMembersModel>();
            CustomUserModel user = await _userManager.GetUserAsync(User);
            if (user.FamilyId != null)
            {
                var family = await _db.Families.FirstOrDefaultAsync(i => i.Id == user.FamilyId);
                if (family != null)
                {
                    //Creates a user roles view model and then adds it to the list made earlier.
                    var accountDetails = new FamilyMembersModel()
                    {
                        Family = family,
                        FamilyMember = user
                    };
                    FMlist.Add(accountDetails);
                }
                else
                {
                    //If family is null then return an empty list.
                    return View(FMlist);
                }
            }
            return View(FMlist);

        }


        //Post function to update the institution for the user.
        [HttpPost]
        public async Task<IActionResult> UpdateUser(CustomUserModel model)
        {
            if (model != null)
            {
                //Finds the user that is logged in.
                CustomUserModel user = await _userManager.GetUserAsync(User);
                //Checks if a user has been returned.
                if (user != null)
                {
                    //Update the changed details.
                    //Check if the details have been updated before updating each section.
                    if (model.Fname != null)
                    {
                        user.Fname = model.Fname;
                    }
                    if (model.Sname != null)
                    {
                        user.Sname = model.Sname;
                    }
                    if (model.JobName != null)
                    {
                        user.JobName = model.JobName;
                    }
                    if (model.Income != user.Income)
                    {
                        user.Income = model.Income;
                    }

                    //Updates the database.
                    _db.Users.Update(user);
                    await _db.SaveChangesAsync();
                    TempData["Success"] = "Updated Account!";
                    return RedirectToAction("Settings");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> CreateFamily(FamilyModel model)
        {
            if (model != null)
            {
                //Need to save the FamilyModel
                FamilyModel family = new FamilyModel();
                family.Name = model.Name;

                //Need to check that the family name entered is not already in the database.
                var checkUnique = await _db.Families.FirstOrDefaultAsync(i => i.Name == family.Name);
                if (checkUnique == null)
                {
                    await _db.Families.AddAsync(family);
                }
                else
                {
                    TempData["Danger"] = "Family Username Already Exists, Please Pick Another Name.";
                    return RedirectToAction("Settings");
                }

                //Need to create the FamilyMemberModel

                FamilyMembersModel members = new FamilyMembersModel();
                CustomUserModel user = await _userManager.GetUserAsync(User);
                members.Family = family;
                members.FamilyMember = user;
                await _db.FamilyMembers.AddAsync(members);

                //Need to add te familyId to the customusermodel.
                user.FamilyId = family.Id;
                _db.Users.Update(user);

                //Save all the changes
                await _db.SaveChangesAsync();

                return RedirectToAction("Settings");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> JoinFamily(FamilyModel model)
        {
            //Need to get the family models ID and then create the FamilyMembersModel using that ID.
            if (model != null)
            {
                FamilyMembersModel members = new FamilyMembersModel();
                var family = await _db.Families.FirstOrDefaultAsync(i => i.Name == model.Name);

                if (family != null)
                {
                    CustomUserModel user = await _userManager.GetUserAsync(User);
                    //Need to check to see if the user is already part of the family.
                    var checkUser = await _db.FamilyMembers.FirstOrDefaultAsync(i => i.FamilyMember.Id == user.Id);
                    if (checkUser == null)
                    {
                        members.Family = family;
                        members.FamilyMember = user;
                        //Save the changes.S
                        await _db.FamilyMembers.AddAsync(members);
                        //Need to add te familyId to the customusermodel.
                        user.FamilyId = family.Id;
                        _db.Users.Update(user);

                        await _db.SaveChangesAsync();
                        return RedirectToAction("Settings");

                    }
                    else
                    {
                        TempData["Danger"] = "You Are Already Part of This Family!";
                        return RedirectToAction("Settings");
                    }
                }
                else
                {
                    TempData["Danger"] = "Family Account Doesn't Exist, Please Try Again.";
                    return RedirectToAction("Settings");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> UnlinkFamily(FamilyMembersModel model)
        {
            //NEED TO FIX- CONTENT RETURNS AS NULL


            //Finds the user by the Id and then removes the Family account infomation from the CustomUserModel and the FamilyMembersModel.
            var user = await _userManager.FindByIdAsync(model.FamilyMember.Id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            //Remove the data entry from the CustomUserModel
            user.FamilyId = null;
            _db.Users.Update(user);

            //Remove the instance in FamilyMembersModel
            _db.FamilyMembers.Remove(model);

            //Save changes
            await _db.SaveChangesAsync();

            TempData["Success"] = "Family Account Unlinked!";
            return RedirectToAction("Settings");
        }
    }
}