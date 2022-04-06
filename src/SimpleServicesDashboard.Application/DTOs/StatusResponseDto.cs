using System;

namespace SimpleServicesDashboard.Application.DTOs
{
    /// <summary>
    /// Application status and details for each connected service to use to identify current configuration.
    /// </summary>
    public sealed class StatusResponseDto
    {
        /// <summary>
        /// Datetime when status response created (request processing time).
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Application information.
        /// </summary>
        public AppInfoDto AppInfo { get; set; }
    }

    /// <summary>
    /// Application info.
    /// </summary>
    public sealed class AppInfoDto
    {
        public string MachineName { get; set; }
        public string EnvironmentName { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime AppStartTime { get; set; }
        public string Version { get; set; }
    }
}