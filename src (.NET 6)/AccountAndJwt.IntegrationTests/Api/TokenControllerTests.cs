using AccountAndJwt.Contracts.Models.Api;
using AspNetCore.Http.Extensions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AccountAndJwt.IntegrationTests.Api
{
    public class TokenControllerTests
    {
        [Fact]
        public async Task AuthorizeByCredentialsTest()
        {
            using (var factory = new CustomWebApplicationFactory())
            {
                var client = factory.CreateClient();
                var response = await client.PostAsJsonAsync("/api/Token/AuthorizeByCredentials", new AuthorizeRequestAm()
                {
                    Login = "admin",
                    Password = "ipANWvuFUA5e2qWk0iTd"
                });

                response.EnsureSuccessStatusCode();
                var authorizedResponse = await response.DeserializeAsync<AuthorizeResponseAm>();

                Assert.True(!String.IsNullOrEmpty(authorizedResponse.AccessToken));
                Assert.True(!String.IsNullOrEmpty(authorizedResponse.RefreshToken));
            }
        }

        [Fact]
        public async Task RefreshTokenTest()
        {
            using (var factory = new CustomWebApplicationFactory())
            {
                var client = factory.CreateClient();
                var authorizedResponse = await client.PostAsJsonAsync("/api/Token/AuthorizeByCredentials", new AuthorizeRequestAm()
                {
                    Login = "admin",
                    Password = "ipANWvuFUA5e2qWk0iTd"
                });

                authorizedResponse.EnsureSuccessStatusCode();
                var authorizedObject = await authorizedResponse.DeserializeAsync<AuthorizeResponseAm>();

                Assert.True(!String.IsNullOrEmpty(authorizedObject.AccessToken));
                Assert.True(!String.IsNullOrEmpty(authorizedObject.RefreshToken));

                var refreshResponse = await client.PostAsJsonAsync("/api/Token/RefreshToken", authorizedObject.RefreshToken);
                refreshResponse.EnsureSuccessStatusCode();

                var refreshObject = await authorizedResponse.DeserializeAsync<RefreshTokenResponseAm>();

                Assert.True(!String.IsNullOrEmpty(refreshObject.AccessToken));
                Assert.Equal(authorizedObject.AccessToken, refreshObject.AccessToken);
            }
        }

        [Fact]
        public async Task RevokeTokenTest()
        {
            using (var factory = new CustomWebApplicationFactory())
            {
                var client = factory.CreateClient();
                var authorizedResponse = await client.PostAsJsonAsync("/api/Token/AuthorizeByCredentials", new AuthorizeRequestAm()
                {
                    Login = "admin",
                    Password = "ipANWvuFUA5e2qWk0iTd"
                });
                authorizedResponse.EnsureSuccessStatusCode();
                var authorizedObject = await authorizedResponse.DeserializeAsync<AuthorizeResponseAm>();

                Assert.True(!String.IsNullOrEmpty(authorizedObject.AccessToken));
                Assert.True(!String.IsNullOrEmpty(authorizedObject.RefreshToken));

                var revokeResponse = await client.PostAsJsonAsync("/api/Token/RevokeToken", authorizedObject.RefreshToken);
                revokeResponse.EnsureSuccessStatusCode();

                var authorizedResponseSecond = await client.PostAsJsonAsync("/api/Token/AuthorizeByCredentials", new AuthorizeRequestAm()
                {
                    Login = "admin",
                    Password = "ipANWvuFUA5e2qWk0iTd"
                });
                authorizedResponseSecond.EnsureSuccessStatusCode();
                var authorizedObjectSecond = await authorizedResponse.DeserializeAsync<AuthorizeResponseAm>();

                Assert.Equal(authorizedObject.RefreshToken, authorizedObjectSecond.RefreshToken);
            }
        }
    }
}