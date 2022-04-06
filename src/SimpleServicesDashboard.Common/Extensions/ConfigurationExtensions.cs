using SimpleServicesDashboard.Common.Configuration;
using Microsoft.Extensions.Configuration;

namespace SimpleServicesDashboard.Common.Extensions
{
    public static class ConfigurationExtensions
    {
        public static HealthCheckOptions GetHealthCheckConfiguration(this IConfiguration configuration)
        {
            return configuration.GetSection(nameof(HealthCheckOptions)).Get<HealthCheckOptions>();
        }

        public static ServicesConfigurationOptions GetServicesConfigurationOptions(this IConfiguration configuration)
        {
            return configuration.GetSection(nameof(ServicesConfigurationOptions)).Get<ServicesConfigurationOptions>();
        }
    }
}