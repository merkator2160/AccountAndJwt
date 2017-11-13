using AccountAndJwt.Database.Models;
using System;

namespace AccountAndJwt.Database.Interfaces
{
    public interface IRoleRepository : IRepository<RoleDb>
    {
        RoleDb GetByNameEager(String roleName);
        RoleDb GetByName(String roleName);
    }
}