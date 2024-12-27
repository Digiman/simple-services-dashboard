using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace SimpleServicesDashboard.Application.Common.Behaviours;

/// <summary>
/// Common logic to log the request processing with MediatR.
/// </summary>
/// <typeparam name="TRequest">Request type.</typeparam>
/// <typeparam name="TResponse">Response type.</typeparam>
public sealed class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation($"Sending a request {requestName}");

        var sw = new Stopwatch();
        sw.Start();

        var response = await next();

        sw.Stop();

        var responseName = typeof(TRequest).Name;
        _logger.LogInformation($"Response {responseName} has been got. Elapsed time = {sw.ElapsedMilliseconds} ms");
        return response;
    }
}