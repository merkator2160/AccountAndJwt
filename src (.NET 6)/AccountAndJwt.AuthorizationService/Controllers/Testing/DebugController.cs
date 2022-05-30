using AccountAndJwt.AuthorizationService.Database;
using AccountAndJwt.AuthorizationService.Database.Interfaces;
using AutoMapper;
using CustomConfiguration;
using Microsoft.AspNetCore.Mvc;

#if DEBUG
namespace AccountAndJwt.AuthorizationService.Controllers.Testing
{
    /// <summary>
    /// This controller only for debugging, playing, testing purposes (it does not appear in production)
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DebugController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<DebugController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DataContext _context;


        public DebugController(
            IWebHostEnvironment env,
            ILogger<DebugController> logger,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            DataContext context)
        {
            _env = env;
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _context = context;
        }


        // ACTIONS //////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Creates a log entry
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult WriteLog(String message)
        {
            _logger.LogError($"Test error log: {message}");
            _logger.LogDebug($"Test debug log: {message}");

            return Ok();
        }

        /// <summary>
        /// Returns current environment name retrieved from IWebHostEnvironment
        /// </summary>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult GetAspNetEnvironment()
        {
            return Ok(_env.EnvironmentName);
        }

        /// <summary>
        /// Returns a raw current server environment variable value introduced in current server global environment variables pool
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(String), 200)]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult GetRawEnvironmentVariable()
        {
            return Ok(Environment.GetEnvironmentVariable(CustomConfigurationProvider.DefaultEnvironmentVariableName));
        }

        /// <summary>
        /// Returns name of the variable which uses to determine current server environment
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(String), 200)]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult GetRawEnvironmentVariableName()
        {
            return Ok(CustomConfigurationProvider.DefaultEnvironmentVariableName);
        }

        /// <summary>
        /// Returns information about available repositories for the primary database 
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(String), 200)]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult GetAvailableRepositories()
        {
            return Ok(_unitOfWork);
        }

        /// <summary>
        /// Returns external internet IP address
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(String), 200)]
        [ProducesResponseType(typeof(String), 500)]
        public async Task<IActionResult> GetExternalIpAddress()
        {
            using (var client = new HttpClient())
            {
                return Ok(await client.GetStringAsync("http://checkip.amazonaws.com/"));
            }
        }
    }
}
#endif