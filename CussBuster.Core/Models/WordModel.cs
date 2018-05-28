using System;
using System.Collections.Generic;
using System.Text;

namespace CussBuster.Core.Models
{
    public class WordModel
    {
		public int WordId { get; set; }
		public string Word { get; set; }
		public byte WordTypeId { get; set; }
		public byte Severity { get; set; }
		public byte SearchTypeId { get; set; }
	}
}
