using AccountAndJwt.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace AccountAndJwt.Database
{
    internal class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }


        // ENTITIES ///////////////////////////////////////////////////////////////////////////////
        public DbSet<ValueDb> Values { get; set; }
        public DbSet<RefreshTokenDb> RefreshTokens { get; set; }
        public DbSet<UserDb> Users { get; set; }
    }
}