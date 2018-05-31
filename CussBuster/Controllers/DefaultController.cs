using System;
using Microsoft.AspNetCore.Mvc;
using CussBuster.Core.Helpers;
using CussBuster.Core.Settings;
using log4net;
using System.Reflection;

namespace CussBuster.Controllers
{
	[Route("v1")]
	public class DefaultController : Controller
	{
		private readonly IMainHelper _mainHelper;
		private readonly IAppSettings _appSettings;

		private ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public DefaultController(IMainHelper mainHelper, IAppSettings appSettings)
		{
			_mainHelper = mainHelper;
			_appSettings = appSettings;
		}

		[HttpPost]
		[Route("{authToken}")]
		public IActionResult Post([FromBody]string text, string authToken)
		{
			try
			{
				if (!_mainHelper.CheckCharacterLimit(text))
				{
					_logger.Error($"User with auth token '{authToken}' was over the character limit of {_appSettings.CharacterLimit}.  Text length: {text.Length}");
					return BadRequest($"Text passed in is longer than the {_appSettings.CharacterLimit} character limit.  Text length: {text.Length}.");
				}

				var user = _mainHelper.CheckAuthorization(authToken);

				if (user == null)
				{
					_logger.Error($"AuthToken: {authToken} could not be found in the database; user is unauthorized");
					return Unauthorized();
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