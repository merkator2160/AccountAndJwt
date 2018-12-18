﻿using AccountAndJwt.Api.Core.Consts;
using AccountAndJwt.Api.Database;
using AccountAndJwt.Api.Database.DependencyInjection;
using AccountAndJwt.Api.Middleware.Config;
using Autofac;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.IO;

namespace AccountAndJwt.Api
{
	public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
	{
		DataContext IDesignTimeDbContextFactory<DataContext>.CreateDbContext(String[] args)
		{
			var configuration = CustomConfigurationProvider.CreateConfiguration(ExecutingEnvironment.Development, Directory.GetCurrentDirectory());
			var builder = new ContainerBuilder();
			builder.RegisterModule(new DatabaseModule(configuration)
			{
				IsMigration = true
			});

			return builder.Build().Resolve<DataContext>();
		}
	}
}