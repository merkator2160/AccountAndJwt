using AccountAndJwt.Ui.Clients;
using AccountAndJwt.Ui.Clients.Interfaces;
using AccountAndJwt.Ui.Models;
using AccountAndJwt.Ui.Services;
using AccountAndJwt.Ui.Services.Interfaces;
using AccountAndJwt.Ui.Utilities;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace AccountAndJwt.Ui
{
    internal class Program
    {
        public static async Task Main(String[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            //var test = builder.Configuration["ServerConfig:BaseAddress"];

            builder.Services
                .AddScoped<JwtTokenParser>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<ILocalStorageService, LocalStorageService>()
                .AddAuthorizationCore()
                .AddBlazoredSessionStorage()
                .AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>()
                .AddScoped<HttpClient>()
                .AddScoped<IAuthorizationHttpClient>(sp => new AuthorizationHttpClient()
                {
                    BaseAddress = new Uri(builder.Configuration["ServerConfig:AuthorizationService:BaseAddress"])
                });

            var host = builder.Build();

            //await LocalStorageTestAsync(host);
            //await BlazoredSessionStorageTestAsync(host);

            await host.RunAsync();
        }
        private static async Task LocalStorageTestAsync(WebAssemblyHost host)
        {
            const String key = "key";

            var localStorage = host.Services.GetRequiredService<ILocalStorageService>();
            await localStorage.SetObjectAsync(key, new User()
            {
                Id = 4,
                FirstName = "FirstName",
                LastName = "LastName"
            });

            var test = await localStorage.GetObjectAsync<User>(key);
        }
        private static async Task BlazoredSessionStorageTestAsync(WebAssemblyHost host)
        {
            const String key = "key";

            var localStorage = host.Services.GetRequiredService<ISessionStorageService>();
            await localStorage.SetItemAsync(key, new User()
            {
                Id = 4,
                FirstName = "FirstName",
                LastName = "LastName"
            });

            var test = await localStorage.GetItemAsync<User>(key);
        }
    }
}