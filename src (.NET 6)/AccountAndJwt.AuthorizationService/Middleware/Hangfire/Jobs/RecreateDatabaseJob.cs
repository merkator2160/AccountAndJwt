using AccountAndJwt.AuthorizationService.Database.DependencyInjection;
using Hangfire;
using Hangfire.Interfaces;

namespace AccountAndJwt.AuthorizationService.Middleware.Hangfire.Jobs
{
    internal class RecreateDatabaseJob : IJob, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RecreateDatabaseJob> _logger;

        private readonly Boolean _isMutexFree;
        private readonly Mutex _mutex;


        public RecreateDatabaseJob(IConfiguration configuration, ILogger<RecreateDatabaseJob> logger)
        {
            _configuration = configuration;
            _logger = logger;

            _mutex = new Mutex(true, nameof(RecreateDatabaseJob), out _isMutexFree);
        }


        // IJob ///////////////////////////////////////////////////////////////////////////////////
        [AutomaticRetry(Attempts = 0)]
        public void Execute()
        {
            try
            {
                if (!_isMutexFree)
                    return;

                DatabaseModule.CheckDatabase(_configuration, DatabaseModule.DropCreateInitializeStrategy);
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