using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DissertationProject.Models
{
    public class FamilyBudgetModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Family")]
        public int FamilyId { get; set; }
        public FamilyModel Family { get; set; }

        public float Budget { get; set; }
    }
}

