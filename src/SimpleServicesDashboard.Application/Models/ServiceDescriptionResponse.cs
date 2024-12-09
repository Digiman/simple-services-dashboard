namespace SimpleServicesDashboard.Application.Models;

public sealed class ServicesDescriptionResponse
{
    public required List<ServiceDescription>? ServicesInfo { get; set; }
}

public sealed class ServiceDescription
{
    public required string Name { get; set; }
    public required string Code { get; set; }
    public required string Environments { get; set; }
}