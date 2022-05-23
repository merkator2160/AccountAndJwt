using AccountAndJwt.Contracts.Models.Api.Response;
using AccountAndJwt.Ui.Clients.Interfaces;
using Microsoft.AspNetCore.Components;

namespace AccountAndJwt.Ui.Pages
{
    [Route("fetchdata")]
    public partial class FetchData
    {
        private WeatherForecastResponseAm[] _forecasts;


        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        [Inject]
        public IAuthorizationHttpClient Client { get; set; }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        protected override async Task OnInitializedAsync()
        {
            _forecasts = await Client.GetWeatherForecastAsync();
        }
    }
}