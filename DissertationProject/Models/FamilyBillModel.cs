using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DissertationProject.Enums;

namespace DissertationProject.Models
{
    public class FamilyBillModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Family")]
        public int FamilyId { get; set; }
        public FamilyModel Family { get; set; }

        [Required]
        public BillTypeEnum BillType { get; set; }

        [Required]
        public string Name { get; set; }


        public string? Description { get; set; }

        [Required]
        public float Amount { get; set; }

        [Required]
        public int DateDue { get; set; }
    }
}