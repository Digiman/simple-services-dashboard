using System.Collections.Generic;
using FluentValidation;

namespace SimpleServicesDashboard.Common.Configuration;

/// <summary>
/// Configuration for services to monitor them in the application.
/// </summary>
public sealed class ServicesConfigurationOptions
{
    /// <summary>
    /// Environment configurations.
    /// </summary>
    public List<EnvironmentConfiguration> Environments { get; set; }

    /// <summary>
    /// List of the services to monitor with configuration.
    /// </summary>
    public List<ServiceConfiguration> Services { get; set; }
}

/// <summary>
/// Configuration for environments.
/// </summary>
public sealed class EnvironmentConfiguration
{
    /// <summary>
    /// Short environment code.
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// Full environment name.
    /// </summary>
    public required string Name { get; set; }
}

/// <summary>
/// Configuration for the service.
/// </summary>
public sealed class ServiceConfiguration
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string HealthEndpoint { get; set; } = "/health";
    public required string AboutEndpoint { get; set; } = "/api/v1/status";
    public required string HealthcheckDashboardEndpoint { get; set; } = "/healthcheck-dashboard";
    public required string SwaggerEndpoint { get; set; } = "/swagger";

    /// <summary>
    /// Specific configuration for each available and monitored environment.
    /// </summary>
    public required List<ServiceEnvironment> Environments { get; set; }
}

/// <summary>
/// Details for service environment (unique values).
/// </summary>
public sealed class ServiceEnvironment
{
    /// <summary>
    /// Environment short code (dev, qa, uat, prod).
    /// </summary>
    public required string Environment { get; set; }

    /// <summary>
    /// Base URL to the service.
    /// </summary>
    public required string BaseUrl { get; set; }
}

public sealed class ServicesConfigurationOptionsValidator : AbstractValidator<ServicesConfigurationOptions>
{
    public ServicesConfigurationOptionsValidator()
    {
    }
}