using Autofac;
using Hangfire;
using Hangfire.Annotations;
using System;

namespace Wialon.Common.Hangfire.Activator
{
	public class AutofacJobActivator : JobActivator
	{
		public static readonly Object LifetimeScopeTag = "BackgroundJobScope";
		private readonly ILifetimeScope _lifetimeScope;
		private readonly Boolean _useTaggedLifetimeScope;


		public AutofacJobActivator([NotNull] ILifetimeScope lifetimeScope, Boolean useTaggedLifetimeScope = true)
		{
			_lifetimeScope = lifetimeScope ?? throw new ArgumentNullException(nameof(lifetimeScope));
			_useTaggedLifetimeScope = useTaggedLifetimeScope;
		}


		// FUNCTIONS //////////////////////////////////////////////////////////////////////////////
		public override Object ActivateJob(Type jobType)
		{
			return _lifetimeScope.Resolve(jobType);
		}
		public override JobActivatorScope BeginScope(JobActivatorContext context)
		{
			return new AutofacScope(_useTaggedLifetimeScope
				? _lifetimeScope.BeginLifetimeScope(LifetimeScopeTag)
				: _lifetimeScope.BeginLifetimeScope());
		}
	}
}