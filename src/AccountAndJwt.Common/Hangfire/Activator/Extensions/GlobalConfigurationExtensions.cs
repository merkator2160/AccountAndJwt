using Autofac;
using Hangfire;
using System;

namespace Wialon.Common.Hangfire.Activator.Extensions
{
	public static class GlobalConfigurationExtensions
	{
		public static IGlobalConfiguration<AutofacJobActivator> UseAutofacActivator(
			this IGlobalConfiguration configuration,
			ILifetimeScope lifetimeScope, Boolean useTaggedLifetimeScope = true)
		{
			return configuration.UseActivator(new AutofacJobActivator(lifetimeScope, useTaggedLifetimeScope));
		}
	}
}
