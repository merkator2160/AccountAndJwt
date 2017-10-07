using AccountAndJwt.Models.Service;
using System;

namespace AccountAndJwt.Services.Interfaces
{
    public interface ITokenService
    {
        CreateAccessTokenByRefreshTokenDto CreateAccessTokenByRefreshToken(String refreshToken);
        CreateAccessTokenByCredentialsDto CreateAccessTokenByCredentials(String login, String password);
        void RevokeRefreshToken(String refreshToken);
    }
}