using AccountAndJwt.Ui.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountAndJwt.Ui
{
	public class Program
	{
		public static async Task Main(String[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);

			builder.RootComponents.Add<App>("app");
			builder.Services.AddTransient(p => new HttpClient
			{
				BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
			});
			builder.Services.AddTransient(p => new RootConfig()
			{
				ServerBaseUrl = "https://localhost:44322/api"
			});

			await builder.Build().RunAsync();
		}
	}
}
