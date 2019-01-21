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
		[InlineData("/api/CoreTest/BasicTest")]
		[InlineData("/api/CoreTest/ContextReferenceTest")]
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

		[Fact]
		public async Task ErrorHandlingMiddlewareTest()
		{
			var client = _factory.CreateClient();
			var response = await client.GetAsync("/api/Debug/CreateUnhandledException");
			var result = response.Content.ReadAsStringAsync().Result;

			Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
			Assert.Equal("Exception message!", result);
		}
	}
}