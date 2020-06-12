using AccountAndJwt.AuthorizationService.Middleware;
using AccountAndJwt.AuthorizationService.Middleware.Config;
using AccountAndJwt.AuthorizationService.Services.Interfaces;
using AccountAndJwt.AuthorizationService.Services.Models;
using AccountAndJwt.AuthorizationService.Services.Models.Exceptions;
using AccountAndJwt.Common.Helpers;
using AccountAndJwt.Database.Interfaces;
using AccountAndJwt.Database.Models.Storage;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AccountAndJwt.AuthorizationService.Services
{
	internal sealed class TokenService : ITokenService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly AudienceConfig _audienceConfig;


		public TokenService(AudienceConfig audienceConfig, IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_audienceConfig = audienceConfig;
		}


		// ITokenService //////////////////////////////////////////////////////////////////////////
		public async Task<CreateAccessTokenByCredentialsDto> CreateAccessTokenByCredentialsAsync(String login, String password)
		{
			var user = await _unitOfWork.Users.GetByLoginEagerAsync(login);
			if(user == null)
				throw new UserNotFoundException($"User with login \"{login}\" was not found");

			if(!String.Equals(user.PasswordHash, KeyHelper.CreatePasswordHash(password, _audienceConfig.PasswordSalt)))
				throw new UserNotFoundException("There is no match Login and Password");

			var refreshTokenValue = Guid.NewGuid().ToString().Replace("-", "");

			user.RefreshToken = refreshTokenValue;
			_unitOfWork.Users.Update(user);
			await _unitOfWork.CommitAsync();

			return new CreateAccessTokenByCredentialsDto()
			{
				AccessToken = CreateJwtAccessToken(user),
				AccessTokenLifeTime = _audienceConfig.TokenLifetimeSec,
				RefreshToken = refreshTokenValue
			};
		}
		public async Task<CreateAccessTokenByRefreshTokenDto> CreateAccessTokenByRefreshTokenAsync(String refreshToken)
		{
			var requestedUser = await _unitOfWork.Users.GetByRefreshTokenEagerAsync(refreshToken);
			if(requestedUser == null)
				throw new RefreshTokenNotFoundException("Refresh token has expired");

			return new CreateAccessTokenByRefreshTokenDto()
			{
				AccessToken = CreateJwtAccessToken(requestedUser),
				AccessTokenLifeTime = _audienceConfig.TokenLifetimeSec
			};
		}
		public async Task RevokeRefreshToken(String refreshToken)
		{
			var requestedUser = await _unitOfWork.Users.GetByRefreshTokenEagerAsync(refreshToken);
			if(requestedUser == null)
				throw new RefreshTokenNotFoundException("Refresh token has already expired");

			requestedUser.RefreshToken = null;
			_unitOfWork.Users.Update(requestedUser);
			await _unitOfWork.CommitAsync();
		}


		// FUNCTIONS /////////////////////////////////////////////////////////////////////////////
		private String CreateJwtAccessToken(UserDb user)
		{
			var currentDateUtc = DateTime.UtcNow;
			var claims = new List<Claim>()
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Iat, currentDateUtc.ToUniversalTime().ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(nameof(user.FirstName), user.FirstName),
				new Claim(nameof(user.LastName), user.LastName),
				new Claim("custom", "customValue")
			};
			claims.AddRange(user.UserRoles.Select(p => new Claim("roles", p.Role.Name)));

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