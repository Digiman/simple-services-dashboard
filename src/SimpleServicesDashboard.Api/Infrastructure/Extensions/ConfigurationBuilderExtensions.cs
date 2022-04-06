using System;
using Microsoft.Extensions.Configuration;

namespace SimpleServicesDashboard.Api.Infrastructure.Extensions
{
    /// <summary>
    /// Extension for the Configuration Builder.
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// Add secret from the AWS Secrets Manager.
        /// </summary>
        /// <param name="builder">Configuration builder.</param>
        /// <param name="secretName">Secret name.</param>
        /// <param name="prefix">Custom prefix for the secret. If empty - will be use by default.</param>
        public static void AddSecretsManagerSecret(this IConfigurationBuilder builder, string secretName, string prefix)
        {
            builder.AddSystemsManager(options =>
            {
                options.Path = $"/aws/reference/secretsmanager/{secretName}";
                options.ReloadAfter = TimeSpan.FromMinutes(5);
                options.Optional = true;

                if (!string.IsNullOrEmpty(prefix))
                {
                    options.Prefix = prefix;
                }
            });
        }
    }
}