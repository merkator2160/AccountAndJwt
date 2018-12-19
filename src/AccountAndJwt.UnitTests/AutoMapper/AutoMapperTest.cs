﻿using AccountAndJwt.Api.Core.DependencyInjection;
using AutoMapper;
using Xunit;

namespace AccountAndJwt.UnitTests.AutoMapper
{
	public class AutoMapperTest
	{
		[Fact]
		public void AutoMapperConfigurationTest()
		{
			var mapperConfiguration = new MapperConfiguration(cfg =>
			{
				cfg.AddProfiles(Collector.LoadAssemblies("AccountAndJwt"));     // Dynamically load all configurations
			});

			mapperConfiguration.CompileMappings();
			mapperConfiguration.AssertConfigurationIsValid();
		}
	}
}