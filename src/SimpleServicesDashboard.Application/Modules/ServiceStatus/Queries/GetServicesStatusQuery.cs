using MediatR;
using Microsoft.Extensions.Logging;
using SimpleServicesDashboard.Application.Models;
using SimpleServicesDashboard.Application.Services.Interfaces;
using SimpleServicesDashboard.Common.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleServicesDashboard.Application.Modules.ServiceStatus.Queries;

/// <summary>
/// Get statuses for all the services.
/// </summary>
public sealed class GetServicesStatusQuery : IRequest<ServicesStatusResponse>
{ }

public sealed class GetServicesStatusQueryHandler : IRequestHandler<GetServicesStatusQuery, ServicesStatusResponse>
{
    private readonly ILogger<GetServicesStatusQueryHandler> _logger;
    private readonly IServicesStatusService _servicesStatusService;

    public GetServicesStatusQueryHandler(ILogger<GetServicesStatusQueryHandler> logger, IServicesStatusService servicesStatusService)
    {
        _logger = logger;
        _servicesStatusService = servicesStatusService;
    }

    public async Task<ServicesStatusResponse> Handle(GetServicesStatusQuery request, CancellationToken cancellationToken)
    {
        using var scope = _logger.BeginNamedScope("GetServicesStatus");
        return await _servicesStatusService.GetServicesStatusAsync();
    }
}