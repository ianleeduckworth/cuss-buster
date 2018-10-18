using System.ComponentModel.DataAnnotations;

namespace CussBuster.Core.Models
{
	public class SigninModel
    {
		[Required]
		[EmailAddress]
		public string EmailAddress { get; set; }

		[Required]
		[MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
		public string Password { get; set; }
    }
}
