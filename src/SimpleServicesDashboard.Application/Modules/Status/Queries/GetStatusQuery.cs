using System.Threading;
using System.Threading.Tasks;
using SimpleServicesDashboard.Application.Models;
using SimpleServicesDashboard.Application.Services.Interfaces;
using SimpleServicesDashboard.Common.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SimpleServicesDashboard.Application.Modules.Status.Queries
{
    /// <summary>
    /// Get application status request.
    /// </summary>
    public sealed class GetStatusQuery : IRequest<StatusResponse>
    { }

    public sealed class GetStatusQueryHandler : IRequestHandler<GetStatusQuery, StatusResponse>
    {
        private readonly ILogger<GetStatusQueryHandler> _logger;
        private readonly IApplicationStatusService _applicationStatusService;

        public GetStatusQueryHandler(ILogger<GetStatusQueryHandler> logger, IApplicationStatusService applicationStatusService)
        {
            _logger = logger;
            _applicationStatusService = applicationStatusService;
        }

        public async Task<StatusResponse> Handle(GetStatusQuery request, CancellationToken cancellationToken)
        {
            using var scope = _logger.BeginNamedScope("GetApplicationStatus");
            return await _applicationStatusService.GetApplicationStatusAsync();
        }
    }
}