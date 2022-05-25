using AccountAndJwt.Ui.Clients;
using AccountAndJwt.Ui.Services;
using AccountAndJwt.Ui.Services.Interfaces;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

namespace AccountAndJwt.Ui
{
    internal class Program
    {
        public static async Task Main(String[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.ConfigureContainer(ConfigureContainer(builder.Configuration));
            RegisterServices(builder.Services);
            RegisterRadzenServices(builder.Services);
            RegisterComponents(builder.RootComponents);

            var host = builder.Build();
            host.Services.GetRequiredService<IRadzenThemeService>().Initialize();

            await host.RunAsync();
        }
        private static AutofacServiceProviderFactory ConfigureContainer(IConfiguration configuration)
        {
            return new AutofacServiceProviderFactory(builder =>
            {
                builder.RegisterType<TypedHttpClient>().AsSelf().AsImplementedInterfaces();
                builder.RegisterType<LocalStorageService>().AsImplementedInterfaces();
                builder.RegisterType<BrowserPopupService>().AsImplementedInterfaces();
                builder.RegisterType<AuthorizationService>().As<AuthenticationStateProvider>().AsSelf().AsImplementedInterfaces().SingleInstance();
                builder.Register(sp => new AuthorizationHttpClient()
                {
                    BaseAddress = new Uri(configuration["ServerConfig:AuthorizationService:BaseAddress"])
                }).AsImplementedInterfaces();
            });
        }
        private static void RegisterServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddAuthorizationCore();
            services.AddBlazoredSessionStorage();
            //services.Replace(ServiceDescriptor.Scoped<IJsonSerializer, NewtonSoftJsonSerializer>());
        }
        private static void RegisterRadzenServices(IServiceCollection services)
        {
            services.AddScoped<IRadzenThemeService, RadzenThemeService>();
            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<TooltipService>();
            services.AddScoped<ContextMenuService>();
        }
        private static void RegisterComponents(RootComponentMappingCollection rootComponents)
        {
            rootComponents.Add<App>("#app");
            rootComponents.Add<HeadOutlet>("head::after");
        }
    }
}