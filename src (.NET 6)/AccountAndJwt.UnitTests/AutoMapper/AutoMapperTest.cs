using AccountAndJwt.Common.DependencyInjection;
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
				cfg.AddMaps(Collector.LoadAssemblies("AccountAndJwt"));
			});

			mapperConfiguration.CompileMappings();
			mapperConfiguration.AssertConfigurationIsValid();
		}
	}
}