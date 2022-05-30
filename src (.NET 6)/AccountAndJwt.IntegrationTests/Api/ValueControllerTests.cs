using AccountAndJwt.Contracts.Models.Api;
using AccountAndJwt.Contracts.Models.Api.Request;
using AccountAndJwt.Contracts.Models.Api.Response;
using AspNetCore.Http.Extensions;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace AccountAndJwt.IntegrationTests.Api
{
    public class ValueControllerTests : IDisposable
    {
        private readonly CustomWebApplicationFactory _factory;


        public ValueControllerTests()
        {
            _factory = new CustomWebApplicationFactory();
        }
        public void Dispose()
        {
            _factory?.Dispose();
        }


        // TESTS //////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public async Task GetPagedTest()
        {
            const Int32 pageSize = 100;
            const Int32 pageNumber = 1;

            var client = _factory.CreateClient();
            await _factory.AuthorizeAsAdminAsync(client);

            await AddValue(client, 1, "qwerty 1");
            await AddValue(client, 2, "qwerty 2");
            await AddValue(client, 3, "qwerty 3");

            var response = await client.GetAsync($"api/Value/{pageSize}/{pageNumber}");
            response.CheckError();

            var allValues = await response.DeserializeAsync<PagedValueResponseAm>();

            Assert.Equal(3, allValues.Values.Length);
        }

        [Fact]
        public async Task GetTest()
        {
            var client = _factory.CreateClient();
            await _factory.AuthorizeAsAdminAsync(client);

            var valueLocation1 = await AddValue(client, 1, "qwerty 1");
            var valueLocation2 = await AddValue(client, 2, "qwerty 2");
            var valueLocation3 = await AddValue(client, 3, "qwerty 3");

            var getValue2Response = await client.GetAsync(valueLocation2);
            getValue2Response.CheckError();

            var value2 = await getValue2Response.DeserializeAsync<ValueAm>();

            Assert.NotEqual(0, value2.Id);
            Assert.Equal(2, value2.Value);
            Assert.Equal("qwerty 2", value2.Commentary);
        }

        [Fact]
        public async Task UpdateTest()
        {
            var client = _factory.CreateClient();
            await _factory.AuthorizeAsAdminAsync(client);

            var valueLocation1 = await AddValue(client, 1, "qwerty 1");
            var valueLocation2 = await AddValue(client, 2, "qwerty 2");
            var valueLocation3 = await AddValue(client, 3, "qwerty 3");

            var allValues = await GetFirstPage(client);

            var putValue2Response = await client.PutAsJsonAsync("/api/Value", new ValueAm()
            {
                Id = allValues[1].Id,
                Value = 10,
                Commentary = "zxcvbnm"
            });
            putValue2Response.EnsureSuccessStatusCode();

            var getValue2Response = await client.GetAsync(valueLocation2);
            getValue2Response.EnsureSuccessStatusCode();

            var value2 = await getValue2Response.DeserializeAsync<ValueAm>();

            Assert.Equal(allValues[1].Id, value2.Id);
            Assert.Equal(10, value2.Value);
            Assert.Equal("zxcvbnm", value2.Commentary);
        }

        [Fact]
        public async Task DeleteTest()
        {
            var client = _factory.CreateClient();
            await _factory.AuthorizeAsAdminAsync(client);

            var valueLocation1 = await AddValue(client, 1, "qwerty 1");
            var valueLocation2 = await AddValue(client, 2, "qwerty 2");
            var valueLocation3 = await AddValue(client, 3, "qwerty 3");

            var allValues = await GetFirstPage(client);
            Assert.Equal(3, allValues.Length);

            var deleteValue2Response = await client.DeleteAsync(valueLocation2);
            deleteValue2Response.CheckError();

            allValues = await GetFirstPage(client);

            Assert.Equal(2, allValues.Length);
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        private async Task<Uri> AddValue(HttpClient client, Int32 value, String commentary)
        {
            var response = await client.PostAsJsonAsync("api/Value", new AddValueRequestAm()
            {
                Value = value,
                Commentary = commentary
            });
            response.CheckError();

            return response.Headers.Location;
        }
        private async Task<ValueAm[]> GetFirstPage(HttpClient client)
        {
            var response = await client.GetAsync($"/api/Value/{1000}/{1}");
            response.CheckError();

            return (await response.DeserializeAsync<PagedValueResponseAm>()).Values;
        }
    }
}