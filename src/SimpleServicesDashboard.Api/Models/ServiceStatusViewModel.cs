namespace SimpleServicesDashboard.Api.Models;

/// <summary>
/// Model to see the service status details.
/// </summary>
public sealed class ServiceStatusViewModel
{
    public required string? AboutUrl { get; set; }
    public required string? HealthUrl { get; set; }
    public required string? HealthcheckDashboardUrl { get; set; }
    public required string? SwaggerdUrl { get; set; }
    public string? ServiceName { get; set; }
    public string? Environment { get; set; }

    /// <summary>
    /// Status data from the service endpoint in JSON format.
    /// </summary>
    public string? StatusData { get; set; }
}