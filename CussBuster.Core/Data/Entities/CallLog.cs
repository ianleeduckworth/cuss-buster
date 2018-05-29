using System;
using System.Collections.Generic;

namespace CussBuster.Core.Data.Entities
{
    public partial class CallLog
    {
        public int CallLogId { get; set; }
        public int UserId { get; set; }
        public DateTime EventDate { get; set; }

        public User User { get; set; }
    }
}
