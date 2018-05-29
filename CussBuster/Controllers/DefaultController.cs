using System;
using Microsoft.AspNetCore.Mvc;
using CussBuster.Core.Helpers;
using CussBuster.Core.Settings;

namespace CussBuster.Controllers
{
	[Route("v1")]
	public class DefaultController : Controller
	{
		private readonly IMainHelper _mainHelper;
		private readonly IAppSettings _appSettings;

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
					return BadRequest($"Text passed in is longer than the {_appSettings.CharacterLimit} character limit.  Text length: {text.Length}.");

				var user = _mainHelper.CheckAuthorization(authToken);

				if (user == null)
					return Unauthorized();

				return Ok(_mainHelper.FindMatches(text, user));
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		[HttpGet]
		[Route("")]
		public IActionResult Default()
		{
			return Ok();
		}
	}
}