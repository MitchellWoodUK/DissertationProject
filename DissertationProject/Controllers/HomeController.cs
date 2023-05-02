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
            var chartList = new List<ChartViewModel>();
            //Need to return a list of ChartViewModel from the logged in family.
            //Need to get the user that is logged in.
            var user = _userManager.GetUserAsync(User);
            if(user != null)
            {
                //Need to get the family that the user is in.
                var family = _db.Families.FirstOrDefaultAsync(i => i.Id == user.Result.FamilyId);
                if(family != null)
                {
                    //Need to get the family members that are in the family.
                    var familyMembers = _db.FamilyMembers.Where(i => i.FamilyMember.FamilyId == family.Result.Id).ToList();
                    //Need to get the bills that are in the family.
                    var familyBills = _db.FamilyBills.Where(i => i.FamilyId == family.Result.Id).ToList();
                    //Need to get the budgets that are in the family.
                    var familyBudgets = _db.FamilyBudgets.Where(i => i.FamilyId == family.Result.Id).ToList();
                    //Need to get the transactions that are in the family.
                    var familyTransactions = _db.FamilyTransactions.Where(i => i.FamilyId == family.Result.Id).ToList();
                    chartList.Add(new ChartViewModel()
                    {
                        Members = familyMembers,
                        Bills = familyBills,
                        Budgets = familyBudgets,
                        Transactions = familyTransactions
                    });
                    //return to the view with the list.
                    return View(chartList);
                }
                else
                {
                    //If family is null then return an empty list.
                    return View(chartList);
                }
            }
            return View(chartList);
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
                //Check that the pin is the correct length.
                if (model.PIN.Length == 4 && System.Text.RegularExpressions.Regex.IsMatch(model.PIN, "^[0-9]{4}$"))
                {
                    family.PIN = model.PIN;
                }
                else
                {
                    TempData["Danger"] = "PIN needs to be 4 digits long and only numbers!";
                    return RedirectToAction("Settings");
                }

                //Need to check that the family name entered is not already in the database.
                var checkUnique = await _db.Families.FirstOrDefaultAsync(i => i.Name == family.Name && i.PIN == family.PIN);
                if (checkUnique == null)
                {
                    //Creates the family and saves it to the database.
                    await _db.Families.AddAsync(family);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    TempData["Danger"] = "Family Already Exists! Please Join!";
                    return RedirectToAction("Settings");
                }

                //Need to create the FamilyMemberModel
                //Creates the empty family members model.
                FamilyMembersModel members = new FamilyMembersModel();
                //Gets the user that is logged in.
                CustomUserModel user = await _userManager.GetUserAsync(User);
                //Get the family that was just created.
                members.Family = await _db.Families.FirstOrDefaultAsync(i => i.Name == family.Name);
                //Adds the family and user to the family members model.
                members.FamilyMember = user;
                //Save the changes.
                await _db.FamilyMembers.AddAsync(members);

                //Need to add te familyId to the customusermodel.
                user.FamilyId = members.Family.Id;
                _db.Users.Update(user);

                //Need to add the family head role to the user.
                await _userManager.AddToRoleAsync(user, "Family_Head");

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
                var family = await _db.Families.FirstOrDefaultAsync(i => i.Name == model.Name && i.PIN == model.PIN);

                if (family != null)
                {
                    CustomUserModel user = await _userManager.GetUserAsync(User);
                    //Need to check to see if the user is already part of the family.
                    var checkUser = await _db.FamilyMembers.FirstOrDefaultAsync(i => i.FamilyMember.Id == user.Id);
                    if (checkUser == null)
                    {
                        members.Family = family;
                        members.FamilyMember = user;
                        //Save the changes.
                        await _db.FamilyMembers.AddAsync(members);
                        //Need to add te familyId to the customusermodel.
                        user.FamilyId = members.Family.Id;
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
                    TempData["Danger"] = "Family Account Doesn't Exist. Please Check Family Name and PIN.";
                    return RedirectToAction("Settings");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> UnlinkFamily(FamilyModel model)
        {
            //Get the user who is logged in.
            CustomUserModel user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Index");
            }
            //Find the family member instance that is linked to the user.
            var family = await _db.FamilyMembers.FirstOrDefaultAsync(i => i.FamilyMember.Id == user.Id);
            if (family == null)
            {
                return RedirectToAction("Index");
            }

            //Remove the data entry from the CustomUserModel
            user.FamilyId = null;
            _db.Users.Update(user);

            //Remove the instance in FamilyMembersModel
            _db.FamilyMembers.Remove(family);

            //Save changes
            await _db.SaveChangesAsync();

            TempData["Success"] = "Family Account Unlinked!";
            return RedirectToAction("Settings");
        }
    }
}