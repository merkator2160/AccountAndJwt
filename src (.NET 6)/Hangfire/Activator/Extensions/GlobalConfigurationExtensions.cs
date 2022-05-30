using Autofac;

namespace Hangfire.Activator.Extensions
{
    public static class GlobalConfigurationExtensions
    {
        public static IGlobalConfiguration<AutofacJobActivator> UseAutofacActivator(this IGlobalConfiguration configuration, ILifetimeScope lifetimeScope, Boolean useTaggedLifetimeScope = true)
        {
            return configuration.UseActivator(new AutofacJobActivator(lifetimeScope, useTaggedLifetimeScope));
        }
    }
}
