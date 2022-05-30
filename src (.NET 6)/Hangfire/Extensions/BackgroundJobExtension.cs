using Hangfire.Interfaces;

namespace Hangfire.Extensions
{
    public static class BackgroundJobExtension
    {
        public static void Enqueue<T>(this BackgroundJob job) where T : IJob
        {
            BackgroundJob.Enqueue<T>(p => p.Execute());
        }
    }
}