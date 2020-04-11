using AccountAndJwt.Common.Config;
using AccountAndJwt.Common.Consts;
using AccountAndJwt.Database;
using AccountAndJwt.Database.DependencyInjection;
using Autofac;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.IO;

namespace AccountAndJwt.AuthorizationService
{
	public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
	{
		DataContext IDesignTimeDbContextFactory<DataContext>.CreateDbContext(String[] args)
		{
			var configuration = CustomConfigurationProvider.CreateConfiguration(HostingEnvironment.Development, Directory.GetCurrentDirectory());
			var builder = new ContainerBuilder();
			builder.RegisterModule(new DatabaseModule(configuration)
			{
				IsMigration = true
			});

			return builder.Build().Resolve<DataContext>();
		}
	}
}