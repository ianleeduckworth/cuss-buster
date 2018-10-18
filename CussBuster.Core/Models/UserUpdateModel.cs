using System;
using System.Collections.Generic;
using System.Text;

namespace CussBuster.Core.Models
{
    public class UserUpdateModel : UserModel
    {
		public bool Racism { get; set; }

		public bool Sexism { get; set; }

		public bool Vulgarity { get; set; }
    }
}
