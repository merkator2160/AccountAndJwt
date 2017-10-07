using AccountAndJwt.Middleware.AutoMapper;
using AutoMapper;
using System.Reflection;
using Xunit;

namespace AccountAndJwt.Tests.AutoMapper
{
    public class AutoMapperTest
    {
        [Fact]
        public void TheWholeAutomapperConfigurationTest()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles(typeof(AutoMapperMiddleware).GetTypeInfo().Assembly);     // Dynamically load all configurations
            });

            mapperConfiguration.CompileMappings();
            mapperConfiguration.AssertConfigurationIsValid();
        }
    }
}