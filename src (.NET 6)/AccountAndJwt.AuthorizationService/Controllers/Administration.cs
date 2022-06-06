using AccountAndJwt.AuthorizationService.Database.Interfaces;
using AccountAndJwt.Contracts.Const;
using AccountAndJwt.Contracts.Models.Api;
using AccountAndJwt.Contracts.Models.Api.Errors;
using AccountAndJwt.Contracts.Models.Api.Request;
using AccountAndJwt.Contracts.Models.Api.Response;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountAndJwt.AuthorizationService.Controllers
{
    /// <summary>
    /// Provide account moderation activities
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AdministrationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;


        public AdministrationController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        // ACTIONS ////////////////////////////////////////////////////////////////////////////////

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
        /// [Auth=(admin)] Delete existed account by it id
        /// </summary>
        /// <response code="460">Business logic validation exception</response>
        [HttpDelete]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ModelStateAm), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(String), 460)]
        [ProducesResponseType(typeof(String), 500)]
        public async Task<IActionResult> DeleteAccount([FromQuery] Int32 userId)
        {
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
        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(typeof(RoleAm[]), 200)]
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
    }
}