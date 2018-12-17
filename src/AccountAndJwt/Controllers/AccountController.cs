using AccountAndJwt.Contracts.Models;
using AccountAndJwt.Services.Interfaces;
using AccountAndJwt.Services.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AccountAndJwt.Controllers
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
        public IActionResult Register([FromBody]RegisterUserAm userDetails)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest($"Please provide valid \"{nameof(userDetails)}\"");

                _userService.Register(_mapper.Map<UserDto>(userDetails));
                _logger.LogInformation($"New user registered {userDetails.Login}");

                return Ok(new RegisterUserResponseAm()
                {
                    GetAccessTokenUrl = _urlHelper.Action("AuthorizeByCredentials", "Token"),
                    RefreshAccessTokenUrl = _urlHelper.Action("RefreshToken", "Token")
                });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete existed account by it id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(String), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult DeleteAccount([FromBody]Int32 userId)
        {
            try
            {
                if (userId == 0)
                    return BadRequest($"Please provide \"{nameof(userId)}\"");

                _logger.LogInformation($"User with id {userId} deleted");
                _userService.DeleteUser(userId);

                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Add role to user with specified id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(String), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult AddRole([FromBody]AddRemoveRoleAm request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest($"Please provide valid \"{nameof(request)}\"");

                _userService.AddRole(request.UserId, request.Role);
                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete role from user with specified id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(String), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult RemoveRole([FromBody]AddRemoveRoleAm request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest($"Please provide valid \"{nameof(request)}\"");

                _userService.RemoveRole(request.UserId, request.Role);
                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Channge E-Mail of current user
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
                if (String.IsNullOrEmpty(newEmail))
                    return BadRequest($"Please provide valid \"{nameof(newEmail)}\"");

                var currentUserId = GetCurrentUserId();

                await _userService.ChangeEmailAsync(currentUserId, newEmail);
                _logger.LogInformation($"Email of user {currentUserId} updated");

                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Change current user First and Last name by provided id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(String), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult ChangeName([FromBody]ChangeNameRequestAm request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest($"Please provide valid \"{nameof(request)}\"");

                var currentUserId = GetCurrentUserId();

                _userService.ChangeName(currentUserId, request.FirstName, request.LastName);
                _logger.LogInformation($"Email of user {currentUserId} updated");

                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get current user associated data 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(GetClaimsResponseAm[]), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult GetCurrentUserClaims()
        {
            var claims = HttpContext.User.Claims.ToArray().Select(x => new GetClaimsResponseAm
            {
                ClaimType = x.Type,
                Value = x.Value
            });
            return Ok(claims);
        }

        /// <summary>
        /// Get information about user by provided id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(UserAm), 200)]
        [ProducesResponseType(typeof(String), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult GetUser(Int32 userId)
        {
            try
            {
                return Ok(_mapper.Map<UserAm>(_userService.GetUser(userId)));
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get information about all users
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(UserAm[]), 200)]
        [ProducesResponseType(typeof(String), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult GetAllUsers()
        {
            try
            {
                return Ok(_mapper.Map<UserAm[]>(_userService.GetAllUsers()));
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Changing current user password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(String), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult ResetPassword([FromBody]ResetPasswordAm request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest($"Please provide valid \"{nameof(request)}\"");

                var currentUserId = GetCurrentUserId();

                _userService.UpdatePassword(currentUserId, request.OldPassword, request.NewPassword);
                _logger.LogInformation($"User with id {currentUserId} change its password");

                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // SUPPORT FUNCTIONS //////////////////////////////////////////////////////////////////////
        private Int32 GetCurrentUserId()
        {
            return Int32.Parse(User.Claims.ToArray().First(p => p.Type.Contains("nameidentifier")).Value);
        }
    }
}