﻿using System.Threading.Tasks;

namespace DenverTraffic.Common.Hangfire.Interfaces
{
	public interface IJob<T>
	{
		Task Execute(T parameter);
	}
}