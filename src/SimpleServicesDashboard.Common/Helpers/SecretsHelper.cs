using System;

namespace SimpleServicesDashboard.Common.Helpers
{
    /// <summary>
    /// Helper to work with credentials.
    /// </summary>
    public static class SecretsHelper
    {
        /// <summary>
        /// Get Terra Live and AWS environment prefix to use in secrets.
        /// </summary>
        /// <returns>Returns env prefix. Possible values: dev, qa, uat, prod.</returns>
        public static string GetEnvironmentPrefix()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return env switch
            {
                "Development" => "dev",
                "QA" => "qa",
                "UAT" => "uat",
                "Production" => "prod",
                _ => "dev"
            };
        }
    }
}