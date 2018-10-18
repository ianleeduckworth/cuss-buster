using System;
using System.Collections.Generic;

namespace CussBuster.Core.Data.Entities
{
    public partial class UserSetting
    {
        public int UserSettingId { get; set; }
        public int UserId { get; set; }
        public byte WordTypeId { get; set; }
        public byte Severity { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public User User { get; set; }
    }
}
