using AccountAndJwt.Ui.Clients;
using AccountAndJwt.Ui.Clients.Interfaces;
using AccountAndJwt.Ui.Models;
using AccountAndJwt.Ui.Services;
using AccountAndJwt.Ui.Services.Interfaces;
using AccountAndJwt.Ui.Utilities;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace AccountAndJwt.Ui
{
    internal class Program
    {
        public static async Task Main(String[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.ConfigureContainer(new AutofacServiceProviderFactory(ConfigureContainer));

            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services
                .AddOptions()
                .AddAuthorizationCore()
                .AddBlazoredSessionStorage()
                //.Replace(ServiceDescriptor.Scoped<IJsonSerializer, NewtonSoftJsonSerializer>())
                .AddScoped<IAuthorizationHttpClient>(sp => new AuthorizationHttpClient()
                {
                    BaseAddress = new Uri(builder.Configuration["ServerConfig:AuthorizationService:BaseAddress"])
                });

            var host = builder.Build();

            //await LocalStorageTestAsync(host);
            //await BlazoredSessionStorageTestAsync(host);

            await host.RunAsync();
        }
        private static void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<HttpClient>().AsSelf();
            builder.RegisterType<SignOutSessionStateManager>().AsSelf();
            builder.RegisterType<UserService>().AsImplementedInterfaces();
            builder.RegisterType<LocalStorageService>().AsImplementedInterfaces();
            builder.RegisterType<CustomAuthenticationStateProvider>().As<AuthenticationStateProvider>().AsImplementedInterfaces().SingleInstance();
            //builder.Register(sp =>
            //{
            //    var configuration = sp.Resolve<WebAssemblyHostConfiguration>();
            //    return new AuthorizationHttpClient()
            //    {
            //        BaseAddress = new Uri(configuration["ServerConfig:AuthorizationService:BaseAddress"])
            //    };
            //}).AsImplementedInterfaces();
        }
        private static async Task LocalStorageTestAsync(WebAssemblyHost host)
        {
            const String key = "key";

            var localStorage = host.Services.GetRequiredService<ILocalStorageService>();
            await localStorage.SetItemAsync(key, new User()
            {
                Id = 4,
                FirstName = "FirstName",
                LastName = "LastName"
            });

            var test = await localStorage.GetItemAsync<User>(key);
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