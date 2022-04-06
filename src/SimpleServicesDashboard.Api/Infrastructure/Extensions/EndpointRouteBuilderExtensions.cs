using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SimpleServicesDashboard.Api.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for endpoint router builder.
    /// </summary>
    public static class EndpointRouteBuilderExtensions
    {
        public static void AddHealthcheckEndpoints(this IEndpointRouteBuilder endpoints,
            Common.Configuration.HealthCheckOptions healthCheckConfig)
        {
            if (healthCheckConfig.HealthCheckUiEnabled)
            {
                // add Health Check UI
                endpoints.MapHealthChecksUI(config =>
                {
                    config.UIPath = "/healthcheck-dashboard";
                });
            }

            // add HealthCheck simple endpoint - only
            endpoints.MapHealthChecks("/healthcheck", new HealthCheckOptions
            {
                Predicate = (check) => check.Tags.Contains("ready")
            });

            // all health checks here with details
            endpoints.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            // add custom health checks
            // Readiness endpoint
            endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                },
                Predicate = (check) => check.Tags.Contains("ready"),
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                AllowCachingResponses = false
            });

            // Liveness endpoint
            endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
            {
                Predicate = (check) => !check.Tags.Contains("ready"),
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                AllowCachingResponses = false
            });
        }
    }
}