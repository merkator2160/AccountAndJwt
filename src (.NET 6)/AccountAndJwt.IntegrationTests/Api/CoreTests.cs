using AccountAndJwt.Contracts.Const;
using AccountAndJwt.Contracts.Models.Api.Response;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace AccountAndJwt.IntegrationTests.Api
{
    public class CoreTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;


        public CoreTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }


        // TESTS //////////////////////////////////////////////////////////////////////////////////
        [Theory]
        [InlineData("/")]
        [InlineData("/api/CoreTest/BasicTest")]
        [InlineData("/api/CoreTest/ContextReferenceTest")]
        public async Task BasicTest(String url)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task EmbeddedAccountsTest()
        {
            var client = _factory.CreateClient();
            await _factory.AuthorizeAsGuestAsync(client);

            client = _factory.CreateClient();
            await _factory.AuthorizeAsAdminAsync(client);
        }

        [Fact]
        public async Task UnhandledExceptionTest()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/CoreTest/UnhandledExceptionTest");
            var result = response.Content.ReadAsStringAsync().Result;

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Contains("Exception message!", result);
        }

        [Fact]
        public async Task UnhandledApplicationExceptionTest()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/CoreTest/UnhandledApplicationExceptionTest");
            var result = response.Content.ReadAsStringAsync().Result;

            Assert.Equal(460, (Int32)response.StatusCode);
            Assert.Contains("ApplicationException message!", result);
        }

        [Fact]
        public async Task GetUserClaimsTest()
        {
            var client = _factory.CreateClient();
            await _factory.AuthorizeAsAdminAsync(client);

            var response = await client.GetAsync("/api/CoreTest/GetCurrentUserClaims");

            response.EnsureSuccessStatusCode();
            var claimsResponse = await response.DeserializeAsync<GetClaimsResponseAm[]>();

            Assert.Contains(claimsResponse, p => p.Value.Equals(Role.Admin));
        }
    }
}