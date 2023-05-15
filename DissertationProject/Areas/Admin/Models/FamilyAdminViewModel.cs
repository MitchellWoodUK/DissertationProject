using System;
using DissertationProject.Models;

namespace DissertationProject.Areas.Admin.Models
{
    public class FamilyAdminViewModel
    {
        public FamilyModel Family { get; set; }
        public List<FamilyMembersModel> FamilyMembers { get; set; }
    }
}