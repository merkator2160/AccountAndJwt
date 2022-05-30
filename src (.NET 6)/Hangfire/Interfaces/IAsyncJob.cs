namespace Hangfire.Interfaces
{
    public interface IAsyncJob
    {
        Task ExecuteAsync();
    }
}