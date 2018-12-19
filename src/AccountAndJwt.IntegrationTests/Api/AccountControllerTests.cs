using AccountAndJwt.Api.Contracts.Models;
using AccountAndJwt.Common.Consts;
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
		public async Task GetAccountInfoTest()
		{
			var client = _factory.CreateClient();
			await _factory.AuthorizeAsAdminAsync(client);

			var response = await client.GetAsync("/api/Debug/GetCurrentUserClaims");

			response.EnsureSuccessStatusCode();
			var claimsResponse = await response.DeserializeAsync<GetClaimsResponseAm[]>();

			Assert.Contains(claimsResponse, p => p.Value.Equals(Role.Admin));
		}
	}
}