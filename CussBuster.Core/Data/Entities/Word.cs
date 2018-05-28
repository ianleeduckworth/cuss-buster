using System;

namespace CussBuster.Core.Data.Entities
{
	public partial class Word
    {
        public int WordId { get; set; }
        public string BadWord { get; set; }
        public byte WordTypeId { get; set; }
        public byte Severity { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public byte SearchTypeId { get; set; }

        public SearchType SearchType { get; set; }
        public WordType WordType { get; set; }
    }
}
