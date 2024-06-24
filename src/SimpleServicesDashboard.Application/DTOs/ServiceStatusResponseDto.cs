using System;
using System.Collections.Generic;

namespace SimpleServicesDashboard.Application.DTOs;

public sealed class ServicesStatusResponseDto
{
    public List<ServiceStatusResponseDto>? Statuses { get; set; }
}

public sealed class ServiceStatusResponseDto
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Environment { get; set; }

    /// <summary>
    /// Datetime when status response created (request processing time).
    /// </summary>
    public DateTime Created { get; set; }

    public string? MachineName { get; set; }
    public string? EnvironmentName { get; set; }
    public DateTime ReleaseDate { get; set; }
    public DateTime AppStartTime { get; set; }
    public string? Version { get; set; }

    public string? BaseUrl { get; set; }
}