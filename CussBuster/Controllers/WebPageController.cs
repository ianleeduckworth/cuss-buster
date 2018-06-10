using CussBuster.Core.Helpers;
using CussBuster.Core.Models;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace CussBuster.Controllers
{
	[Route("v1/webPage")]
	public class WebPageController : Controller
    {
		private IWebPageHelper _webPageHelper;

		private ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public WebPageController(IWebPageHelper webPageHelper)
		{
			_webPageHelper = webPageHelper;
		}

		[HttpPost]
		public IActionResult Post([FromBody] SignupModel signupModel)
		{
			try
			{
				var result = _webPageHelper.SignUp(signupModel);
				return Ok(result);
			}
			catch(Exception ex)
			{
				_logger.Error($"Unhandled exception occurred when attempting to sign up", ex);
				return StatusCode(500, ex.Message);
			}
		}

		[HttpOptions]
		public IActionResult Options()
		{
			return Ok();
		}
    }
}
