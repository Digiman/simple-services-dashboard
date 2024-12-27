using Microsoft.Extensions.Diagnostics.HealthChecks;
using SimpleServicesDashboard.Common.Configuration;

namespace SimpleServicesDashboard.Api.Infrastructure.HealthCheck;

/// <summary>
/// Extension to add additional custom health checks.
/// </summary>
public static class HealthCheckExtensions
{
    public static IHealthChecksBuilder AddMemoryHealthCheck(this IHealthChecksBuilder builder, HealthStatus? failureStatus = null,
        IEnumerable<string>? tags = default, long? thresholdInBytes = null)
    {
        // Register a check of type GCInfo.
        builder.AddCheck<MemoryHealthCheck>(MemoryHealthCheck.Name, failureStatus ?? HealthStatus.Degraded, tags, timeout: null);

        // Configure named options to pass the threshold into the check.
        if (thresholdInBytes.HasValue)
        {
            builder.Services.Configure<MemoryCheckOptions>(nameof(MemoryCheckOptions), options => { options.Threshold = thresholdInBytes.Value; });
        }

        return builder;
    }
}