using AccountAndJwt.AuthorizationService.Middleware;
using AccountAndJwt.AuthorizationService.Middleware.Config;
using AccountAndJwt.AuthorizationService.Services.Interfaces;
using AccountAndJwt.AuthorizationService.Services.Models;
using AccountAndJwt.Common.Helpers;
using AccountAndJwt.Contracts.Const;
using AccountAndJwt.Contracts.Models.Api;
using AccountAndJwt.Contracts.Models.Api.Errors;
using AccountAndJwt.Contracts.Models.Api.Request;
using AccountAndJwt.Contracts.Models.Api.Response;
using AccountAndJwt.Database.Interfaces;
using AccountAndJwt.Database.Models.Storage;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly AudienceConfig _audienceConfig;
        private readonly IUrlHelper _urlHelper;


        public AccountController(IUrlHelper urlHelper, IMapper mapper, IUnitOfWork unitOfWork, IEmailService emailService, AudienceConfig audienceConfig)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _audienceConfig = audienceConfig;
            _urlHelper = urlHelper;
        }


        // ACTIONS ////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Register new user by provided credentials
        /// </summary>
        /// <response code="460">Business logic validation exception</response>
        [HttpPost]
        [ProducesResponseType(typeof(RegisterUserResponseAm), 201)]
        [ProducesResponseType(typeof(ModelStateAm), 400)]
        [ProducesResponseType(typeof(String), 460)]
        [ProducesResponseType(typeof(String), 500)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestAm userDetails)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _unitOfWork.Users.UserExistsAsync(userDetails.Login))
                StatusCode(460, "User name is already occupied!");

            var userDb = _mapper.Map<UserDb>(userDetails);
            userDb.PasswordHash = KeyHelper.CreatePasswordHash(userDetails.Password, _audienceConfig.PasswordSalt);

            await _unitOfWork.Users.AddAsync(userDb);
            await _unitOfWork.CommitAsync();

            return Ok(new RegisterUserResponseAm()
            {
                AccessTokenUrl = _urlHelper.Action("AuthorizeByCredentials", "Token"),
                RefreshAccessTokenUrl = _urlHelper.Action("RefreshToken", "Token"),
                RevokeRefreshTokenUrl = _urlHelper.Action("RevokeToken", "Token")
            });
        }

        /// <summary>
        /// [Auth=(admin)] Delete existed account by it id
        /// </summary>
        /// <response code="460">Business logic validation exception</response>
        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ModelStateAm), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(String), 460)]
        [ProducesResponseType(typeof(String), 500)]
        public async Task<IActionResult> DeleteAccount([FromBody] Int32 userId)
        {
            if (userId == 0)
                return StatusCode(460, $"Please provide \"{nameof(userId)}\"");

            var requestedUser = await _unitOfWork.Users.GetEagerAsync(userId);
            if (requestedUser == null)
                StatusCode(460, $"User with provided id: \"{userId}\" was not found!");

            _unitOfWork.Users.Remove(requestedUser);
            await _unitOfWork.CommitAsync();

            return Ok();
        }

        /// <summary>
        /// [Auth=(admin)] Returns all available roles
        /// </summary>
        /// <response code="460">Business logic validation exception</response>
        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        [ProducesResponseType(typeof(RoleAm), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(String), 460)]
        [ProducesResponseType(typeof(String), 500)]
        public async Task<IActionResult> GetAvailableRoles()
        {
            return Ok(_mapper.Map<RoleAm[]>(await _unitOfWork.Users.GetAvailableRolesAsync()));
        }

        /// <summary>
        /// [Auth=(admin)] Add role to user with specified id
        /// </summary>
        /// <response code="460">Business logic validation exception</response>
        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ModelStateAm), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(String), 460)]
        [ProducesResponseType(typeof(String), 500)]
        public async Task<IActionResult> AddUserRole([FromBody] AddRemoveUserRoleRequestAm request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _unitOfWork.Users.UserRoleExistsAsync(request.UserId, request.RoleId))
                return StatusCode(460, $"User with id: \"{request.UserId}\", already has role with id: \"{request.RoleId}\"!");

            await _unitOfWork.Users.AddUserRoleAsync(request.UserId, request.RoleId);
            await _unitOfWork.CommitAsync();

            return Ok();
        }

        /// <summary>
        /// [Auth=(admin)] Delete role from user with specified id
        /// </summary>
        /// <response code="460">Business logic validation exception</response>
        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ModelStateAm), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(String), 460)]
        [ProducesResponseType(typeof(String), 500)]
        public async Task<IActionResult> RemoveUserRole([FromBody] AddRemoveUserRoleRequestAm request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _unitOfWork.Users.UserRoleExistsAsync(request.UserId, request.RoleId))
                return StatusCode(460, $"User with id: \"{request.UserId}\", has no role with id: \"{request.RoleId}\"!");

            await _unitOfWork.Users.DeleteUserRoleAsync(request.UserId, request.RoleId);
            await _unitOfWork.CommitAsync();

            return Ok();
        }

        /// <summary>
        /// [Auth] Change E-Mail of current user
        /// </summary>
        /// <response code="460">Business logic validation exception</response>
        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ModelStateAm), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(String), 460)]
        [ProducesResponseType(typeof(String), 500)]
        public async Task<IActionResult> ChangeEmail([FromBody] String newEmail)
        {
            if (String.IsNullOrEmpty(newEmail))
                return StatusCode(460, $"Please provide valid \"{nameof(newEmail)}\"");

            var userId = User.GetId();
            var requestedUser = await _unitOfWork.Users.GetEagerAsync(userId);
            if (requestedUser == null)
                return StatusCode(460, $"User with provided id: \"{userId}\" was not found!");

            requestedUser.Email = newEmail;

            _unitOfWork.Users.Update(requestedUser);
            await _unitOfWork.CommitAsync();

            await _emailService.SendAsync(new EmailMessage()
            {
                Destination = requestedUser.Email,
                Subject = "Email successfully changed",
                Body = $"New Email {requestedUser.Email}"
            });

            return Ok();
        }

        /// <summary>
        /// [Auth] Change current user First and Last name by provided id
        /// </summary>
        /// <response code="460">Business logic validation exception</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(String), 460)]
        [ProducesResponseType(typeof(String), 500)]
        public async Task<IActionResult> ChangeName([FromBody] ChangeNameRequestAm request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.GetId();
            var requestedUser = await _unitOfWork.Users.GetEagerAsync(userId);
            if (requestedUser == null)
                return StatusCode(460, $"User with provided id: \"{userId}\" was not found!");

            requestedUser.FirstName = request.FirstName;
            requestedUser.LastName = request.LastName;

            _unitOfWork.Users.Update(requestedUser);
            await _unitOfWork.CommitAsync();

            return Ok();
        }

        /// <summary>
        /// [Auth=(admin)] Get information about user by provided id
        /// </summary>
        /// <response code="460">Business logic validation exception</response>
        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        [ProducesResponseType(typeof(UserAm), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(String), 460)]
        [ProducesResponseType(typeof(String), 500)]
        public async Task<IActionResult> GetUser([FromQuery] Int32 userId)
        {
            var requestedUser = await _unitOfWork.Users.GetEagerAsync(userId);
            if (requestedUser == null)
                return StatusCode(460, $"User with provided id: \"{userId}\" was not found!");

            return Ok(_mapper.Map<UserAm>(requestedUser));
        }

        /// <summary>
        /// [Auth=(admin)] Returns information about some amount of users, paged
        /// </summary>
        /// <response code="460">Business logic validation exception</response>
        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        [ProducesResponseType(typeof(PagedUserResponseAm), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(String), 460)]
        [ProducesResponseType(typeof(String), 500)]
        public async Task<IActionResult> GetUsersPaged([FromBody] GetUsersPagedRequestAm request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(_mapper.Map<PagedUserResponseAm>(await _unitOfWork.Users.GetPagedEagerAsync(request.PageSize, request.PageNumber)));
        }

        /// <summary>
        /// [Auth] Changing current user password
        /// </summary>
        /// <response code="460">Business logic validation exception</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ModelStateAm), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(String), 460)]
        [ProducesResponseType(typeof(String), 500)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestAm request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.GetId();
            var requestedUser = await _unitOfWork.Users.GetAsync(userId);
            if (requestedUser == null)
                return StatusCode(460, $"User with provided id: \"{userId}\" was not found!");

            var providedOldPasswordHash = KeyHelper.CreatePasswordHash(request.OldPassword, _audienceConfig.PasswordSalt);
            if (!String.Equals(providedOldPasswordHash, requestedUser.PasswordHash))
                return StatusCode(460, $"The {nameof(request.OldPassword)} is wrong!");

            requestedUser.PasswordHash = KeyHelper.CreatePasswordHash(request.NewPassword, _audienceConfig.PasswordSalt);
            _unitOfWork.Users.Update(requestedUser);
            await _unitOfWork.CommitAsync();

            return Ok();
        }
    }
}