using System;
using System.Threading.Tasks;
using Xunit;

namespace AccountAndJwt.IntegrationTests.Api
{
	public class CoreTests : IClassFixture<PandaWebApplicationFactory>
	{
		private readonly PandaWebApplicationFactory _factory;


		public CoreTests(PandaWebApplicationFactory factory)
		{
			_factory = factory;
		}


		// TESTS //////////////////////////////////////////////////////////////////////////////////
		[Theory]
		[InlineData("/api/Test/BasicTest")]
		[InlineData("/api/Test/ContextReferenceTest")]
		public async Task BasicTest(String url)
		{
			var client = _factory.CreateClient();
			var response = await client.GetAsync(url);

			response.EnsureSuccessStatusCode();
			Assert.True(true);
		}

		[Fact]
		public async Task EmbeddedAccountsTest()
		{
			var client = _factory.CreateClient();
			await _factory.AuthorizeAsGuestAsync(client);

			client = _factory.CreateClient();
			await _factory.AuthorizeAsAdminAsync(client);
		}
	}
}