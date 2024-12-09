namespace SimpleServicesDashboard.Api.Models;

public sealed class DashboardViewModel
{
    public required List<ServiceViewModel> Services { get; set; }

    public required Dictionary<string, string> Environments { get; set; }
}

public sealed class ServiceViewModel
{
    public required string Code { get; set; }
    public required string Name { get; set; }

    public required Dictionary<string, ServiceEnvironmentViewModel> Environments { get; set; }
}

public sealed class ServiceEnvironmentViewModel
{
    public required string Name { get; set; }
    public DateTime Created { get; set; }
    public string? MachineName { get; set; }
    public string? EnvironmentName { get; set; }
    public DateTime ReleaseDate { get; set; }
    public DateTime AppStartTime { get; set; }
    public string? Version { get; set; }
    public string? BaseUrl { get; set; }
}

public sealed class ServiceDetailsViewModel
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string EnvironmentCode { get; set; }
    public required ServiceEnvironmentViewModel ServiceDetailsModel { get; set; }
}