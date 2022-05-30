using AccountAndJwt.ApiClients.Http.Authorization.Interfaces;
using AccountAndJwt.Common.Exceptions;
using AccountAndJwt.Common.Http;
using AccountAndJwt.Contracts.Const;
using AccountAndJwt.Contracts.Models.Api;
using AccountAndJwt.Contracts.Models.Api.Request;
using AccountAndJwt.Contracts.Models.Api.Response;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace AccountAndJwt.ApiClients.Http.Authorization
{
    public class AuthorizationHttpClient : TypedHttpClient, IAuthorizationHttpClient
    {
        public AuthorizationHttpClient()
        {
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpMimeType.Application.Json));
        }


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
        public async Task RevokeTokenAsync(String refreshToken)
        {
            using (var response = await this.PostAsJsonAsync("api/Token/RevokeToken", refreshToken))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpServerException(HttpMethod.Post, response.StatusCode, response.Headers.Location!.AbsolutePath, await response.Content.ReadAsStringAsync());
            }
        }

        // Account //
        public async Task<RegisterUserResponseAm> RegisterAsync(RegisterUserRequestAm request)
        {
            using (var response = await this.PostAsJsonAsync("api/Account/Register", request))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpServerException(HttpMethod.Post, response.StatusCode, response.Headers.Location!.AbsolutePath, await response.Content.ReadAsStringAsync());

                return await response.Content.ReadFromJsonAsync<RegisterUserResponseAm>();
            }
        }
        public async Task DeleteAccountAsync(Int32 userId, String accessToken)
        {
            var verb = HttpMethod.Delete;
            using (var requestMessage = new HttpRequestMessage(verb, $"api/Account/DeleteAccount?userId={userId}"))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using (var response = await SendAsync(requestMessage))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new HttpServerException(verb, response.StatusCode, response.Headers.Location!.AbsolutePath, await response.Content.ReadAsStringAsync());
                }
            }
        }
        public async Task<RoleAm[]> GetAvailableRolesAsync(String accessToken)
        {
            var verb = HttpMethod.Get;
            using (var requestMessage = new HttpRequestMessage(verb, "api/Account/GetAvailableRoles"))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using (var response = await SendAsync(requestMessage))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new HttpServerException(verb, response.StatusCode, response.Headers.Location!.AbsolutePath, await response.Content.ReadAsStringAsync());

                    return await response.Content.ReadFromJsonAsync<RoleAm[]>();
                }
            }
        }
        public async Task AddUserRoleAsync(AddRemoveUserRoleRequestAm request, String accessToken)
        {
            var verb = HttpMethod.Post;
            using (var requestMessage = new HttpRequestMessage(verb, "api/Account/AddUserRole"))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                requestMessage.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, HttpMimeType.Application.Json);

                using (var response = await SendAsync(requestMessage))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new HttpServerException(verb, response.StatusCode, response.Headers.Location!.AbsolutePath, await response.Content.ReadAsStringAsync());
                }
            }
        }
        public async Task RemoveUserRoleAsync(AddRemoveUserRoleRequestAm request, String accessToken)
        {
            var verb = HttpMethod.Post;
            using (var requestMessage = new HttpRequestMessage(verb, "api/Account/RemoveUserRole"))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                requestMessage.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, HttpMimeType.Application.Json);

                using (var response = await SendAsync(requestMessage))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new HttpServerException(verb, response.StatusCode, response.Headers.Location!.AbsolutePath, await response.Content.ReadAsStringAsync());
                }
            }
        }
        public async Task ChangeEmailAsync(String newEmail, String accessToken)
        {
            var verb = HttpMethod.Put;
            using (var requestMessage = new HttpRequestMessage(verb, "api/Account/ChangeEmail"))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                requestMessage.Content = new StringContent(newEmail, Encoding.UTF8, HttpMimeType.Application.Json);

                using (var response = await SendAsync(requestMessage))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new HttpServerException(verb, response.StatusCode, response.Headers.Location!.AbsolutePath, await response.Content.ReadAsStringAsync());
                }
            }
        }
        public async Task ChangeNameAsync(ChangeNameRequestAm request, String accessToken)
        {
            var verb = HttpMethod.Put;
            using (var requestMessage = new HttpRequestMessage(verb, "api/Account/ChangeEmail"))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                requestMessage.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, HttpMimeType.Application.Json);

                using (var response = await SendAsync(requestMessage))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new HttpServerException(verb, response.StatusCode, response.Headers.Location!.AbsolutePath, await response.Content.ReadAsStringAsync());
                }
            }
        }
        public async Task<UserAm> GetUserAsync(String accessToken)
        {
            var verb = HttpMethod.Get;
            using (var requestMessage = new HttpRequestMessage(verb, "api/Account/GetUser"))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using (var response = await SendAsync(requestMessage))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new HttpServerException(verb, response.StatusCode, response.Headers.Location!.AbsolutePath, await response.Content.ReadAsStringAsync());

                    return await response.Content.ReadFromJsonAsync<UserAm>();
                }
            }
        }
        public async Task<PagedUserResponseAm> GetUsersPagedAsync(GetUsersPagedRequestAm request, String accessToken)
        {
            var verb = HttpMethod.Post;
            using (var requestMessage = new HttpRequestMessage(verb, "api/Account/GetUsersPaged"))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                requestMessage.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, HttpMimeType.Application.Json);

                using (var response = await SendAsync(requestMessage))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new HttpServerException(verb, response.StatusCode, response.Headers.Location!.AbsolutePath, await response.Content.ReadAsStringAsync());

                    return await response.Content.ReadFromJsonAsync<PagedUserResponseAm>();
                }
            }
        }
        public async Task ResetPasswordAsync(ResetPasswordRequestAm request, String accessToken)
        {
            var verb = HttpMethod.Post;
            using (var requestMessage = new HttpRequestMessage(verb, "api/Account/ResetPassword"))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                requestMessage.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, HttpMimeType.Application.Json);

                using (var response = await SendAsync(requestMessage))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new HttpServerException(verb, response.StatusCode, response.Headers.Location!.AbsolutePath, await response.Content.ReadAsStringAsync());
                }
            }
        }

        // Debug //
        public async Task<WeatherForecastResponseAm[]> GetWeatherForecastAsync()
        {
            return await this.GetFromJsonAsync<WeatherForecastResponseAm[]>("api/WeatherForecast/GetWeatherForecast");
        }
    }
}