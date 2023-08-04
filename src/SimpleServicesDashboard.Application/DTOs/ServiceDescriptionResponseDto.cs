using System.Collections.Generic;

namespace SimpleServicesDashboard.Application.DTOs;

public sealed class ServicesDescriptionResponseDto
{
    public List<ServiceDescriptionDto> ServicesInfo { get; set; }
}

public sealed class ServiceDescriptionDto
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string Environments { get; set; }
}