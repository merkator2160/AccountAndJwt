using AccountAndJwt.AuthorizationService.Middleware.Hangfire.Models;
using AccountAndJwt.Common.Hangfire.Interfaces;
using Hangfire;

namespace AccountAndJwt.AuthorizationService.Middleware.Hangfire.Jobs
{
    internal class SampleParametrizedJob : IJob<SampleJobParameter>, IDisposable
    {
        private readonly ILogger _logger;

        private readonly Boolean _isMutexFree;
        private readonly Mutex _mutex;


        public SampleParametrizedJob(ILogger logger)
        {
            _logger = logger;

            _mutex = new Mutex(true, nameof(SampleParametrizedJob), out _isMutexFree);
        }


        // IJob ///////////////////////////////////////////////////////////////////////////////////
        [AutomaticRetry(Attempts = 0)]
        public void Execute(SampleJobParameter parameter)
        {
            try
            {
                if (!_isMutexFree)
                    return;

                Console.WriteLine($"{nameof(SampleParametrizedJob)} is executing, parameter: {parameter.Parameter}");

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