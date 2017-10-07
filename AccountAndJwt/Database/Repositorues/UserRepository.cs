using AccountAndJwt.Database.Interfaces;
using AccountAndJwt.Models.Database;
using System;
using System.Linq;

namespace AccountAndJwt.Database.Repositorues
{
    internal class UserRepository : EfRepositoryBase<UserDb, DataContext>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {

        }


        // IUserRepository ////////////////////////////////////////////////////////////////////////
        public UserDb GetByLogin(String login)
        {
            return Context.Users.FirstOrDefault(p => p.Login == login);
        }
    }
}