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
    public string Code { get; set; }

    /// <summary>
    /// Full environment name.
    /// </summary>
    public string Name { get; set; }
}

/// <summary>
/// Configuration for the service.
/// </summary>
public sealed class ServiceConfiguration
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string HealthEndpoint { get; set; }
    public string AboutEndpoint { get; set; }

    /// <summary>
    /// Specific configuration for each available and monitored environment.
    /// </summary>
    public List<ServiceEnvironment> Environments { get; set; }
}

/// <summary>
/// Details for service environment (unique values).
/// </summary>
public sealed class ServiceEnvironment
{
    /// <summary>
    /// Environment short code (dev, qa, uat, prod).
    /// </summary>
    public string Environment { get; set; }

    /// <summary>
    /// Base URL to the service.
    /// </summary>
    public string BaseUrl { get; set; }
}

public sealed class ServicesConfigurationOptionsValidator : AbstractValidator<ServicesConfigurationOptions>
{
    public ServicesConfigurationOptionsValidator()
    {
    }
}