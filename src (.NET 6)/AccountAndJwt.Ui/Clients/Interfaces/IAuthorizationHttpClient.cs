using AccountAndJwt.Contracts.Models.Api.Request;
using AccountAndJwt.Contracts.Models.Api.Response;

namespace AccountAndJwt.Ui.Clients.Interfaces
{
    public interface IAuthorizationHttpClient
    {
        // Token //
        Task<AuthorizeResponseAm> AuthorizeByCredentialsAsync(String login, String password);
        Task<String> RefreshTokenAsync(String refreshToken);
        void RevokeTokenAsync(String refreshToken);

        // Account //
        Task<HttpResponseMessage> RegisterAsync(RegisterUserRequestAm request);

        // Debug //
        Task<WeatherForecastResponseAm[]> GetWeatherForecastAsync();
    }
}