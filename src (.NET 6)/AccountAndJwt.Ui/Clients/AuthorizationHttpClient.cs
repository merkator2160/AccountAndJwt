using AccountAndJwt.Contracts.Models.Api.Request;
using AccountAndJwt.Contracts.Models.Api.Response;
using AccountAndJwt.Contracts.Models.Exceptions;
using AccountAndJwt.Ui.Clients.Interfaces;
using System.Net.Http.Json;

namespace AccountAndJwt.Ui.Clients
{
    internal class AuthorizationHttpClient : TypedHttpClient, IAuthorizationHttpClient
    {
        // IAuthorizationHttpClient ///////////////////////////////////////////////////////////////

        // Token //
        public async Task<AuthorizeResponseAm> AuthorizeByCredentialsAsync(String login, String password)
        {
            using (var response = await this.PostAsJsonAsync("api/Token/AuthorizeByCredentials", new AuthorizeRequestAm()
            {
                Login = login,
                Password = password
            }))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpServerException(HttpMethod.Post, response.StatusCode, response.Headers.Location!.AbsolutePath, await response.Content.ReadAsStringAsync());

                return await response.Content.ReadFromJsonAsync<AuthorizeResponseAm>();
            }
        }
        public async Task<String> RefreshTokenAsync(String refreshToken)
        {
            using (var response = await this.PostAsJsonAsync("api/Token/RefreshToken", refreshToken))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpServerException(HttpMethod.Post, response.StatusCode, response.Headers.Location!.AbsolutePath, await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsStringAsync();
            }
        }
        public async void RevokeTokenAsync(String refreshToken)
        {
            using (var response = await this.PostAsJsonAsync("api/Token/RevokeToken", refreshToken))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpServerException(HttpMethod.Post, response.StatusCode, response.Headers.Location!.AbsolutePath, await response.Content.ReadAsStringAsync());
            }
        }

        // Account //
        public async Task<HttpResponseMessage> RegisterAsync(RegisterUserRequestAm request)
        {
            return await this.PostAsJsonAsync("api/Token/AuthorizeByCredentials", request);
        }

        // Debug //
        public async Task<WeatherForecastResponseAm[]> GetWeatherForecastAsync()
        {
            return await this.GetFromJsonAsync<WeatherForecastResponseAm[]>("api/WeatherForecast/GetWeatherForecast");
        }
    }
}