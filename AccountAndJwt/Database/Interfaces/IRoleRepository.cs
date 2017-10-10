using AccountAndJwt.Models.Database;
using System;

namespace AccountAndJwt.Database.Interfaces
{
    public interface IRoleRepository : IRepository<RoleDb>
    {
        RoleDb GetByNameEager(String roleName);
        RoleDb GetByName(String roleName);
    }
}