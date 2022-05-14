using AccountAndJwt.Contracts.Models.Api;

namespace AccountAndJwt.Ui.Clients.Interfaces
{
    public interface IAuthorizationHttpClient
    {
        // Token //
        Task<HttpResponseMessage> AuthorizeByCredentialsAsync(AuthorizeRequestAm credentials);
        Task<HttpResponseMessage> RefreshTokenAsync(String refreshToken);
        Task<HttpResponseMessage> RevokeTokenAsync(String refreshToken);

        // Debug //
        Task<WeatherForecastAm[]> GetWeatherForecastAsync();

        // Other //
        Task<T> Get<T>(String uri, String accessToken);
        Task<T> Post<T>(String uri, Object value, String accessToken);
    }
}