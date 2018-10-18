using CussBuster.Core.ExtensionMethods;
using CussBuster.Core.Helpers;
using CussBuster.Core.Models;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Mail;
using System.Reflection;

namespace CussBuster.Controllers
{
	[Route("v1/webPage")]
	public class WebPageController : Controller
    {
		private readonly IWebPageHelper _webPageHelper;
		private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
				{
					string msg = $"Could not parse API token passed in into a GUID.  API token: {apiToken}";
					_logger.Error(msg);
					return StatusCode((int)HttpStatusCode.BadRequest, msg);
				}

				var result = _webPageHelper.GetUserInfo(apiTokenGuid);

				if (result == null)
					return NotFound();

				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.Error($"Unhandled exception occurred when attempting to get user data", ex);
				return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
			}
		}

		[HttpPost]
		public IActionResult Post([FromBody] UserSignupModel signupModel)
		{
			try
			{
				const string user = "api";

				ValidateModel(signupModel);

				var result = _webPageHelper.SignUp(signupModel, user);
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				_logger.Error(ex.Message);
				return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
			}
			catch(Exception ex)
			{
				_logger.Error($"Unhandled exception occurred when attempting to sign up", ex);
				return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
			}
		}

		[HttpPut]
		public IActionResult Put(string apiToken, string password, [FromBody]UserUpdateModel userUpdateModel)
		{
			try
			{
				ValidateModel(userUpdateModel);

				if (!Guid.TryParse(apiToken, out Guid apiTokenGuid))
				{
					string msg = $"Could not parse API token passed in into a GUID.  API token: {apiToken}";
					_logger.Error(msg);
					return StatusCode((int)HttpStatusCode.BadRequest, msg);
				}

				var result = _webPageHelper.UpdateUserInfo(apiTokenGuid, password, userUpdateModel);

				if (result == null)
					return NotFound();

				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.Error($"Unhandled exception occurred when attempting to update user data", ex);
				return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
			}
		}

		[HttpOptions]
		public IActionResult Options()
		{
			return Ok();
		}

		private void ValidateModel<T>(T model) where T : UserModel
		{
			if (!ModelState.IsValid)
			{
				throw new InvalidOperationException(ModelState.GetErrorsText());
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
