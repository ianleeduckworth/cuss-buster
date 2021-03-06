﻿using CussBuster.Core.Exceptions;
using CussBuster.Core.Helpers;
using CussBuster.Core.Models;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Reflection;

namespace CussBuster.Controllers
{
	[Route("v1/signin")]
    public class SigninController : Controller
    {
		private readonly ISigninHelper _signinHelper;
		private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public SigninController(ISigninHelper signinHelper)
		{
			_signinHelper = signinHelper;
		}

		[HttpPost]
		public IActionResult Post([FromBody]SigninModel signinModel)
		{
			try
			{
				return Ok(_signinHelper.Signin(signinModel.EmailAddress, signinModel.Password));
			}
			catch (UserNotFoundException ex)
			{
				_logger.Warn($"Could not find user where email address is {signinModel.EmailAddress}", ex);
				return BadRequest("Could not find user associated with your email address.  Please create an account if you have not done so.");
			}
			catch (UnauthorizedAccessException ex)
			{
				_logger.Error($"Unauthorized access occurred.  Email: {signinModel.EmailAddress}", ex);
				return StatusCode((int)HttpStatusCode.Unauthorized, ex.Message);
			}
			catch (Exception ex)
			{
				var msg = $"Unhandled exception occurred while attempting to sign in: {ex.Message}";
				_logger.Error(msg, ex);
				return StatusCode((int)HttpStatusCode.InternalServerError, msg);
			}
		}
	}
}
