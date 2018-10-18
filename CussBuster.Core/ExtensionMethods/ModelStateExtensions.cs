using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CussBuster.Core.ExtensionMethods
{
    public static class ModelStateExtensions
    {
		public static string GetErrorsText(this ModelStateDictionary modelStateDictionary)
		{
			return modelStateDictionary.Select(x => x.Value.Errors).Where(y => y.Count > 0).Select(x => x[0].ErrorMessage).Aggregate((a, b) => $"{a}{Environment.NewLine}{b}");
		}
    }
}
