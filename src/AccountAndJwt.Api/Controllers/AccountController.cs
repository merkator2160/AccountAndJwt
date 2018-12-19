using AccountAndJwt.Api.Contracts.Models;
using AccountAndJwt.Api.Middleware;
using AccountAndJwt.Api.Services.Interfaces;
using AccountAndJwt.Api.Services.Models;
using AccountAndJwt.Common.Consts;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AccountAndJwt.Api.Controllers
{
	/// <summary>
	/// Provide authorization activities
	/// </summary>
	[Route("api/[controller]/[action]")]
	public class AccountController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IUserService _userService;
		private readonly IUrlHelper _urlHelper;
		private readonly ILogger<AccountController> _logger;


		public AccountController(IUserService userService, IUrlHelper urlHelper, ILogger<AccountController> logger, IMapper mapper)
		{
			_mapper = mapper;
			_userService = userService;
			_urlHelper = urlHelper;
			_logger = logger;
		}


		// ACTIONS ////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Register new user by provided credentials
		/// </summary>
		/// <param name="userDetails"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(typeof(RegisterUserResponseAm), 201)]
		[ProducesResponseType(typeof(String), 400)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> Register([FromBody]RegisterUserAm userDetails)
		{
			try
			{
				if(!ModelState.IsValid)
					return BadRequest($"Please provide valid \"{nameof(userDetails)}\"");

				await _userService.RegisterAsync(_mapper.Map<UserDto>(userDetails));
				_logger.LogInformation($"New user registered {userDetails.Login}");

				return Ok(new RegisterUserResponseAm()
				{
					GetAccessTokenUrl = _urlHelper.Action("AuthorizeByCredentials", "Token"),
					RefreshAccessTokenUrl = _urlHelper.Action("RefreshToken", "Token")
				});
			}
			catch(ApplicationException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// [Auth=(admin)] Delete existed account by it id
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize(Roles = Role.Admin)]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(String), 400)]
		[ProducesResponseType(401)]
		[ProducesResponseType(403)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> DeleteAccount([FromBody]Int32 userId)
		{
			try
			{
				if(userId == 0)
					return BadRequest($"Please provide \"{nameof(userId)}\"");

				await _userService.DeleteUserAsync(userId);
				_logger.LogInformation($"User with id {userId} deleted");

				return Ok();
			}
			catch(ApplicationException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// [Auth=(admin)] Add role to user with specified id
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize(Roles = Role.Admin)]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(String), 400)]
		[ProducesResponseType(401)]
		[ProducesResponseType(403)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> AddRole([FromBody]AddRemoveRoleAm request)
		{
			try
			{
				if(!ModelState.IsValid)
					return BadRequest($"Please provide valid \"{nameof(request)}\"");

				await _userService.AddRoleAsync(request.UserId, request.Role);
				return Ok();
			}
			catch(ApplicationException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// [Auth=(admin)] Delete role from user with specified id
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize(Roles = Role.Admin)]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(String), 400)]
		[ProducesResponseType(401)]
		[ProducesResponseType(403)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> RemoveRole([FromBody]AddRemoveRoleAm request)
		{
			try
			{
				if(!ModelState.IsValid)
					return BadRequest($"Please provide valid \"{nameof(request)}\"");

				await _userService.RemoveRoleAsync(request.UserId, request.Role);
				return Ok();
			}
			catch(ApplicationException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// [Auth] Change E-Mail of current user
		/// </summary>
		/// <param name="newEmail"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(String), 400)]
		[ProducesResponseType(401)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> ChangeEmail([FromBody]String newEmail)
		{
			try
			{
				if(String.IsNullOrEmpty(newEmail))
					return BadRequest($"Please provide valid \"{nameof(newEmail)}\"");

				var currentUserId = User.GetId();

				await _userService.ChangeEmailAsync(currentUserId, newEmail);
				_logger.LogInformation($"Email of user {currentUserId} updated");

				return Ok();
			}
			catch(ApplicationException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// [Auth] Change current user First and Last name by provided id
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(String), 400)]
		[ProducesResponseType(401)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> ChangeName([FromBody]ChangeNameRequestAm request)
		{
			try
			{
				if(!ModelState.IsValid)
					return BadRequest($"Please provide valid \"{nameof(request)}\"");

				var currentUserId = User.GetId();

				await _userService.ChangeNameAsync(currentUserId, request.FirstName, request.LastName);
				_logger.LogInformation($"Email of user {currentUserId} updated");

				return Ok();
			}
			catch(ApplicationException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// [Auth=(admin)] Get information about user by provided id
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		[Authorize(Roles = Role.Admin)]
		[HttpGet]
		[ProducesResponseType(typeof(UserAm), 200)]
		[ProducesResponseType(typeof(String), 400)]
		[ProducesResponseType(401)]
		[ProducesResponseType(403)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> GetUser(Int32 userId)
		{
			try
			{
				return Ok(_mapper.Map<UserAm>(await _userService.GetUserAsync(userId)));
			}
			catch(ApplicationException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// [Auth=(admin)] Get information about all users
		/// </summary>
		/// <returns></returns>
		[Authorize(Roles = Role.Admin)]
		[HttpGet]
		[ProducesResponseType(typeof(UserAm[]), 200)]
		[ProducesResponseType(typeof(String), 400)]
		[ProducesResponseType(401)]
		[ProducesResponseType(403)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> GetAllUsers()
		{
			try
			{
				return Ok(_mapper.Map<UserAm[]>(await _userService.GetAllUsersAsync()));
			}
			catch(ApplicationException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// [Auth] Changing current user password
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Authorize]
		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(String), 400)]
		[ProducesResponseType(401)]
		[ProducesResponseType(typeof(String), 500)]
		public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordAm request)
		{
			try
			{
				if(!ModelState.IsValid)
					return BadRequest($"Please provide valid \"{nameof(request)}\"");

				var currentUserId = User.GetId();

				await _userService.UpdatePasswordAsync(currentUserId, request.OldPassword, request.NewPassword);
				_logger.LogInformation($"User with id {currentUserId} change its password");

				return Ok();
			}
			catch(ApplicationException ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}