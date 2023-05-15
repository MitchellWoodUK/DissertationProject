using DissertationProject.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DissertationProject.Models
{
    public class FamilyTransactionModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Family")]
        public int FamilyId { get; set; }
        public FamilyModel Family { get; set; }

        [Required]
        [ForeignKey("CustomUser")]
        public string UserId { get; set; }
        public CustomUserModel CustomUser { get; set; }


        [Required]
        public TransactionTypeEnum TransactionType { get; set; }

        [Required]
        public string Name { get; set; }


        public string Description { get; set; }

        [Required]
        public float Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}