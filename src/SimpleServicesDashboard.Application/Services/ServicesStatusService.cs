using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SimpleServicesDashboard.Application.Common.Interfaces;
using SimpleServicesDashboard.Application.Models;
using SimpleServicesDashboard.Application.Services.Interfaces;
using SimpleServicesDashboard.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleServicesDashboard.Application.Services;

/// <summary>
/// Service to manage the statuses for external service monitored in he application.
/// </summary>
public sealed class ServicesStatusService : IServicesStatusService
{
    private readonly ILogger<ServicesStatusService> _logger;
    private readonly ServicesConfigurationOptions _servicesConfiguration;
    private readonly IServiceAccessFactory _serviceAccessFactory;

    public ServicesStatusService(ILogger<ServicesStatusService> logger,
        IOptions<ServicesConfigurationOptions> servicesConfiguration,
        IServiceAccessFactory serviceAccessFactory)
    {
        _logger = logger;
        _serviceAccessFactory = serviceAccessFactory;
        _servicesConfiguration = servicesConfiguration.Value;
    }

    public async Task<ServicesStatusResponse> GetServicesStatusAsync()
    {
        try
        {
            List<Task<List<ServiceStatusResponse>>> tasks = new();

            foreach (var servicesConfiguration in _servicesConfiguration.Services)
            {
                tasks.Add(CheckServiceCode(servicesConfiguration.Code)
                    ? BuildServiceResponsesAsync(servicesConfiguration)
                    : BuildEmptyResultAsync(servicesConfiguration));
            }

            var taskResults = await Task.WhenAll(tasks);

            var result = new ServicesStatusResponse();

            foreach (var taskResult in taskResults)
            {
                result.Statuses.AddRange(taskResult);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during reading the services statuses");

            throw;
        }
    }

    public async Task<ServicesStatusResponse> GetServicesStatusAsync(string code)
    {
        try
        {
            var result = new ServicesStatusResponse();

            var serviceConfiguration = _servicesConfiguration.Services.FirstOrDefault(x => x.Code == code);

            if (serviceConfiguration != null)
            {
                if (CheckServiceCode(code))
                {
                    result.Statuses.AddRange(await BuildServiceResponsesAsync(serviceConfiguration));
                }
                else
                {
                    result.Statuses.AddRange(BuildEmptyResult(serviceConfiguration));
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during reading the service statuses by the code for all environments");

            throw;
        }
    }

    public async Task<ServiceStatusResponse> GetServiceStatusAsync(string code, string environment)
    {
        var serviceConfiguration = _servicesConfiguration.Services.FirstOrDefault(x => x.Code == code);

        if (serviceConfiguration != null)
        {
            var serviceEnvironment = serviceConfiguration.Environments.FirstOrDefault(x => x.Environment == environment);

            if (serviceEnvironment != null)
            {
                var aboutUrl = serviceEnvironment.BaseUrl + serviceConfiguration.AboutEndpoint;

                return CheckServiceCode(serviceConfiguration.Code)
                    ? await BuildServiceResponseAsync(aboutUrl, environment, serviceConfiguration.Name, serviceConfiguration.Code, serviceEnvironment.BaseUrl)
                    : BuildEmptyResult(serviceConfiguration, serviceEnvironment);
            }
        }

        return CreateNotFoundResponse(code, environment);
    }

    public async Task<ServiceDetailsResponse> GetServiceDetailsAsync(string code, string environment)
    {
        var serviceConfiguration = _servicesConfiguration.Services.FirstOrDefault(x => x.Code == code);

        if (serviceConfiguration != null)
        {
            var serviceEnvironment = serviceConfiguration.Environments.FirstOrDefault(x => x.Environment == environment);
            var environmentName = _servicesConfiguration.Environments.FirstOrDefault(x => x.Code == environment)?.Name ?? environment;

            if (serviceEnvironment != null)
            {
                var aboutUrl = serviceEnvironment.BaseUrl + serviceConfiguration.AboutEndpoint;
                var healthUrl = serviceEnvironment.BaseUrl + serviceConfiguration.HealthEndpoint;
                var healthcheckDashboardUrl = serviceEnvironment.BaseUrl + serviceConfiguration.HealthcheckDashboardEndpoint;
                var swaggerUrl = serviceEnvironment.BaseUrl + serviceConfiguration.SwaggerEndpoint;

                return CheckServiceCode(serviceConfiguration.Code)
                    ? await BuildServiceDetailsResponseAsync(aboutUrl, healthUrl, healthcheckDashboardUrl, swaggerUrl, environmentName, serviceConfiguration.Name, serviceConfiguration.Code)
                    : BuildEmptyServiceDetailsResult(serviceConfiguration, environmentName);
            }
        }

        return new ServiceDetailsResponse
        {
            JsonData = ""
        };
    }

    public Task<ServicesDescriptionResponse> GetServicesDescriptionAsync()
    {
        var result = _servicesConfiguration.Services.Select(service => new ServiceDescription
        {
            Name = service.Name,
            Code = service.Code,
            Environments = string.Join(',', service.Environments.Select(x => x.Environment))
        }).ToList();

        return Task.FromResult(new ServicesDescriptionResponse
        {
            ServicesInfo = result
        });
    }

    #region Common methods to get the service status.

    private async Task<List<ServiceStatusResponse>> BuildServiceResponsesAsync(ServiceConfiguration servicesConfiguration)
    {
        List<Task<ServiceStatusResponse>> tasks = new();

        foreach (var servicesConfigurationEnvironment in servicesConfiguration.Environments)
        {
            var aboutUrl = servicesConfigurationEnvironment.BaseUrl + servicesConfiguration.AboutEndpoint;

            tasks.Add(BuildServiceResponseAsync(aboutUrl, servicesConfigurationEnvironment.Environment,
                servicesConfiguration.Name, servicesConfiguration.Code, servicesConfigurationEnvironment.BaseUrl));
        }

        var taskResults = await Task.WhenAll(tasks);

        return taskResults.ToList();
    }

    private async Task<ServiceStatusResponse> BuildServiceResponseAsync(string aboutUrl, string environment, string name, string code, string baseUrl)
    {
        var serviceAccess = _serviceAccessFactory.GetServiceAccess(code);
        var response = await serviceAccess.GetServiceStatus(aboutUrl);

        if (response is not null)
        {
            return new ServiceStatusResponse
            {
                Name = $"{name} ({environment})",
                Code = code,
                Environment = environment,
                Created = response.Created,
                MachineName = response.AppInfo?.MachineName ?? "",
                EnvironmentName = response.AppInfo?.EnvironmentName ?? "",
                ReleaseDate = response.AppInfo?.ReleaseDate ?? DateTime.UtcNow,
                AppStartTime = response.AppInfo?.AppStartTime ?? DateTime.UtcNow,
                Version = response.AppInfo?.Version ?? "",
                BaseUrl = baseUrl
            };
        }

        return CreateDefaultResponse(code, name, environment);
    }

    private async Task<ServiceDetailsResponse> BuildServiceDetailsResponseAsync(string aboutUrl, string healthUrl, string healthcheckDashboardUrl, string swaggerUrl, string environment, string name, string code)
    {
        var serviceAccess = _serviceAccessFactory.GetServiceAccess(code);
        var result = await serviceAccess.GetServiceStatus(aboutUrl);

        return new ServiceDetailsResponse
        {
            AboutUrl = aboutUrl,
            HealthUrl = healthUrl,
            HealthcheckDashboardUrl = healthcheckDashboardUrl,
            SwaggerdUrl = swaggerUrl,
            ServiceName = name,
            Environment = environment,
            JsonData = SerializeResultToJson(result)
        };
    }

    #endregion

    #region Get services statuses.

    private static IEnumerable<ServiceStatusResponse> BuildEmptyResult(ServiceConfiguration servicesConfiguration)
    {
        return servicesConfiguration.Environments.Select(servicesConfigurationEnvironment =>
            new ServiceStatusResponse
            {
                Name = $"{servicesConfiguration.Name} ({servicesConfigurationEnvironment.Environment})",
                Code = servicesConfiguration.Code,
                Environment = servicesConfigurationEnvironment.Environment,
                Created = DateTime.UtcNow,
                EnvironmentName = servicesConfigurationEnvironment.Environment,
                BaseUrl = servicesConfigurationEnvironment.BaseUrl
            });
    }

    private static Task<List<ServiceStatusResponse>> BuildEmptyResultAsync(ServiceConfiguration servicesConfiguration)
    {
        return Task.FromResult(servicesConfiguration.Environments.Select(servicesConfigurationEnvironment =>
            new ServiceStatusResponse
            {
                Name = $"{servicesConfiguration.Name} ({servicesConfigurationEnvironment.Environment})",
                Code = servicesConfiguration.Code,
                Environment = servicesConfigurationEnvironment.Environment,
                Created = DateTime.UtcNow,
                EnvironmentName = servicesConfigurationEnvironment.Environment,
                BaseUrl = servicesConfigurationEnvironment.BaseUrl
            }).ToList());
    }

    private static ServiceStatusResponse BuildEmptyResult(ServiceConfiguration servicesConfiguration, ServiceEnvironment serviceEnvironment)
    {
        return new ServiceStatusResponse
        {
            Name = $"{servicesConfiguration.Name} ({serviceEnvironment.Environment})",
            Code = servicesConfiguration.Code,
            Environment = serviceEnvironment.Environment,
            Created = DateTime.UtcNow,
            EnvironmentName = serviceEnvironment.Environment,
            BaseUrl = serviceEnvironment.BaseUrl
        };
    }

    private static ServiceStatusResponse CreateDefaultResponse(string code, string name, string environment)
    {
        return new ServiceStatusResponse
        {
            Name = $"{name} ({environment})",
            Code = code,
            Environment = environment,
            Created = DateTime.UtcNow
        };
    }

    private static ServiceStatusResponse CreateNotFoundResponse(string code, string environment)
    {
        return new ServiceStatusResponse
        {
            Name = "Not Found!",
            Code = code,
            EnvironmentName = environment,
            Environment = environment
        };
    }

    #endregion

    #region Get service details data.

    private static ServiceDetailsResponse BuildEmptyServiceDetailsResult(ServiceConfiguration serviceConfiguration, string environment)
    {
        return new ServiceDetailsResponse
        {
            AboutUrl = string.Empty,
            HealthUrl = string.Empty,
            HealthcheckDashboardUrl = string.Empty,
            SwaggerdUrl = string.Empty,
            ServiceName = serviceConfiguration.Name,
            Environment = environment,
            JsonData = string.Empty
        };
    }

    private static string SerializeResultToJson(object? result)
    {
        return JsonSerializer.Serialize(result, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }

    #endregion

    #region Helpers.

    private static bool CheckServiceCode(string code)
    {
        return code is "email";
    }

    #endregion
}