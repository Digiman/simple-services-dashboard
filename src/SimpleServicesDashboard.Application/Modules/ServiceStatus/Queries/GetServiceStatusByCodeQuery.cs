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
    public sealed class GetServiceStatusByCodeQuery : IRequest<ServicesStatusResponse>
    {
        public string Code { get; set; }
    }

    public sealed class GetServiceStatusByCodeQueryHandler : IRequestHandler<GetServiceStatusByCodeQuery, ServicesStatusResponse>
    {
        private readonly ILogger<GetServiceStatusByCodeQueryHandler> _logger;
        private readonly IServicesStatusService _servicesStatusService;

        public GetServiceStatusByCodeQueryHandler(ILogger<GetServiceStatusByCodeQueryHandler> logger, IServicesStatusService servicesStatusService)
        {
            _logger = logger;
            _servicesStatusService = servicesStatusService;
        }

        public async Task<ServicesStatusResponse> Handle(GetServiceStatusByCodeQuery request, CancellationToken cancellationToken)
        {
            using var scope = _logger.BeginNamedScope("GetServiceStatusByCode", ("Code", request.Code));
            return await _servicesStatusService.GetServicesStatusAsync(request.Code);
        }
    }
}