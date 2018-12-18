using AccountAndJwt.Api.Middleware.AutoMapper;
using AutoMapper;
using System.Reflection;
using Xunit;

namespace AccountAndJwt.UnitTests.AutoMapper
{
	public class AutoMapperTest
	{
		[Fact]
		public void TheWholeAutoMapperConfigurationTest()
		{
			var mapperConfiguration = new MapperConfiguration(cfg =>
			{
				cfg.AddProfiles(typeof(AutoMapperModule).GetTypeInfo().Assembly);     // Dynamically load all configurations
			});

			mapperConfiguration.CompileMappings();
			mapperConfiguration.AssertConfigurationIsValid();
		}
	}
}