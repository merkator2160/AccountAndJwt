using AccountAndJwt.AuthorizationService.Database;
using AccountAndJwt.Contracts.Const;
using AccountAndJwt.Contracts.Models.Api.Request;
using AccountAndJwt.Contracts.Models.Api.Response;
using AspNetCore.Http.Extensions;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                    webBuilder.ConfigureAppConfiguration((context, builder) =>
                    {
                        builder.AddEnvironmentVariables()
                            .SetBasePath(context.HostingEnvironment.WebRootPath)
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                            .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: false, reloadOnChange: false);
                    });
                });
        }


        // AUTHORIZATION //////////////////////////////////////////////////////////////////////////
        public async Task AuthorizeAsGuestAsync(HttpClient client)
        {
            await AuthorizeAsync(client, "guest", "HtR00MtOxKyHUg7359QL");
        }
        public async Task AuthorizeAsAdminAsync(HttpClient client)
        {
            await AuthorizeAsync(client, "admin", "ipANWvuFUA5e2qWk0iTd");
        }
        public async Task AuthorizeAsync(HttpClient client, String login, String password)
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

        public void PopulateDatabase()
        {
            var configuration = Services.GetRequiredService<IConfiguration>();
            var passwordSalt = configuration["AudienceConfig:PasswordSalt"];
            var context = Services.GetRequiredService<DataContext>();

            context.Database.EnsureCreated();
            context.AddRoles();
            context.AddUsers(passwordSalt);
        }
        public void RecreateDatabase()
        {
            var configuration = Services.GetRequiredService<IConfiguration>();
            var passwordSalt = configuration["AudienceConfig:PasswordSalt"];
            var context = Services.GetRequiredService<DataContext>();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.AddRoles();
            context.AddUsers(passwordSalt);
        }
        public void TruncateDatabase()
        {
            var context = Services.GetRequiredService<DataContext>();

            context.Database.EnsureDeleted();
        }
    }
}