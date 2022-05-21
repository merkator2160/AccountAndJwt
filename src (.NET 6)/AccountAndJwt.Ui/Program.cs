using AccountAndJwt.Ui.Clients;
using AccountAndJwt.Ui.Clients.Interfaces;
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
using Radzen;

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

                // Radzen //
                .AddScoped<IRadzenThemeService, RadzenThemeService>()
                .AddScoped<DialogService>()
                .AddScoped<NotificationService>()
                .AddScoped<TooltipService>()
                .AddScoped<ContextMenuService>()
                // Radzen //

                //.Replace(ServiceDescriptor.Scoped<IJsonSerializer, NewtonSoftJsonSerializer>())
                .AddScoped<IAuthorizationHttpClient>(sp => new AuthorizationHttpClient()
                {
                    BaseAddress = new Uri(builder.Configuration["ServerConfig:AuthorizationService:BaseAddress"])
                });

            var host = builder.Build();
            host.Services.GetRequiredService<IRadzenThemeService>().Initialize();

            await host.RunAsync();
        }
        private static void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<HttpClient>().AsSelf();
            builder.RegisterType<SignOutSessionStateManager>().AsSelf();
            builder.RegisterType<UserService>().AsImplementedInterfaces();
            builder.RegisterType<LocalStorageService>().AsImplementedInterfaces();
            builder.RegisterType<CustomAuthenticationStateProvider>().As<AuthenticationStateProvider>().AsImplementedInterfaces().SingleInstance();
        }
    }
}