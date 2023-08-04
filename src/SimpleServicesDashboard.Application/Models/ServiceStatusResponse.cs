using System;
using System.Collections.Generic;

namespace SimpleServicesDashboard.Application.Models;

public sealed class ServicesStatusResponse
{
    public List<ServiceStatusResponse> Statuses { get; set; }

    public ServicesStatusResponse()
    {
        Statuses = new List<ServiceStatusResponse>();
    }
}

public sealed class ServiceStatusResponse
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string Environment { get; set; }

    public DateTime Created { get; set; }

    public string MachineName { get; set; }
    public string EnvironmentName { get; set; }
    public DateTime ReleaseDate { get; set; }
    public DateTime AppStartTime { get; set; }
    public string Version { get; set; }

    public string BaseUrl { get; set; }
}

public sealed class ServiceDetailsResponse
{
    public string Url { get; set; }
    public string ServiceName { get; set; }
    public string Environment { get; set; }
    public string JsonData { get; set; }
}