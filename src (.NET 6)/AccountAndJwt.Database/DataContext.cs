using AccountAndJwt.Common.Extensions;
using AccountAndJwt.Database.Models.Storage;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AccountAndJwt.Database
{
    // TODO: Implement fluent migrations: https://fluentmigrator.github.io/
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
            Initialize();
        }


        // ENTITIES ///////////////////////////////////////////////////////////////////////////////
        public DbSet<ValueDb> Values { get; set; }
        public DbSet<UserDb> Users { get; set; }
        public DbSet<RoleDb> Roles { get; set; }
        public DbSet<UserRoleDb> UserRoles { get; set; }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        private void Initialize()
        {
            ChangeTracker.AutoDetectChangesEnabled = false;                             // manual changes tracking, increasing working speed 4x times
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;     // Equivalent of .AsNoTracking() for each select query
            Database.AutoTransactionsEnabled = true;                                    // Required for "Unit of work pattern"
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.CollectMappings(Assembly.GetExecutingAssembly());

            // ...or do it manually below. For example,
            // modelBuilder.AddConfiguration(new ParkingTerritoryMap());

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                base.OnConfiguring(optionsBuilder);
            }
        }
    }
}