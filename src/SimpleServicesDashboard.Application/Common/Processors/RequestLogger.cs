using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace SimpleServicesDashboard.Application.Common.Processors
{
    /// <summary>
    /// Logger for al requests in the MediatR.
    /// </summary>
    /// <typeparam name="TRequest">Request.</typeparam>
    public sealed class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger<TRequest> _logger;

        public RequestLogger(ILogger<TRequest> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogInformation("InboundCallingService Request: {Name} {@Request}", requestName, request);

            return Task.CompletedTask;
        }
    }
}