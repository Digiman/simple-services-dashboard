using System;

namespace SimpleServicesDashboard.Application.Models
{
    /// <summary>
    /// Application status and details for each connected service to use to identify current configuration.
    /// </summary>
    public sealed class StatusResponse
    {
        /// <summary>
        /// Datetime when status response created (request processing time).
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Application information.
        /// </summary>
        public AppInfo AppInfo { get; set; }
    }

    /// <summary>
    /// Application info.
    /// </summary>
    public sealed class AppInfo
    {
        public string MachineName { get; set; }
        public string EnvironmentName { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime AppStartTime { get; set; }
        public string Version { get; set; }
    }
}