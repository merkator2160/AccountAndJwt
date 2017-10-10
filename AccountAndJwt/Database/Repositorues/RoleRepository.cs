using AccountAndJwt.Database.Interfaces;
using AccountAndJwt.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AccountAndJwt.Database.Repositorues
{
    internal class RoleRepository : EfRepositoryBase<RoleDb, DataContext>, IRoleRepository
    {
        public RoleRepository(DataContext context) : base(context)
        {

        }


        // IRoleRepository ////////////////////////////////////////////////////////////////////////
        public RoleDb GetByNameEager(String roleName)
        {
            return Context.Roles
                .Include(p => p.UserRoles)
                .ThenInclude(p => p.User)
                .FirstOrDefault(p => p.RoleName == roleName);
        }
        public RoleDb GetByName(String roleName)
        {
            return Context.Roles.FirstOrDefault(p => p.RoleName == roleName);
        }
    }
}