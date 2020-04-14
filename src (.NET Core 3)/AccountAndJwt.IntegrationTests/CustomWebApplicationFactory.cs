using AccountAndJwt.Common.Consts;
using AccountAndJwt.Contracts.Models;
using AspNetCore.Http.Extensions;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AccountAndJwt.IntegrationTests
{
	public class CustomWebApplicationFactory : WebApplicationFactory<TestStartup>
	{
		// OVERRIDE ///////////////////////////////////////////////////////////////////////////////
		protected override IHostBuilder CreateHostBuilder()
		{
			return Host.CreateDefaultBuilder()
				.UseServiceProviderFactory(new AutofacServiceProviderFactory())
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<TestStartup>();
					webBuilder.UseEnvironment(HostingEnvironment.Development);
					webBuilder.UseWebRoot(Environment.CurrentDirectory);
				});
		}


		// AUTHORIZATION //////////////////////////////////////////////////////////////////////////
		public async Task AuthorizeAsGuestAsync(HttpClient client)
		{
			await AuthorizeAsync("guest", "HtR00MtOxKyHUg7359QL", client);
		}
		public async Task AuthorizeAsAdminAsync(HttpClient client)
		{
			await AuthorizeAsync("admin", "ipANWvuFUA5e2qWk0iTd", client);
		}
		public async Task AuthorizeAsync(String login, String password, HttpClient client)
		{
			var response = await client.PostAsJsonAsync("/api/Token/AuthorizeByCredentials", new AuthorizeRequestAm()
			{
				Login = login,
				Password = password
			});

			var authorizationTokens = await response.DeserializeAsync<AuthorizeResponseAm>();

			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpMimeType.Application.Json));
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorizationTokens.AccessToken);
		}
	}
}