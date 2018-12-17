using AccountAndJwt.Api.Middleware.DependencyInjection;
using Autofac;
using AutoMapper;
using Module = Autofac.Module;

namespace AccountAndJwt.Api.Middleware.AutoMapper
{
	internal class AutoMapperModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterInstance(GetConfiguredMapper());
		}
		private static IMapper GetConfiguredMapper()
		{
			var mapperConfiguration = new MapperConfiguration(RegisterMappings);
			mapperConfiguration.CompileMappings();
			return mapperConfiguration.CreateMapper();
		}
		private static void RegisterMappings(IMapperConfigurationExpression configure)
		{
			configure.AddProfiles(Collector.LoadAllLocalAssemblies());     // Dynamically load all configurations

			// ...or do it manually below. Example: https://github.com/AutoMapper/AutoMapper/wiki/Configuration
			// ...or see examples in Profiles directory.
		}
	}
}