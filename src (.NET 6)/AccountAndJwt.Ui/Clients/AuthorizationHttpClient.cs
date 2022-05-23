using AccountAndJwt.Contracts.Models.Api.Request;
using AccountAndJwt.Contracts.Models.Api.Response;
using AccountAndJwt.Ui.Clients.Interfaces;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace AccountAndJwt.Ui.Clients
{
    internal class AuthorizationHttpClient : HttpClient, IAuthorizationHttpClient
    {
        public AuthorizationHttpClient()
        {

        }


        // IHttpService ///////////////////////////////////////////////////////////////////////////

        // Token //
        public async Task<HttpResponseMessage> AuthorizeByCredentialsAsync(AuthorizeRequestAm credentials)
        {
            return await this.PostAsJsonAsync("api/Token/AuthorizeByCredentials", credentials);
        }
        public async Task<HttpResponseMessage> RefreshTokenAsync(String refreshToken)
        {
            return await this.PostAsJsonAsync("api/Token/RefreshToken", refreshToken);
        }
        public async Task<HttpResponseMessage> RevokeTokenAsync(String refreshToken)
        {
            return await this.PostAsJsonAsync("api/Token/RevokeToken", refreshToken);
        }

        // Debug //
        public async Task<WeatherForecastResponseAm[]> GetWeatherForecastAsync()
        {
            return await this.GetFromJsonAsync<WeatherForecastResponseAm[]>("api/WeatherForecast/GetWeatherForecast");
        }

        // Other //
        public async Task<T> Get<T>(String uri, String accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            return await Send<T>(request, accessToken);
        }
        public async Task<T> Post<T>(String uri, Object value, String accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");

            return await Send<T>(request, accessToken);
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        private async Task<T> Send<T>(HttpRequestMessage request, String accessToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using (var response = await SendAsync(request))
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    //_navigationManager.NavigateTo("logout");
                    return default;
                }

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<T>();

                var error = await response.Content.ReadFromJsonAsync<Dictionary<String, String>>();
                throw new Exception(error["message"]);
            }
        }
    }
}