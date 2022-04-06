using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using SimpleServicesDashboard.Application.Common.Interfaces;
using SimpleServicesDashboard.Application.Models;
using SimpleServicesDashboard.Application.Services.Interfaces;
using SimpleServicesDashboard.Common.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SimpleServicesDashboard.Application.Services
{
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
                var tasks = new List<Task>();

                foreach (var servicesConfiguration in _servicesConfiguration.Services)
                {
                    if (CheckServiceCode(servicesConfiguration.Code))
                    {
                        tasks.Add(BuildServiceResponses(servicesConfiguration));
                    }
                    else
                    {
                        tasks.Add(BuildEmptyResultAsync(servicesConfiguration));
                    }
                }

                await Task.WhenAll(tasks);

                var result = new ServicesStatusResponse();

                foreach (var task in tasks)
                {
                    result.Statuses.AddRange(((Task<List<ServiceStatusResponse>>)task).Result);
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
                        result.Statuses.AddRange(await BuildServiceResponses(serviceConfiguration));
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
                    var url = serviceEnvironment.BaseUrl + serviceConfiguration.AboutEndpoint;

                    return CheckServiceCode(serviceConfiguration.Code)
                        ? await BuildServiceResponse(url, environment, serviceConfiguration.Name, serviceConfiguration.Code, serviceEnvironment.BaseUrl)
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
                    var url = serviceEnvironment.BaseUrl + serviceConfiguration.AboutEndpoint;

                    return CheckServiceCode(serviceConfiguration.Code)
                        ? await BuildServiceDetailsResponse(url, environmentName, serviceConfiguration.Name, serviceConfiguration.Code)
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

        private async Task<List<ServiceStatusResponse>> BuildServiceResponses(ServiceConfiguration servicesConfiguration)
        {
            var tasks = new List<Task>();

            foreach (var servicesConfigurationEnvironment in servicesConfiguration.Environments)
            {
                var url = servicesConfigurationEnvironment.BaseUrl + servicesConfiguration.AboutEndpoint;
                tasks.Add(BuildServiceResponse(url, servicesConfigurationEnvironment.Environment,
                    servicesConfiguration.Name, servicesConfiguration.Code, servicesConfigurationEnvironment.BaseUrl));
            }

            await Task.WhenAll(tasks);

            var result = new List<ServiceStatusResponse>();

            foreach (var task in tasks)
            {
                result.Add(((Task<ServiceStatusResponse>)task).Result);
            }

            return result;
        }

        private async Task<ServiceStatusResponse> BuildServiceResponse(string url, string environment, string name, string code, string baseUrl)
        {
            var serviceAccess = _serviceAccessFactory.GetServiceAccess(code);
            var response = await serviceAccess.GetServiceStatus(url);

            if (response != null)
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

        private async Task<ServiceDetailsResponse> BuildServiceDetailsResponse(string url, string environment, string name, string code)
        {
            var serviceAccess = _serviceAccessFactory.GetServiceAccess(code);
            var result = await serviceAccess.GetServiceStatus(url);

            return new ServiceDetailsResponse
            {
                Url = url,
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

        private static Task<IEnumerable<ServiceStatusResponse>> BuildEmptyResultAsync(ServiceConfiguration servicesConfiguration)
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
                }));
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
                Url = "",
                ServiceName = serviceConfiguration.Name,
                Environment = environment,
                JsonData = ""
            };
        }

        private static string SerializeResultToJson(object result)
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
}