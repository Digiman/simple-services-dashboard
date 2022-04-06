using System;
using System.Collections.Generic;

namespace SimpleServicesDashboard.Api.Models
{
    public sealed class DashboardViewModel
    {
        public List<ServiceViewModel> Services { get; set; }

        public Dictionary<string, string> Environments { get; set; }
    }

    public sealed class ServiceViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public Dictionary<string, ServiceEnvironmentViewModel> Environments { get; set; }
    }

    public sealed class ServiceEnvironmentViewModel
    {
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public string MachineName { get; set; }
        public string EnvironmentName { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime AppStartTime { get; set; }
        public string Version { get; set; }
        public string BaseUrl { get; set; }
    }

    public sealed class ServiceDetailsViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string EnvironmentCode { get; set; }
        public ServiceEnvironmentViewModel ServiceDetailsModel { get; set; }
    }
}