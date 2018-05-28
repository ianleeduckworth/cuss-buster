using System;
using System.Collections.Generic;

namespace CussBuster.Core.Data.Entities
{
    public partial class WordAudit
    {
        public long WordAuditId { get; set; }
        public int WordId { get; set; }
        public DateTime EventDate { get; set; }
    }
}
