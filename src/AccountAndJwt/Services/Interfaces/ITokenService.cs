using AccountAndJwt.Api.Services.Models;
using System;

namespace AccountAndJwt.Api.Services.Interfaces
{
	public interface ITokenService
	{
		CreateAccessTokenByRefreshTokenDto CreateAccessTokenByRefreshToken(String refreshToken);
		CreateAccessTokenByCredentialsDto CreateAccessTokenByCredentials(String login, String password);
		void RevokeRefreshToken(String refreshToken);
	}
}