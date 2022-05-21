using AccountAndJwt.Common.Hangfire.Interfaces;
using Hangfire;

namespace AccountAndJwt.AuthorizationService.Middleware.Hangfire.Jobs
{
    internal class SampleJob : IJob, IDisposable
    {
        private readonly ILogger<SampleJob> _logger;

        private readonly Boolean _isMutexFree;
        private readonly Mutex _mutex;


        public SampleJob(ILogger<SampleJob> logger)
        {
            _logger = logger;

            _mutex = new Mutex(true, nameof(SampleJob), out _isMutexFree);
        }


        // IJob ///////////////////////////////////////////////////////////////////////////////////
        [AutomaticRetry(Attempts = 0)]
        public void Execute()
        {
            try
            {
                if (!_isMutexFree)
                    return;

                Console.WriteLine($"{nameof(SampleJob)} is executing!");

                Thread.Sleep(1000);
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