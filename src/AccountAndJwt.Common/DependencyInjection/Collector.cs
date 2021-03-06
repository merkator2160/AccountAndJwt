﻿using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Linq;
using System.Reflection;

namespace AccountAndJwt.Common.DependencyInjection
{
	public static class Collector
	{
		public static void RegisterLocalServices(this ContainerBuilder builder)
		{
			builder.RegisterServices(Assembly.GetCallingAssembly());
		}
		public static void RegisterServices(this ContainerBuilder builder, Assembly[] assembliesToScan)
		{
			foreach(var assembly in assembliesToScan)
			{
				builder.RegisterServices(assembly);
			}
		}
		public static void RegisterServices(this ContainerBuilder builder, Assembly assemblyToScan)
		{
			builder
				.RegisterAssemblyTypes(assemblyToScan)
				.Where(p => p.IsClass && p.Name.EndsWith("Service"))
				.AsSelf()
				.AsImplementedInterfaces();
		}
		public static void RegisterLocalHangfireJobs(this ContainerBuilder builder)
		{
			builder.RegisterHangfireJobs(Assembly.GetCallingAssembly());
		}
		public static void RegisterHangfireJobs(this ContainerBuilder builder, Assembly assemblyToScan)
		{
			builder
				.RegisterAssemblyTypes(assemblyToScan)
				.Where(p => p.IsClass && p.Name.EndsWith("Job"))
				.AsSelf()
				.AsImplementedInterfaces();
		}
		public static void RegisterServiceConfiguration(this ContainerBuilder builder, IConfiguration configuration, Assembly[] assembliesToScan)
		{
			foreach(var assembly in assembliesToScan)
			{
				builder.RegisterServiceConfiguration(configuration, assembly);
			}
		}
		public static void RegisterLocalConfiguration(this ContainerBuilder builder, IConfiguration configuration)
		{
			builder.RegisterServiceConfiguration(configuration, Assembly.GetCallingAssembly());
		}
		public static void RegisterServiceConfiguration(this ContainerBuilder builder, IConfiguration configuration, Assembly assemblyToScan)
		{
			var configTypes = assemblyToScan.DefinedTypes.Where(p => p.IsClass && p.Name.EndsWith("Config")).ToArray();
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
		public static Assembly GetAssembly(String name)
		{
			return Assembly.Load(new AssemblyName(DependencyContext.Default.CompileLibraries.First(p => p.Name.Equals(name)).Name));
		}
	}
}