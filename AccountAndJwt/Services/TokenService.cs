using AccountAndJwt.Database.Interfaces;
using AccountAndJwt.Middleware;
using AccountAndJwt.Models.Config;
using AccountAndJwt.Models.Database;
using AccountAndJwt.Models.Exceptions;
using AccountAndJwt.Models.Service;
using AccountAndJwt.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace AccountAndJwt.Services
{
    internal class TokenService : ITokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AudienceConfig _audienceConfig;


        public TokenService(IOptions<AudienceConfig> audienceConfig, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _audienceConfig = audienceConfig.Value;
        }


        // ITokenService //////////////////////////////////////////////////////////////////////////
        public CreateAccessTokenByCredentialsDto CreateAccessTokenByCredentials(String login, String password)
        {
            var user = _unitOfWork.Users.GetByLoginEager(login);
            if (user == null)
                throw new UserNotFoundException($"User with login \"{login}\" was not found");

            if (!String.Equals(user.Password, password))
                throw new UserNotFoundException("There is no match Login and Password");


            var refreshTokenValue = Guid.NewGuid().ToString().Replace("-", "");

            user.RefreshToken = refreshTokenValue;
            _unitOfWork.Users.Update(user);
            _unitOfWork.Commit();

            return new CreateAccessTokenByCredentialsDto()
            {
                AccessToken = CreateJwtAccessToken(user),
                AccessTokenLifeTime = _audienceConfig.TokenLifetimeSec,
                RefreshToken = refreshTokenValue
            };
        }
        public CreateAccessTokenByRefreshTokenDto CreateAccessTokenByRefreshToken(String refreshToken)
        {
            var requestedUser = _unitOfWork.Users.GetByRefreshTokenEager(refreshToken);
            if (requestedUser == null)
                throw new RefreshTokenNotFoundException("Refresh token has expired");

            return new CreateAccessTokenByRefreshTokenDto()
            {
                AccessToken = CreateJwtAccessToken(requestedUser),
                AccessTokenLifeTime = _audienceConfig.TokenLifetimeSec
            };
        }
        public void RevokeRefreshToken(String refreshToken)
        {
            var requestedUser = _unitOfWork.Users.GetByRefreshTokenEager(refreshToken);
            if (requestedUser == null)
                throw new RefreshTokenNotFoundException("Refresh token has already expired");

            requestedUser.RefreshToken = null;
            _unitOfWork.Users.Update(requestedUser);
            _unitOfWork.Commit();
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
            claims.AddRange(user.UserRoles.Select(p => new Claim("roles", p.Role.RoleName)));

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