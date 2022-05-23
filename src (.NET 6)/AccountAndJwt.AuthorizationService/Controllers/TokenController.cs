using AccountAndJwt.AuthorizationService.Middleware;
using AccountAndJwt.AuthorizationService.Middleware.Config;
using AccountAndJwt.Common.Helpers;
using AccountAndJwt.Contracts.Models.Api.Errors;
using AccountAndJwt.Contracts.Models.Api.Request;
using AccountAndJwt.Contracts.Models.Api.Response;
using AccountAndJwt.Database.Interfaces;
using AccountAndJwt.Database.Models.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AccountAndJwt.AuthorizationService.Controllers
{
    /// <summary>
    /// Provides operations under JWT access tokens for the whole API
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TokenController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AudienceConfig _audienceConfig;


        public TokenController(IUnitOfWork unitOfWork, AudienceConfig audienceConfig)
        {
            _unitOfWork = unitOfWork;
            _audienceConfig = audienceConfig;
        }


        // ACTIONS ////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Creates the access-token by username and password
        /// </summary>
        /// <response code="460">Business logic validation exception</response>
        [HttpPost]
        [ProducesResponseType(typeof(AuthorizeResponseAm), 200)]
        [ProducesResponseType(typeof(ModelStateAm), 400)]
        [ProducesResponseType(typeof(ModelStateAm), 415)]
        [ProducesResponseType(typeof(String), 460)]
        [ProducesResponseType(typeof(String), 500)]
        public async Task<IActionResult> AuthorizeByCredentials([FromBody] AuthorizeRequestAm credentials)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _unitOfWork.Users.GetByLoginEagerAsync(credentials.Login);
            if (user == null)
                return StatusCode(460, $"User with {nameof(credentials.Login)}: \"{credentials.Login}\" was not found!");

            if (!String.Equals(user.PasswordHash, KeyHelper.CreatePasswordHash(credentials.Password, _audienceConfig.PasswordSalt)))
                return StatusCode(460, "There is no match login and password!");

            var refreshTokenValue = Guid.NewGuid().ToString().Replace("-", "");

            user.RefreshToken = refreshTokenValue;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.CommitAsync();

            return Ok(new AuthorizeResponseAm()
            {
                AccessToken = CreateJwtAccessToken(user),
                RefreshToken = refreshTokenValue
            });
        }

        /// <summary>
        /// Creates new access token by provided refresh token
        /// </summary>
        /// <response code="460">Business logic validation exception</response>
        [HttpPost]
        [ProducesResponseType(typeof(String), 200)]
        [ProducesResponseType(typeof(ModelStateAm), 400)]
        [ProducesResponseType(typeof(String), 460)]
        [ProducesResponseType(typeof(String), 500)]
        public async Task<IActionResult> RefreshToken([FromBody] String refreshToken)
        {
            var requestedUser = await _unitOfWork.Users.GetByRefreshTokenEagerAsync(refreshToken);
            if (requestedUser == null)
                return StatusCode(460, "Provided refresh token was not found!");

            return Ok(CreateJwtAccessToken(requestedUser));
        }

        /// <summary>
        /// Makes provided refresh token invalid
        /// </summary>
        /// <response code="460">Business logic validation exception</response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ModelStateAm), 400)]
        [ProducesResponseType(typeof(String), 460)]
        [ProducesResponseType(typeof(String), 500)]
        public async Task<IActionResult> RevokeToken([FromBody] String refreshToken)
        {
            var requestedUser = await _unitOfWork.Users.GetByRefreshTokenEagerAsync(refreshToken);
            if (requestedUser == null)
                return StatusCode(460, "Provided refresh token was not found!");

            requestedUser.RefreshToken = null;
            _unitOfWork.Users.Update(requestedUser);
            await _unitOfWork.CommitAsync();

            return Ok();
        }


        // FUNCTIONS /////////////////////////////////////////////////////////////////////////////
        private String CreateJwtAccessToken(UserDb user)
        {
            var currentDateUtc = DateTime.UtcNow;
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName)
            };
            claims.AddRange(user.UserRoles.Select(p => new Claim(ClaimTypes.Role, p.Role.Name)));

            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                issuer: _audienceConfig.ValidIssuer,
                audience: _audienceConfig.ValidAudience,
                claims: claims,
                notBefore: currentDateUtc,
                expires: currentDateUtc.Add(TimeSpan.FromSeconds(_audienceConfig.TokenLifetimeSec)),
                signingCredentials: new SigningCredentials(JwtAuthMiddleware.CreateSigningKey(_audienceConfig.Secret), SecurityAlgorithms.HmacSha256)));
        }
    }
}