using Autofac;
using Hangfire;
using System;

namespace Wialon.Common.Hangfire.Activator
{
	public class AutofacScope : JobActivatorScope
	{
		private readonly ILifetimeScope _lifetimeScope;


		public AutofacScope(ILifetimeScope lifetimeScope)
		{
			_lifetimeScope = lifetimeScope;
		}


		// FUNCTIONS //////////////////////////////////////////////////////////////////////////
		public override Object Resolve(Type type)
		{
			return _lifetimeScope.Resolve(type);
		}
		public override void DisposeScope()
		{
			_lifetimeScope.Dispose();
		}
	}
}
