using System;
using Serilog.Configuration;
using Serilog.Enrichers.Custom.Enrichers;

namespace Serilog.Enrichers.Custom
{
    /// <summary>
    /// Custom extensions for Serilog to log additional data from the application and environment.
    /// </summary>
    public static class CustomLoggerConfigurationExtensions
    {
        /// <summary>
        /// Enrich log events with a EnvironmentName property containing the value of the ASPNETCORE_ENVIRONMENT or DOTNET_ENVIRONMENT environment variable.
        /// </summary>
        /// <param name="enrichmentConfiguration">Logger enrichment configuration.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration WithEnvironmentName(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null)
            {
                throw new ArgumentNullException(nameof(enrichmentConfiguration));
            }
            
            return enrichmentConfiguration.With<EnvironmentNameEnricher>();
        }
        
        /// <summary>
        /// Enrich log events with an EC2-INSTANCE-ID property containing the current EC2 instance Id.
        /// </summary>
        /// <param name="enrichmentConfiguration">Logger enrichment configuration.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration WithEc2InstanceId(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null)
            {
                throw new ArgumentNullException(nameof(enrichmentConfiguration));
            }

            return enrichmentConfiguration.With<Ec2InstanceIdEnricher>();
        }
    }
}