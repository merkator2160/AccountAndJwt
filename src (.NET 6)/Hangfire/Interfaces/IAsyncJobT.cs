namespace Hangfire.Interfaces
{
    public interface IAsyncJob<T>
    {
        Task ExecuteAsync(T parameter);
    }
}