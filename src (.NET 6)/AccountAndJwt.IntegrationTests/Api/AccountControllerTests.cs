using AccountAndJwt.Contracts.Models.Api;
using System.Threading.Tasks;
using Xunit;

namespace AccountAndJwt.IntegrationTests.Api
{
    public class AccountControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;


        public AccountControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }


        // TESTS //////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public async Task GetAvailableRolesTest()
        {
            var client = _factory.CreateClient();
            await _factory.AuthorizeAsAdminAsync(client);
            var response = await client.GetAsync("/api/Account/GetAvailableRoles");

            response.EnsureSuccessStatusCode();
            var authorizedResponse = await response.DeserializeAsync<RoleAm[]>();

            Assert.True(authorizedResponse.Length == 2);
        }
    }
}