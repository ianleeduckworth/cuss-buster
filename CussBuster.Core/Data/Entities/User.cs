using System;
using System.Collections.Generic;

namespace CussBuster.Core.Data.Entities
{
    public partial class User
    {
        public User()
        {
            CallLog = new HashSet<CallLog>();
            UserSetting = new HashSet<UserSetting>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid ApiToken { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool CanCallApi { get; set; }
        public int CallsPerMonth { get; set; }
        public decimal PricePerMonth { get; set; }
        public string Email { get; set; }
        public decimal? CreditCardNumber { get; set; }
		public string CreditCardExpirationDate { get; set; }
		public string CreditCardCvcCode { get; set; }
		public byte[] Password { get; set; }
		public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }

		public ICollection<CallLog> CallLog { get; set; }
        public ICollection<UserSetting> UserSetting { get; set; }
    }
}
