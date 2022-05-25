using AccountAndJwt.Common.Config;
using AccountAndJwt.Contracts.Const;
using AccountAndJwt.Database;
using AccountAndJwt.Database.DependencyInjection;
using Microsoft.EntityFrameworkCore.Design;

namespace AccountAndJwt.AuthorizationService
{
    public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        DataContext IDesignTimeDbContextFactory<DataContext>.CreateDbContext(String[] args)
        {
            var configuration = CustomConfigurationProvider.CreateConfiguration(HostingEnvironment.Development, Directory.GetCurrentDirectory());

            return DatabaseModule.CreateMigrationContext(configuration);
        }
    }
}