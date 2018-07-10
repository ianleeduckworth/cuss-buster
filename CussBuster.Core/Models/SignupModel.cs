using System.ComponentModel.DataAnnotations;

namespace CussBuster.Core.Models
{
	public class UserSignupModel
    {
		[Required]
		[MaxLength(100)]
		public string FirstName { get; set; }

		[Required]
		[MaxLength(100)]
        public string LastName { get; set; }

		[Required]
		[MaxLength(200)]
        public string EmailAddress { get; set; }

		[CreditCard]
        public string CreditCardNumber { get; set; }

		[Required]
        public byte PricingTierId { get; set; }
	}
}
