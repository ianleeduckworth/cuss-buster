using System;
using System.Collections.Generic;
using System.Text;

namespace CussBuster.Core.Models
{
    public class ReturnModel
    {
		public string Word { get; set; }
		public byte WordTypeId { get; set; }
		public string WordType { get; set; }
		public byte Severity { get; set; }
		public int Occurrences { get; set; }
	}
}
