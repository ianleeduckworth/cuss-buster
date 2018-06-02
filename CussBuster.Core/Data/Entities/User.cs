using System;
using System.Collections.Generic;

namespace CussBuster.Core.Data.Entities
{
    public partial class User
    {
        public User()
        {
            CallLog = new HashSet<CallLog>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid ApiToken { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public bool CanCallApi { get; set; }
        public int CallsPerMonth { get; set; }
        public decimal PricePerMonth { get; set; }

        public ICollection<CallLog> CallLog { get; set; }
    }
}
