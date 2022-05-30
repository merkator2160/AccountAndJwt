using AccountAndJwt.AuthorizationService.Database;
using AccountAndJwt.AuthorizationService.Database.DependencyInjection;
using AccountAndJwt.Contracts.Const;
using CustomConfiguration;
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