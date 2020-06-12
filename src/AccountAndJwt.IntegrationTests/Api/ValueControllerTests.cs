using AccountAndJwt.Contracts.Models;
using AspNetCore.Http.Extensions;
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

			var postResponse1 = await client.PostAsJsonAsync("api/Values", new AddValueAm()
			{
				Value = 1,
				Commentary = "qwerty 1"
			});
			postResponse1.EnsureSuccessStatusCode();

			var postResponse2 = await client.PostAsJsonAsync("api/Values", new AddValueAm()
			{
				Value = 2,
				Commentary = "qwerty 2"
			});
			postResponse2.EnsureSuccessStatusCode();

			var postResponse3 = await client.PostAsJsonAsync("api/Values", new AddValueAm()
			{
				Value = 3,
				Commentary = "qwerty 3"
			});
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
				Value = 1,
				Commentary = "zxcvbnm"
			});
			putValue2Response.EnsureSuccessStatusCode();

			getValue2Response = await client.GetAsync(postResponse2.Headers.Location);
			getValue2Response.EnsureSuccessStatusCode();

			value2 = await getValue2Response.DeserializeAsync<ValueAm>();
			Assert.Equal(allValues[1].Id, value2.Id);
			Assert.Equal(1, value2.Value);
			Assert.Equal("zxcvbnm", value2.Commentary);

			var deleteValue2Response = await client.DeleteAsync(postResponse2.Headers.Location);
			deleteValue2Response.EnsureSuccessStatusCode();

			getAllResponse = await client.GetAsync("/api/Values");
			getAllResponse.EnsureSuccessStatusCode();

			allValues = await getAllResponse.DeserializeAsync<ValueAm[]>();
			Assert.Equal(2, allValues.Length);
		}
	}
}