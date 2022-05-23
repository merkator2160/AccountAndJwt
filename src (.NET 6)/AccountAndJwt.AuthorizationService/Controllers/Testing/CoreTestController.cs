using AccountAndJwt.Common.Const;
using AccountAndJwt.Contracts.Models.Api.Response;
using AccountAndJwt.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#if DEBUG
namespace AccountAndJwt.AuthorizationService.Controllers.Testing
{
    /// <summary>
    /// This controller only for integration testing purposes (it does not appear in production)
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CoreTestController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;


        public CoreTestController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        // ACTIONS ////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This test only checks controller availability
        /// </summary>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult BasicTest()
        {
            return Ok();
        }

        /// <summary>
        /// This test returns reference equality information about two two injected data contexts, reference should be the same
        /// </summary>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult ContextReferenceTest()
        {
            var contextInstance1 = _serviceProvider.GetRequiredService<DataContext>();
            var contextInstance2 = _serviceProvider.GetRequiredService<DataContext>();

            if (!Object.ReferenceEquals(contextInstance1, contextInstance2))
                throw new Exception("Data context are not the same!");

            return Ok();
        }

        /// <summary>
        /// Throws an unhandled exception
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult UnhandledExceptionTest()
        {
            throw new Exception("Exception message!");
        }

        /// <summary>
        /// Throws an unhandled application exception
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult UnhandledApplicationExceptionTest()
        {
            throw new ApplicationException("ApplicationException message!");
        }

        /// <summary>
        /// Returns collection of claims assigned to the current user from provided JWT token
        /// </summary>
        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        [ProducesResponseType(typeof(GetClaimsResponseAm[]), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult GetCurrentUserClaims()
        {
            var claims = HttpContext.User.Claims.ToArray().Select(x => new GetClaimsResponseAm
            {
                ClaimType = x.Type,
                Value = x.Value
            });
            return Ok(claims);
        }
    }
}
#endif