using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Linq;
using System.Reflection;

namespace AccountAndJwt.Api.Core.DependencyInjection
{
	internal static class Collector
	{
		public static void RegisterLocalServices(this ContainerBuilder builder)
		{
			builder.RegisterServices(Assembly.GetCallingAssembly());
		}
		public static void RegisterServices(this ContainerBuilder builder, Assembly assembly)
		{
			builder
				.RegisterAssemblyTypes(assembly)
				.Where(p => p.IsClass && p.Name.EndsWith("Service"))
				.AsSelf()
				.AsImplementedInterfaces();
		}
		public static void RegisterLocalConfiguration(this ContainerBuilder builder, IConfiguration configuration)
		{
			builder.RegisterConfiguration(configuration, Assembly.GetCallingAssembly());
		}
		public static void RegisterConfiguration(this ContainerBuilder builder, IConfiguration configuration, Assembly assembly)
		{
			var configTypes = assembly.DefinedTypes.Where(p => p.IsClass && p.Name.EndsWith("Config")).ToArray();
			foreach(var x in configTypes)
			{
				var configInstance = configuration.GetSection(x.Name).Get(x);
				if(configInstance != null)
				{
					builder.RegisterInstance(configInstance).AsSelf();
				}
			}
		}
		public static Assembly[] LoadAssemblies(String partOfName)
		{
			return DependencyContext.Default.CompileLibraries
				.Where(d => d.Name.Contains(partOfName))
				.Select(p => Assembly.Load(new AssemblyName(p.Name)))
				.ToArray();
		}
		public static Assembly[] LoadAllLocalAssemblies()
		{
			return DependencyContext.Default.CompileLibraries
				.Select(p => Assembly.Load(new AssemblyName(p.Name)))
				.ToArray();
		}
		public static Assembly GetAssembly(String name)
		{
			return Assembly.Load(new AssemblyName(DependencyContext.Default.CompileLibraries.First(p => p.Name.Equals(name)).Name));
		}
	}
}