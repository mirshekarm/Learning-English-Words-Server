﻿using Persistence;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrustructrue;
using Services;
using ViewModels.Requests;
using ViewModels.Responses;
using Dtat.Logging;
using Infrustructrue.Attributes;
using Infrustructrue.Enums;

namespace Server.Controllers
{
	public class UsersController : BaseApiControllerWithDatabase
	{	
		#region Constractor
		public UsersController
			(IUnitOfWork unitOfWork,
			IUserServices userServices,
			ILogger<UsersController> logger) : base(unitOfWork)
		{
			Logger = logger;
			UserServices = userServices;
		}
		#endregion Constractor

		#region Properties
		public IUserServices UserServices { get; }
		public ILogger<UsersController> Logger { get; }
		#endregion /Properties

		#region HttpGet
		[Authorize(UserRoles.Admin)]
		[HttpGet("GetAllUsers")]
		public async Task<ActionResult<Dtat.Results.Result<List<User>>>> GetAllUsers()
		{
			var result =
				await UserServices.GetAllUsersAsync();

			if (result.IsFailed)
				return BadRequest(result);

			return Ok(result);
		}
		#endregion /HttpGet

		#region HttpPost
		[HttpPost("Login")]
		public async Task<ActionResult<Dtat.Results.Result<LoginResponseViewModel>>>
			LoginAsync([FromBody] LoginRequestViewModel viewModel)
		{
			var response = 
				await UserServices.LoginAsync(viewModel, ipAddress: GetIPAddress());

			if (response.IsFailed)
				return BadRequest(response);

			return Ok(response);
		}


		[HttpPost("Register")]
		public async Task<ActionResult<Dtat.Results.Result>>
			RegisterAccount([FromBody] RegisterRequestViewModel registerRequestViewModel)
		{
			var result =
				await UserServices.RegisterAsync(registerRequestViewModel: registerRequestViewModel);

			if (result.IsFailed)
				return BadRequest(result);

			return Ok(result);
		}


		[Authorize(UserRoles.Admin)]
		[HttpPost("UpdateUser")]
		public async Task<ActionResult<Dtat.Results.Result>>
			UpdateUserAsync([FromBody] UpdateUserRequestViewModel updateUserRequestViewModel)
		{
			var result =
				await UserServices.UpdateUserAsync(updateUserRequestViewModel: updateUserRequestViewModel);

			if (result.IsFailed)
				return BadRequest(result);

			return Ok(result);
		}


		[HttpDelete("Logout/{refreshToken}")]
		public async Task<ActionResult<Dtat.Results.Result>> LogoutToken(string refreshToken)
		{
			var response = await
				UserServices.LogoutAsync(refreshToken);

			if (response.IsFailed)
				return BadRequest(response);

			return Ok(response);
		}


		[Authorize(UserRoles.Admin)]
		[HttpPost("ChangeUserRole")]
		public async Task<ActionResult<Dtat.Results.Result>>
			ChangeUserRoleAsync([FromBody] ChangeUserRoleRequestViewModel changeUserRoleRequestViewModel)
		{
			var result =
				await UserServices.ChangeUserRoleAsync(changeUserRoleRequestViewModel);

			if (result.IsFailed)
				return BadRequest(result);

			return Ok(result);
		}


		[HttpPost("RefreshToken/{refreshToken?}")]
		public async Task<ActionResult<Dtat.Results.Result<LoginResponseViewModel>>> RefreshToken(string refreshToken)
		{
			var response =
				await UserServices.RefreshTokenAsync(token: refreshToken, ipAddress: GetIPAddress());

			if (response.IsFailed)
				return BadRequest(response);

			return Ok(response);
		}
		#endregion /HttpPost

		#region Methods
		[NonAction]
		private string GetIPAddress()
		{
			var requestHeaders = Request?.Headers;

			if (requestHeaders != null && requestHeaders.ContainsKey("X-Forwarded-For"))
            {
				return Request.Headers["X-Forwarded-For"];
			}
            else
            {
				return HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4().ToString();
			}			
		}
		#endregion /Methods
	}
}
