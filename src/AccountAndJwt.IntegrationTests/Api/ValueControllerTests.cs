using AccountAndJwt.Contracts.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace AccountAndJwt.IntegrationTests.Api
{
	public class ValueControllerTests : IClassFixture<CustomWebApplicationFactory>
	{
		private readonly CustomWebApplicationFactory _factory;


		public ValueControllerTests(CustomWebApplicationFactory factory)
		{
			_factory = factory;
		}


		// TESTS //////////////////////////////////////////////////////////////////////////////////
		[Fact]
		public async Task CrudTest()
		{
			var client = _factory.CreateClient();
			await _factory.AuthorizeAsAdminAsync(client);

			var postResponse1 = await client.PostAsJsonAsync("api/Values", "qwerty 1");
			var postResponse2 = await client.PostAsJsonAsync("api/Values", "qwerty 2");
			var postResponse3 = await client.PostAsJsonAsync("api/Values", "qwerty 3");

			postResponse1.EnsureSuccessStatusCode();
			postResponse2.EnsureSuccessStatusCode();
			postResponse3.EnsureSuccessStatusCode();

			var getAllResponse = await client.GetAsync("/api/Values");
			getAllResponse.EnsureSuccessStatusCode();

			var allValues = await getAllResponse.DeserializeAsync<ValueAm[]>();
			Assert.Equal(3, allValues.Length);

			var getValue2Response = await client.GetAsync(postResponse2.Headers.Location);
			getValue2Response.EnsureSuccessStatusCode();

			var value2 = await getValue2Response.DeserializeAsync<ValueAm>();
			Assert.Equal(allValues[1].Id, value2.Id);
			Assert.Equal(allValues[1].Value, value2.Value);

			var putValue2Response = await client.PutAsJsonAsync("/api/Values", new ValueAm()
			{
				Id = allValues[1].Id,
				Value = "zxcvbnm"
			});
			putValue2Response.EnsureSuccessStatusCode();

			getValue2Response = await client.GetAsync(postResponse2.Headers.Location);
			getValue2Response.EnsureSuccessStatusCode();

			value2 = await getValue2Response.DeserializeAsync<ValueAm>();
			Assert.Equal(allValues[1].Id, value2.Id);
			Assert.Equal("zxcvbnm", value2.Value);

			var deleteValue2Response = await client.DeleteAsync(postResponse2.Headers.Location);
			deleteValue2Response.EnsureSuccessStatusCode();

			getAllResponse = await client.GetAsync("/api/Values");
			getAllResponse.EnsureSuccessStatusCode();

			allValues = await getAllResponse.DeserializeAsync<ValueAm[]>();
			Assert.Equal(2, allValues.Length);
		}
	}
}