using AutoMapper;
using SimpleServicesDashboard.Application.Mapping;
using Xunit;

namespace SimpleServicesDashboard.Application.Tests.Mappings
{
    /// <summary>
    /// Simple tests to validate the configuration for mappings between data models.
    /// </summary>
    public sealed class MappingsConfigurationTests
    {
        [Fact]
        public void ConfigurationShouldBeValid()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DtoV1MappingProfile());
            });

            config.AssertConfigurationIsValid();
        }
    }
}