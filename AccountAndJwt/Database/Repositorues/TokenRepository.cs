using AccountAndJwt.Database.Interfaces;
using AccountAndJwt.Models.Database;
using System;
using System.Linq;

namespace AccountAndJwt.Database.Repositorues
{
    internal class TokenRepository : EfRepositoryBase<RefreshTokenDb, DataContext>, ITokenRepository
    {

        public TokenRepository(DataContext context) : base(context)
        {

        }


        // ITokenRepository ///////////////////////////////////////////////////////////////////////
        public void AddOrUpdate(RefreshTokenDb refreshToken)
        {
            var requestedToken = Context.RefreshTokens.FirstOrDefault(p => p.Id == refreshToken.Id);
            if (requestedToken != null)
            {
                Context.RefreshTokens.Remove(requestedToken);
            }

            Context.RefreshTokens.Add(refreshToken);
        }
        public RefreshTokenDb GetToken(String refreshToken)
        {
            return Context.RefreshTokens.FirstOrDefault(x => String.Equals(x.Value, refreshToken));
        }
        public void ExpireAllUserRefreshTokens(Guid userId)
        {
            var allUserRefreshTokens = Context.RefreshTokens.Where(p => p.ClientId == userId).ToArray();
            Context.RefreshTokens.RemoveRange(allUserRefreshTokens);
        }
        public void Delete(String refreshToken)
        {
            var requestedToken = Context.RefreshTokens.FirstOrDefault(x => String.Equals(x.Value, refreshToken));
            if (requestedToken != null)
                Context.RefreshTokens.Remove(requestedToken);
        }
    }
}