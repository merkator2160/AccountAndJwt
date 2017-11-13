using AccountAndJwt.Database.Interfaces;
using AccountAndJwt.Database.Models;
using Microsoft.EntityFrameworkCore;
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
        public UserDb GetByLoginEager(String login)
        {
            return Context.Users
                .Include(p => p.UserRoles)
                .ThenInclude(p => p.Role)
                .FirstOrDefault(p => p.Login == login);
        }
        public UserDb GetByRefreshTokenEager(String refreshToken)
        {
            return Context.Users
                .Include(p => p.UserRoles)
                .ThenInclude(p => p.Role)
                .FirstOrDefault(p => p.RefreshToken == refreshToken);
        }
        public UserDb GetEager(Int32 id)
        {
            return Context.Users
                .Include(p => p.UserRoles)
                .ThenInclude(p => p.Role)
                .FirstOrDefault(p => p.Id == (Int32)id);
        }
        public UserDb[] GetAllEager()
        {
            return Context.Users
                .Include(p => p.UserRoles)
                .ThenInclude(p => p.Role)
                .ToArray();
        }
        public void AddRole(Int32 userId, Int32 roleId)
        {
            var userRole = new UserRoleDb()
            {
                UserId = userId,
                RoleId = roleId
            };
            Context.UserRoles.Add(userRole);
        }
        public void RemoveRole(Int32 userId, Int32 roleId)
        {
            var requestedUserRole = Context.UserRoles.First(p => p.RoleId == roleId && p.UserId == userId);
            Context.UserRoles.Remove(requestedUserRole);
        }
    }
}