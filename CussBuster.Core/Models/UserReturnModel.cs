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
    }
}
