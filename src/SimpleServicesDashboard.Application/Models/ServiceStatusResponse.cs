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
    public required string Name { get; set; }
    public required string Code { get; set; }
    public required string Environment { get; set; }

    public DateTime Created { get; set; }

    public string? MachineName { get; set; }
    public string? EnvironmentName { get; set; }
    public DateTime ReleaseDate { get; set; }
    public DateTime AppStartTime { get; set; }
    public string? Version { get; set; }

    public string? BaseUrl { get; set; }
}

public sealed class ServiceDetailsResponse
{
    public string? AboutUrl { get; set; }
    public string? HealthUrl { get; set; }
    public string? HealthcheckDashboardUrl { get; set; }
    public string? SwaggerdUrl { get; set; }
    public string? ServiceName { get; set; }
    public string? Environment { get; set; }
    public string? JsonData { get; set; }
}