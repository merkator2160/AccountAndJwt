using AccountAndJwt.Models.Database;
using System;

namespace AccountAndJwt.Database.Interfaces
{
    public interface IUserRepository : IRepository<UserDb>
    {
        UserDb GetByLogin(String login);
    }
}