using System;
using Microsoft.AspNetCore.Mvc;
using CussBuster.Core.Helpers;

namespace CussBuster.Controllers
{
	[Route("v1")]
	public class DefaultController : Controller
	{
		private readonly IMainHelper _mainHelper;

		public DefaultController(IMainHelper mainHelper)
		{
			_mainHelper = mainHelper;
		}

		[HttpPost]
		[Route("{authToken}")]
		public IActionResult Post([FromBody]string text, string authToken)
		{
			try
			{
				if (!_mainHelper.CheckAuthorization(authToken))
					return Unauthorized();

				return Ok(_mainHelper.FindMatches(text));
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