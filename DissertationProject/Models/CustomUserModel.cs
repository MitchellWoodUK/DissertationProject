using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DissertationProject.Models
{
    public class CustomUserModel : IdentityUser
    {
        [Required]
        public string Fname { get; set; }
        [Required]
        public string Sname { get; set; }
        
        public string JobName { get; set; }

        public float Income { get; set; }

        [ForeignKey("FamilyMembers")]
        public int? FamilyId { get; set; }
    }
}
