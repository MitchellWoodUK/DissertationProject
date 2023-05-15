using System;
using System.ComponentModel.DataAnnotations;

namespace DissertationProject.Models
{
	public class FamilyMembersModel
	{
		[Key]
		public int Id { get; set;}
		[Required]
		public FamilyModel Family { get; set; }
		[Required]
		public CustomUserModel FamilyMember { get; set; }
	}
}