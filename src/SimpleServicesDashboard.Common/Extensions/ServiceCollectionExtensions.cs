using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleServicesDashboard.Common.Configuration;

namespace SimpleServicesDashboard.Common.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configure application custom configuration with default validation with Data Annotations.
    /// </summary>
    /// <param name="services">Services collection.</param>
    /// <param name="configuration">Application configuration.</param>
    public static void AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddWithValidation<HealthCheckOptions>(nameof(HealthCheckOptions));
        services.AddWithValidation<MemoryCheckOptions>(nameof(MemoryCheckOptions));
        services.AddWithValidation<ServicesConfigurationOptions>(nameof(ServicesConfigurationOptions));
    }

    /// <summary>
    /// Configure application custom configuration wth validation by using FluentValidation.
    /// </summary>
    /// <param name="services">Services collection.</param>
    public static void AddConfigurationWithFluentValidation(this IServiceCollection services)
    {
        services.AddWithValidation<HealthCheckOptions, HealthCheckOptionsValidator>(nameof(HealthCheckOptions));
        services.AddWithValidation<MemoryCheckOptions, MemoryCheckOptionsValidator>(nameof(MemoryCheckOptions));
        services.AddWithValidation<ServicesConfigurationOptions, ServicesConfigurationOptionsValidator>(nameof(ServicesConfigurationOptions));
    }
}