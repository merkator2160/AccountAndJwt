using AccountAndJwt.Models.Database;
using System;

namespace AccountAndJwt.Database.Interfaces
{
    public interface ITokenRepository : IRepository<RefreshTokenDb>
    {
        void AddOrUpdate(RefreshTokenDb refreshToken);
        RefreshTokenDb GetToken(String refreshToken);
        void ExpireAllUserRefreshTokens(Guid userId);
        void Delete(String refreshToken);
    }
}