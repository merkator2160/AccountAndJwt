using Autofac;
using AutoMapper;
using System.Reflection;
using Module = Autofac.Module;

namespace AccountAndJwt.Common.DependencyInjection
{
	public class AutoMapperModule : Module
	{
		private readonly Assembly[] _assembliesToScan;


		public AutoMapperModule(Assembly[] assembliesToScan)
		{
			_assembliesToScan = assembliesToScan;
		}


		// FUNCTIONS //////////////////////////////////////////////////////////////////////////////
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterInstance(GetConfiguredMapper());
		}
		private IMapper GetConfiguredMapper()
		{
			var mapperConfiguration = new MapperConfiguration(RegisterMappings);
			mapperConfiguration.CompileMappings();
			return mapperConfiguration.CreateMapper();
		}
		private void RegisterMappings(IMapperConfigurationExpression configure)
		{
			configure.AddMaps(_assembliesToScan);     // Dynamically load all configurations

			// ...or do it manually below. Example: https://github.com/AutoMapper/AutoMapper/wiki/Configuration
			// ...or see examples in Profiles directory.
		}
	}
}