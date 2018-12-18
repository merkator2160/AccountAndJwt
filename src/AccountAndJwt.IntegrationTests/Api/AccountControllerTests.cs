using AccountAndJwt.Api.Contracts.Models;
using AccountAndJwt.Api.Core.Consts;
using System.Threading.Tasks;
using Xunit;

namespace AccountAndJwt.IntegrationTests.Api
{
	public class AccountControllerTests : IClassFixture<PandaWebApplicationFactory>
	{
		private readonly PandaWebApplicationFactory _factory;


		public AccountControllerTests(PandaWebApplicationFactory factory)
		{
			_factory = factory;
		}


		// TESTS //////////////////////////////////////////////////////////////////////////////////
		[Fact]
		public async Task GetAccountInfoTest()
		{
			var client = _factory.CreateClient();
			await _factory.AuthorizeAsAdminAsync(client);

			var response = await client.GetAsync("/api/Account/GetAccountInfo");

			response.EnsureSuccessStatusCode();
			var authorizedResponse = await response.DeserializeAsync<UserAm>();

			Assert.Contains(Role.Admin, authorizedResponse.Roles);
		}
	}
}