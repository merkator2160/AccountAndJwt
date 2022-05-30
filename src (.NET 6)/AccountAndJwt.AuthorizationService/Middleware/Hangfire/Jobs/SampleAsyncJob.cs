using Hangfire;
using Hangfire.Interfaces;

namespace AccountAndJwt.AuthorizationService.Middleware.Hangfire.Jobs
{
    internal class SampleAsyncJob : IAsyncJob, IDisposable
    {
        private readonly ILogger<SampleAsyncJob> _logger;

        private readonly Boolean _isMutexFree;
        private readonly Mutex _mutex;


        public SampleAsyncJob(ILogger<SampleAsyncJob> logger)
        {
            _logger = logger;

            _mutex = new Mutex(true, nameof(SampleAsyncJob), out _isMutexFree);
        }


        // IJob ///////////////////////////////////////////////////////////////////////////////////
        [AutomaticRetry(Attempts = 0)]
        public async Task ExecuteAsync()
        {
            try
            {
                if (!_isMutexFree)
                    return;

                Console.WriteLine($"{nameof(SampleAsyncJob)} is executing!");

                await Task.Delay(1000);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}\r\n{ex.StackTrace}");
                throw;
            }
        }


        // IDisposable ////////////////////////////////////////////////////////////////////////////
        public void Dispose()
        {
            _mutex?.Dispose();
        }
    }
}