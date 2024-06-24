using Microsoft.Extensions.Configuration;
using SimpleServicesDashboard.Common.Configuration;

namespace SimpleServicesDashboard.Common.Extensions;

/// <summary>
/// Simple extensions for configuration.
/// </summary>
public static class ConfigurationExtensions
{
    public static HealthCheckOptions? GetHealthCheckConfiguration(this IConfiguration configuration)
    {
        return configuration.GetSection(nameof(HealthCheckOptions)).Get<HealthCheckOptions>();
    }

    public static MemoryCheckOptions? GetMemoryCheckConfiguration(this IConfiguration configuration)
    {
        return configuration.GetSection(nameof(MemoryCheckOptions)).Get<MemoryCheckOptions>();
    }

    public static ServicesConfigurationOptions? GetServicesConfigurationOptions(this IConfiguration configuration)
    {
        return configuration.GetSection(nameof(ServicesConfigurationOptions)).Get<ServicesConfigurationOptions>();
    }
}