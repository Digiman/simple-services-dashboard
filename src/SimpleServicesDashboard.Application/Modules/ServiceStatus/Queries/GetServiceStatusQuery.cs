using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SimpleServicesDashboard.Application.Models;
using SimpleServicesDashboard.Application.Services.Interfaces;
using SimpleServicesDashboard.Common.Extensions;

namespace SimpleServicesDashboard.Application.Modules.ServiceStatus.Queries
{
    /// <summary>
    /// Get statuses for specific services.
    /// </summary>
    public sealed class GetServiceStatusQuery : IRequest<ServiceStatusResponse>
    {
        public string Code { get; set; }

        public string Environment { get; set; }
    }

    public sealed class GetServiceStatusQueryHandler : IRequestHandler<GetServiceStatusQuery, ServiceStatusResponse>
    {
        private readonly ILogger<GetServiceStatusQueryHandler> _logger;
        private readonly IServicesStatusService _servicesStatusService;

        public GetServiceStatusQueryHandler(ILogger<GetServiceStatusQueryHandler> logger, IServicesStatusService servicesStatusService)
        {
            _logger = logger;
            _servicesStatusService = servicesStatusService;
        }

        public async Task<ServiceStatusResponse> Handle(GetServiceStatusQuery request, CancellationToken cancellationToken)
        {
            using var scope = _logger.BeginNamedScope("GetServiceStatus",
                ("Code", request.Code), ("Environment", request.Environment));
            return await _servicesStatusService.GetServiceStatusAsync(request.Code, request.Environment);
        }
    }
}