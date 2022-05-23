using AccountAndJwt.Contracts.Models.Api.Response;
using Microsoft.AspNetCore.Mvc;

namespace AccountAndJwt.AuthorizationService.Controllers.Testing
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly String[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly ILogger<WeatherForecastController> _logger;


        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }


        // ACTIONS ////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        [ProducesResponseType(typeof(WeatherForecastResponseAm[]), 200)]
        [ProducesResponseType(typeof(String), 500)]
        public IActionResult GetWeatherForecast()
        {
            var rng = new Random();
            var weatherForecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecastResponseAm
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            return Ok(weatherForecasts);
        }
    }
}