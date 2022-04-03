using Microsoft.Extensions.Configuration;

namespace AccountAndJwt.Common.Config
{
    public static class CustomConfigurationProvider
    {
        public const String DefaultEnvironmentVariableName = "ASPNETCORE_ENVIRONMENT";


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        public static IConfigurationRoot CollectEnvironmentRelatedConfiguration()
        {
            return CollectEnvironmentRelatedConfiguration(DefaultEnvironmentVariableName);
        }
        public static IConfigurationRoot CollectEnvironmentRelatedConfiguration(String environmentVariableName)
        {
            var environment = Environment.GetEnvironmentVariable(environmentVariableName);
            if (String.IsNullOrWhiteSpace(environment))
                throw new ArgumentNullException($"Environment variable was not found: \"{environmentVariableName}\"!");

            return CreateConfiguration(environment, Directory.GetCurrentDirectory());
        }
        public static IConfigurationRoot CreateConfiguration(String environment, String basePath)
        {
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: false);

            return builder.Build();
        }
    }
}