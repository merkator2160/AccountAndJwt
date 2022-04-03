namespace AccountAndJwt.Common.Hangfire.Interfaces
{
	public interface IJob<T>
	{
		void Execute(T parameter);
	}
}