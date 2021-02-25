using AccountAndJwt.AuthorizationService.Middleware;
using AccountAndJwt.AuthorizationService.Services.Interfaces;
using AccountAndJwt.AuthorizationService.Services.Models;
using AccountAndJwt.Common.Consts;
using AccountAndJwt.Contracts.Models;
using AccountAndJwt.Contracts.Models.Errors;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AccountAndJwt.AuthorizationService.Controllers
{
	/// <summary>
	/// Provide authorization activities
	/// </summary>
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class AccountController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IUserService _userService;
		private readonly IUrlHelper _urlHelper;


		public AccountController(IUserService userService, IUrlHelper urlHelper, IMapper mapper)
		{
			_mapper = mapper;
			_userService = userService;
			_urlHelper = urlHelper;
		}


		// ACTIONS ////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Register new user by provided credentials
		/// </summary>
		/// <param name="userDetails"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(typeof(RegisterUserResponseAm), 201)]
		[ProducesResponseType(typeof(ModelStateAm), 400)]
		[ProducesResponseType(typeof(ModelStateAm), 415)]
		[ProducesResponseType(typeof(String), 460)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> Register([FromBody] RegisterUserAm userDetails)
		{
			if(!ModelState.IsValid)
				return BadRequest(ModelState);

			await _userService.RegisterAsync(_mapper.Map<UserDto>(userDetails));

			return Ok(new RegisterUserResponseAm()
			{
				GetAccessTokenUrl = _urlHelper.Action("AuthorizeByCredentials", "Token"),
				RefreshAccessTokenUrl = _urlHelper.Action("RefreshToken", "Token")
			});
		}

		/// <summary>
		/// [Auth=(admin)] Delete existed account by it id
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize(Roles = Role.Admin)]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(ModelStateAm), 400)]
		[ProducesResponseType(401)]
		[ProducesResponseType(403)]
		[ProducesResponseType(typeof(ModelStateAm), 415)]
		[ProducesResponseType(typeof(String), 460)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> DeleteAccount([FromBody] Int32 userId)
		{
			if(userId == 0)
				return StatusCode(460, $"Please provide \"{nameof(userId)}\"");

			await _userService.DeleteUserAsync(userId);

			return Ok();
		}

		/// <summary>
		/// [Auth=(admin)] Add role to user with specified id
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize(Roles = Role.Admin)]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(ModelStateAm), 400)]
		[ProducesResponseType(401)]
		[ProducesResponseType(403)]
		[ProducesResponseType(typeof(ModelStateAm), 415)]
		[ProducesResponseType(typeof(String), 460)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> AddRole([FromBody] AddRemoveRoleAm request)
		{
			if(!ModelState.IsValid)
				return BadRequest(ModelState);

			await _userService.AddRoleAsync(request.UserId, request.Role);

			return Ok();
		}

		/// <summary>
		/// [Auth=(admin)] Delete role from user with specified id
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize(Roles = Role.Admin)]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(ModelStateAm), 400)]
		[ProducesResponseType(401)]
		[ProducesResponseType(403)]
		[ProducesResponseType(typeof(ModelStateAm), 415)]
		[ProducesResponseType(typeof(String), 460)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> RemoveRole([FromBody] AddRemoveRoleAm request)
		{
			if(!ModelState.IsValid)
				return BadRequest(ModelState);

			await _userService.RemoveRoleAsync(request.UserId, request.Role);

			return Ok();
		}

		/// <summary>
		/// [Auth] Change E-Mail of current user
		/// </summary>
		/// <param name="newEmail"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(ModelStateAm), 400)]
		[ProducesResponseType(401)]
		[ProducesResponseType(typeof(String), 460)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> ChangeEmail([FromBody] String newEmail)
		{
			if(String.IsNullOrEmpty(newEmail))
				return StatusCode(460, $"Please provide valid \"{nameof(newEmail)}\"");

			await _userService.ChangeEmailAsync(User.GetId(), newEmail);

			return Ok();
		}

		/// <summary>
		/// [Auth] Change current user First and Last name by provided id
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize]
		[ProducesResponseType(200)]
		[ProducesResponseType(401)]
		[ProducesResponseType(typeof(ModelStateAm), 415)]
		[ProducesResponseType(typeof(String), 460)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> ChangeName([FromBody] ChangeNameRequestAm request)
		{
			if(!ModelState.IsValid)
				return BadRequest(ModelState);

			await _userService.ChangeNameAsync(User.GetId(), request.FirstName, request.LastName);

			return Ok();
		}

		/// <summary>
		/// [Auth=(admin)] Get information about user by provided id
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		[Authorize(Roles = Role.Admin)]
		[HttpGet]
		[ProducesResponseType(typeof(UserAm), 200)]
		[ProducesResponseType(401)]
		[ProducesResponseType(403)]
		[ProducesResponseType(typeof(String), 460)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> GetUser([FromQuery] Int32 userId)
		{
			return Ok(_mapper.Map<UserAm>(await _userService.GetUserAsync(userId)));
		}

		/// <summary>
		/// [Auth=(admin)] Get information about all users
		/// </summary>
		/// <returns></returns>
		[Authorize(Roles = Role.Admin)]
		[HttpGet]
		[ProducesResponseType(typeof(UserAm[]), 200)]
		[ProducesResponseType(401)]
		[ProducesResponseType(403)]
		[ProducesResponseType(typeof(String), 460)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> GetAllUsers()
		{
			return Ok(_mapper.Map<UserAm[]>(await _userService.GetAllUsersAsync()));
		}

		/// <summary>
		/// [Auth] Changing current user password
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Authorize]
		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(ModelStateAm), 400)]
		[ProducesResponseType(401)]
		[ProducesResponseType(typeof(ModelStateAm), 415)]
		[ProducesResponseType(typeof(String), 460)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordAm request)
		{
			if(!ModelState.IsValid)
				return BadRequest(ModelState);

			await _userService.UpdatePasswordAsync(User.GetId(), request.OldPassword, request.NewPassword);

			return Ok();
		}
	}
}