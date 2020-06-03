using Autofac.Core;
using Autofac.Core.Registration;
using NLog;
using System;
using System.Linq;
using System.Reflection;
using Module = Autofac.Module;

namespace AccountAndJwt.Common.DependencyInjection
{
	public class NLogModule : Module
	{
		protected override void AttachToComponentRegistration(IComponentRegistryBuilder componentRegistryBuilder, IComponentRegistration registration)
		{
			// Handle constructor parameters.
			registration.Preparing += OnComponentPreparing;

			// Handle properties.
			registration.Activated += (sender, e) => InjectLoggerProperties(e.Instance);
		}
		private static void InjectLoggerProperties(Object instance)
		{
			var instanceType = instance.GetType();

			// Get all the injectable properties to set.
			// If you wanted to ensure the properties were only UNSET properties,
			// here's where you'd do it.
			var properties = instanceType
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(p => p.PropertyType == typeof(ILogger) && p.CanWrite && p.GetIndexParameters().Length == 0);

			// Set the properties located.
			foreach(var propToSet in properties)
			{
				propToSet.SetValue(instance, LogManager.GetLogger(instanceType.FullName), null);
			}
		}
		private static void OnComponentPreparing(Object sender, PreparingEventArgs e)
		{
			e.Parameters = e.Parameters.Union(new[]
			{
				new ResolvedParameter(
					(p, i) => p.ParameterType == typeof (ILogger),
					(p, i) => LogManager.GetLogger(p.Member.DeclaringType.FullName))
			});
		}
	}
}