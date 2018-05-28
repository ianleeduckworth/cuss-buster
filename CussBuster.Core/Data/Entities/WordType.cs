using System;
using System.Collections.Generic;

namespace CussBuster.Core.Data.Entities
{
    public partial class WordType
    {
        public WordType()
        {
            Word = new HashSet<Word>();
        }

        public byte WordTypeId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }

        public ICollection<Word> Word { get; set; }
    }
}
