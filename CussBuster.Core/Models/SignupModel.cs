using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CussBuster.Core.Models
{
    public class SignupModel
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

		[Required]
		[CreditCard]
        public string CreditCardNumber { get; set; }

		[Required]
        public byte PricingTierId { get; set; }
	}
}
