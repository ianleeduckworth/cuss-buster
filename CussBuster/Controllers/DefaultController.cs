using System;
using Microsoft.AspNetCore.Mvc;
using CussBuster.Core.Helpers;
using CussBuster.Core.Settings;
using log4net;
using System.Reflection;
using CussBuster.Core.DataAccess;
using System.Net;

namespace CussBuster.Controllers
{
	[Route("v1")]
	public class DefaultController : Controller
	{
		private readonly IMainHelper _mainHelper;
		private readonly IAppSettings _appSettings;
		private readonly IUserManager _userManager;

		private ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public DefaultController(IMainHelper mainHelper, IAppSettings appSettings, IUserManager userManager)
		{
			_mainHelper = mainHelper;
			_appSettings = appSettings;
			_userManager = userManager;
		}

		[HttpPost]
		[Route("{authToken}")]
		public IActionResult Post([FromBody]string text, string authToken)
		{
			try
			{
				//check if the request is over the character limit
				if (!_mainHelper.CheckCharacterLimit(text))
				{
					_logger.Error($"User with auth token '{authToken}' was over the character limit of {_appSettings.CharacterLimit}.  Text length: {text.Length}");
					return BadRequest($"Text passed in is longer than the {_appSettings.CharacterLimit} character limit.  Text length: {text.Length}.");
				}

				//get the user based on the auth token.  if the auth token can't be correlated to a user, return unauthorized
				var user = _mainHelper.CheckAuthorization(authToken);
				if (user == null)
				{
					_logger.Error($"AuthToken: {authToken} could not be found in the database; user is unauthorized");
					return Unauthorized();
				}

				//if the user can call the api currently, see if the user is at/over their monthly limit.  If they are, their account will be locked
				if (user.CanCallApi)
				{
					_userManager.CheckLockAccount(user);
				}
				
				//if the user cannot call the API, check to see if we should unlock the account (if, for example, it's the first of the month).  If we shouldn't, return a bad request
				if (!user.CanCallApi)
				{
					if (!_userManager.CheckUnlockAccount(user))
					{
						_logger.Error($"User with auth token: {authToken} could not call API.  CanCallApi is false; check monthly limit for user");
						return StatusCode((int)HttpStatusCode.PaymentRequired, "You have reached your call limit for the month.  Please contact support for more information");
					}
				}

				_logger.Info($"User with AuthToken: {authToken} called API successfully");
				return Ok(_mainHelper.FindMatches(text, user));
			}
			catch (Exception ex)
			{
				_logger.Error($"Unhandled exception occurred for user with AuthToken: {authToken}", ex);
				return BadRequest();
			}
		}

		[HttpGet]
		[Route("")]
		public IActionResult Default()
		{
			_logger.Info("Default endpoint hit");
			return Ok();
		}
	}
}