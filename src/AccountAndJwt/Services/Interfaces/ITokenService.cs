using AccountAndJwt.Api.Services.Models;
using System;
using System.Threading.Tasks;

namespace AccountAndJwt.Api.Services.Interfaces
{
	public interface ITokenService
	{
		Task<CreateAccessTokenByRefreshTokenDto> CreateAccessTokenByRefreshTokenAsync(String refreshToken);
		Task<CreateAccessTokenByCredentialsDto> CreateAccessTokenByCredentialsAsync(String login, String password);
		Task RevokeRefreshToken(String refreshToken);
	}
}