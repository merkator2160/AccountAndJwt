using AccountAndJwt.Contracts.Models.Api.Request;
using AccountAndJwt.Contracts.Models.Api.Response;
using AspNetCore.Http.Extensions;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace AccountAndJwt.IntegrationTests.Api
{
    public class TokenControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        const String _login = "admin";
        const String _password = "ipANWvuFUA5e2qWk0iTd";

        private readonly CustomWebApplicationFactory _factory;


        public TokenControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }


        // TESTS //////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public async Task AuthorizeByCredentialsTest()
        {
            var client = _factory.CreateClient();
            var authorizedResponse = await AuthorizeByCredentials(client, _login, _password);

            Assert.NotNull(authorizedResponse.AccessToken);
            Assert.NotNull(authorizedResponse.RefreshToken);
        }

        [Fact]
        public async Task RefreshTokenTest()
        {
            var client = _factory.CreateClient();
            var authorizedResponse = await AuthorizeByCredentials(client, _login, _password);

            Assert.NotNull(authorizedResponse.AccessToken);
            Assert.NotNull(authorizedResponse.RefreshToken);

            var refreshResponse = await client.PostAsJsonAsync("/api/Token/RefreshToken", authorizedResponse.RefreshToken);
            refreshResponse.EnsureSuccessStatusCode();

            var accessToken = await refreshResponse.Content.ReadAsStringAsync();

            Assert.NotNull(accessToken);
        }

        [Fact]
        public async Task RevokeTokenTest()
        {
            var client = _factory.CreateClient();
            var authorizedResponseFirst = await AuthorizeByCredentials(client, _login, _password);

            var revokeResponse = await client.PostAsJsonAsync("/api/Token/RevokeToken", authorizedResponseFirst.RefreshToken);
            revokeResponse.EnsureSuccessStatusCode();

            var authorizedResponseSecond = await AuthorizeByCredentials(client, _login, _password);

            Assert.NotEqual(authorizedResponseFirst.RefreshToken, authorizedResponseSecond.RefreshToken);
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        private async Task<AuthorizeResponseAm> AuthorizeByCredentials(HttpClient client, String login, String password)
        {
            var response = await client.PostAsJsonAsync("/api/Token/AuthorizeByCredentials", new AuthorizeRequestAm()
            {
                Login = login,
                Password = password
            });

            response.EnsureSuccessStatusCode();
            return await response.DeserializeAsync<AuthorizeResponseAm>();
        }
    }
}