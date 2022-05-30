using Autofac.Builder;
using Hangfire.Annotations;

namespace Hangfire.Activator.Extensions
{
    public static class RegistrationExtensions
    {
        public static IRegistrationBuilder<TLimit, TActivatorData, TStyle> InstancePerBackgroundJob<TLimit, TActivatorData, TStyle>(
            [NotNull] this IRegistrationBuilder<TLimit, TActivatorData, TStyle> registration)
        {
            return registration.InstancePerBackgroundJob(new Object[] { });
        }
        public static IRegistrationBuilder<TLimit, TActivatorData, TStyle> InstancePerBackgroundJob<TLimit, TActivatorData, TStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TStyle> registration,
            params Object[] lifetimeScopeTags)
        {
            var tags = new List<Object> { AutofacJobActivator.LifetimeScopeTag };
            tags.AddRange(lifetimeScopeTags);

            return registration.InstancePerMatchingLifetimeScope(tags.ToArray());
        }
    }
}