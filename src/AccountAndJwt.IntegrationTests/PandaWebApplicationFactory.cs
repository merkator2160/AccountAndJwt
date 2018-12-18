using AccountAndJwt.Api.Contracts.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace AccountAndJwt.IntegrationTests
{
	public class PandaWebApplicationFactory : WebApplicationFactory<TestStartup>
	{
		// OVERRIDE ///////////////////////////////////////////////////////////////////////////////
		protected override void ConfigureClient(HttpClient client)
		{
			base.ConfigureClient(client);
		}
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			base.ConfigureWebHost(builder);
		}
		protected override TestServer CreateServer(IWebHostBuilder builder)
		{
			return base.CreateServer(builder);
		}
		protected override IWebHostBuilder CreateWebHostBuilder()
		{
			return WebHost.CreateDefaultBuilder()
				.UseEnvironment("Development")
				.UseWebRoot(Environment.CurrentDirectory)
				.UseStartup<TestStartup>()
				.ConfigureLogging(logging =>
				{
					logging.ClearProviders();
					logging.SetMinimumLevel(LogLevel.Trace);
				});
		}
		protected override void Dispose(Boolean disposing)
		{
			base.Dispose(disposing);
		}
		protected override IEnumerable<Assembly> GetTestAssemblies()
		{
			return base.GetTestAssemblies();
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

			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorizationTokens.AccessToken);
		}


		// DEPENDENCY INJECTION ///////////////////////////////////////////////////////////////////
		public T Resolve<T>()
		{
			return (T)Server.Host.Services.GetService(typeof(T));
		}
	}
}