using System;
using System.Collections.Generic;
using System.Text;

namespace CussBuster.Core.Models
{
    public class UserReturnModel
    {
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public Guid ApiToken { get; set; }
		public bool AccountLocked { get; set; }
		public int CallsPerMonth { get; set; }
		public decimal PricePerMonth { get; set; }
		public string AccountType { get; set; }
		public string CreditCardNumber { get; set; }
		public int CallsThisMonth { get; set; }
		public bool Racism { get; set; }
		public byte? RacismSeverity { get; set; }
		public bool Vulgarity { get; set; }
		public byte? VulgaritySeverity { get; set; }
		public bool Sexism { get; set; }
		public byte? SexismSeverity { get; set; }
	}
}
