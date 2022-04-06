using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace SimpleServicesDashboard.Application.Common.Processors
{
    /// <summary>
    /// Logger for all the responses in the MediatR.
    /// </summary>
    /// <typeparam name="TRequest">Request.</typeparam>
    /// <typeparam name="TResponse">Response.</typeparam>
    public sealed class ResponseLogger<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TResponse> _logger;

        public ResponseLogger(ILogger<TResponse> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            var responseName = typeof(TResponse).Name;
            _logger.LogInformation("InboundCallingService Response: {Name} {@Response}", responseName, response);

            return Task.CompletedTask;
        }
    }
}