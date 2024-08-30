using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleServicesDashboard.Application.DTOs;
using SimpleServicesDashboard.Application.Modules.Status.Queries;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace SimpleServicesDashboard.Api.Controllers;

/// <summary>
/// Controller to get the status of teh application to monitor activity and work, get configuration details and etc.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Produces("application/json")]
[SwaggerTag("Work with application status and configuration details")]
public sealed class StatusController : BaseApiController
{
    public StatusController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
    { }

    /// <summary>
    /// Get status for application.
    /// </summary>
    /// <returns>Returns information about application and it's status.</returns>
    /// <response code="200">Returns information about application.</response>
    /// <response code="500">Server error happened during processing the request.</response>
    [HttpGet]
    [ProducesResponseType(typeof(StatusResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StatusResponseDto>> GetStatus()
    {
        var query = new GetStatusQuery();
        return await ProcessApiCallAsync<GetStatusQuery, StatusResponseDto>(query);
    }
}