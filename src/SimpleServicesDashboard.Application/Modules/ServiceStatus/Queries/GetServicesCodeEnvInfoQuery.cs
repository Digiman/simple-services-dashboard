using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SimpleServicesDashboard.Application.Models;
using SimpleServicesDashboard.Application.Services.Interfaces;
using SimpleServicesDashboard.Common.Extensions;

namespace SimpleServicesDashboard.Application.Modules.ServiceStatus.Queries;

/// <summary>
/// Get the services descriptions to see what the code, services and environments available by configuration.
/// </summary>
public sealed class GetServicesCodeEnvInfoQuery : IRequest<ServicesDescriptionResponse>
{ }

public sealed class GetServicesCodeEnvInfoQueryHandler : IRequestHandler<GetServicesCodeEnvInfoQuery, ServicesDescriptionResponse>
{
    private readonly ILogger<GetServicesCodeEnvInfoQueryHandler> _logger;
    private readonly IServicesStatusService _servicesStatusService;

    public GetServicesCodeEnvInfoQueryHandler(ILogger<GetServicesCodeEnvInfoQueryHandler> logger, IServicesStatusService servicesStatusService)
    {
        _logger = logger;
        _servicesStatusService = servicesStatusService;
    }

    public async Task<ServicesDescriptionResponse> Handle(GetServicesCodeEnvInfoQuery request, CancellationToken cancellationToken)
    {
        using var scope = _logger.BeginNamedScope("GetServicesCodeEnvInfo");
        return await _servicesStatusService.GetServicesDescriptionAsync();
    }
}