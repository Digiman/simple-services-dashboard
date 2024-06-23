using System.Collections.Generic;

namespace SimpleServicesDashboard.Application.Models;

public sealed class ServicesDescriptionResponse
{
    public List<ServiceDescription>? ServicesInfo { get; set; }
}

public sealed class ServiceDescription
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Environments { get; set; }
}