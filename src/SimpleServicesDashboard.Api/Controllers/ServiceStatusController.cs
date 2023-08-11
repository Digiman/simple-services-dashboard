using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleServicesDashboard.Application.DTOs;
using SimpleServicesDashboard.Application.Modules.ServiceStatus.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace SimpleServicesDashboard.Api.Controllers;

/// <summary>
/// Controller to work with the monitored application statuses.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Produces("application/json")]
[SwaggerTag("Work with application statuses and configuration details - monitored services")]
public sealed class ServiceStatusController : BaseApiController
{
    public ServiceStatusController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    { }

    /// <summary>
    /// Get the details about monitored applications.
    /// </summary>
    /// <returns>Returns the list of details for each monitored services and environments.</returns>
    /// <response code="200">Returns information about monitored applications.</response>
    /// <response code="500">Server error happened during processing the request.</response>
    [HttpGet("all")]
    [ProducesResponseType(typeof(ServicesStatusResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServicesStatusResponseDto>> GetApplicationStatuses()
    {
        var query = new GetServicesStatusQuery();
        return await ProcessApiCallAsync<GetServicesStatusQuery, ServicesStatusResponseDto>(query);
    }

    /// <summary>
    /// Get the details about monitored application/service by the code.
    /// </summary>
    /// <param name="code">Application/service code.</param>
    /// <returns>Returns the list of details for each monitored services and environments.</returns>
    /// <response code="200">Returns information about monitored applications.</response>
    /// <response code="500">Server error happened during processing the request.</response>
    [HttpGet("{code}")]
    [ProducesResponseType(typeof(ServiceStatusResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServicesStatusResponseDto>> GetApplicationStatusByCode(string code)
    {
        var query = new GetServiceStatusByCodeQuery
        {
            Code = code
        };
        return await ProcessApiCallAsync<GetServiceStatusByCodeQuery, ServicesStatusResponseDto>(query);
    }

    /// <summary>
    /// Get the details about monitored application/service by the code and environment.
    /// </summary>
    /// <param name="code">Application/service code.</param>
    /// <param name="environment">Environment name (short code - dev, qa, uat, prod).</param>
    /// <returns>Returns the list of details for each monitored services and environments.</returns>
    /// <response code="200">Returns information about monitored applications.</response>
    /// <response code="500">Server error happened during processing the request.</response>
    [HttpGet("{code}/{environment}")]
    [ProducesResponseType(typeof(ServiceStatusResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServiceStatusResponseDto>> GetApplicationStatusByCodeAndEnvironment(string code, string environment)
    {
        var query = new GetServiceStatusQuery
        {
            Code = code,
            Environment = environment
        };
        return await ProcessApiCallAsync<GetServiceStatusQuery, ServiceStatusResponseDto>(query);
    }

    /// <summary>
    /// Get information about configured services (code, name, environments) from configuration.
    /// </summary>
    /// <returns>Returns the list of the configured services with short description.</returns>
    /// <response code="200">Returns information about monitored applications.</response>
    /// <response code="500">Server error happened during processing the request.</response>
    [HttpGet("info")]
    [ProducesResponseType(typeof(ServicesDescriptionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServicesDescriptionResponseDto>> GetApplicationsInfo()
    {
        var query = new GetServicesCodeEnvInfoQuery();
        return await ProcessApiCallAsync<GetServicesCodeEnvInfoQuery, ServicesDescriptionResponseDto>(query);
    }
}