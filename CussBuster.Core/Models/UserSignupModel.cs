using System.ComponentModel.DataAnnotations;

namespace CussBuster.Core.Models
{
	public class UserSignupModel : UserModel
    {
		[Required]
		[StringLength(100)]
		public string AddressLine1 { get; set; }

		[Required]
		[StringLength(100)]
		public string AddressLine2 { get; set; }

		[Required]
		[StringLength(50)]
		public string City { get; set; }

		[Required]
		[StringLength(2)]
		public string State { get; set; }

		[Required]
		[StringLength(5)]
		public string ZipCode { get; set; }

		[Required]
		[StringLength(5)]
		public string CreditCardExpirationDate { get; set; }

		[Required]
		[StringLength(3)]
		public string CreditCardCvcCode { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
