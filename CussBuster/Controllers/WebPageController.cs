using CussBuster.Core.Helpers;
using CussBuster.Core.Models;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

		[HttpGet]
		public IActionResult Get(string apiToken)
		{
			try
			{
				if (!Guid.TryParse(apiToken, out Guid apiTokenGuid))
					throw new InvalidOperationException($"Could not parse API token passed in into a GUID.  API token: {apiToken}");

				var result = _webPageHelper.GetUserInfo(apiTokenGuid);

				if (result == null)
					return NotFound();

				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.Error($"Unhandled exception occurred when attempting to get user data", ex);
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost]
		public IActionResult Post([FromBody] UserSignupModel signupModel)
		{
			try
			{
				ValidateModel(signupModel);

				var result = _webPageHelper.SignUp(signupModel);
				return Ok(result);
			}
			catch(Exception ex)
			{
				_logger.Error($"Unhandled exception occurred when attempting to sign up", ex);
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPut]
		public IActionResult Put(string apiToken, [FromBody]UserUpdateModel userUpdateModel)
		{
			try
			{
				ValidateModel(userUpdateModel);

				if (!Guid.TryParse(apiToken, out Guid apiTokenGuid))
					throw new InvalidOperationException($"Could not parse API token passed in into a GUID.  API token: {apiToken}");

				var result = _webPageHelper.UpdateUserInfo(apiTokenGuid, userUpdateModel);

				if (result == null)
					return NotFound();

				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.Error($"Unhandled exception occurred when attempting to update user data", ex);
				return StatusCode(500, ex.Message);
			}
		}

		[HttpOptions]
		public IActionResult Options()
		{
			return Ok();
		}

		private void ValidateModel<T>(T model) where T : UserSignupModel
		{
			if (!ModelState.IsValid)
			{
				var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).Select(x => x[0].ErrorMessage).Aggregate((a, b) => $"{a}{Environment.NewLine}{b}");
				throw new InvalidOperationException(errors);
			}

			if (!IsValidEmail(model.EmailAddress))
				throw new InvalidOperationException("Email address entered is invalid");
		}

		private bool IsValidEmail(string email)
		{
			try
			{
				var addr = new MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}
	}
}
