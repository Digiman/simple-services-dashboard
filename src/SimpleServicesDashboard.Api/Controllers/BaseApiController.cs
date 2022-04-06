using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SimpleServicesDashboard.Api.Controllers
{
    /// <summary>
    /// Base class for all API controllers in the service.
    /// </summary>
    public class BaseApiController : ControllerBase
    {
        protected readonly IMediator Mediator;
        protected readonly IMapper Mapper;

        public BaseApiController(IMediator mediator, IMapper mapper)
        {
            Mediator = mediator;
            Mapper = mapper;
        }

        /// <summary>
        /// Process API call with MediatR.
        /// </summary>
        /// <typeparam name="TRequest">Type of the request.</typeparam>
        /// <typeparam name="TResult">Type of the DTO model with result.</typeparam>
        /// <param name="request">Query to process.</param>
        /// <returns>Returns result after processing.</returns>
        protected async Task<TResult> ProcessApiCallAsync<TRequest, TResult>(TRequest request)
        {
            var response = await Mediator.Send(request);

            var result = Mapper.Map<TResult>(response);

            return result;
        }

        /// <summary>
        /// Process API call with MediatR.
        /// </summary>
        /// <typeparam name="TModel">Type of the DTO input model.</typeparam>
        /// <typeparam name="TRequest">Type of the query/command (request) for MediatR.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="model">Input model.</param>
        /// <returns>Returns result after processing.</returns>
        private async Task<TResult> ProcessApiCallAsync<TModel, TRequest, TResult>(TModel model)
        {
            var request = Mapper.Map<TRequest>(model);

            var response = await Mediator.Send(request);

            var result = Mapper.Map<TResult>(response);

            return result;
        }

        /// <summary>
        /// Execute action in the controller.
        /// </summary>
        /// <typeparam name="TModel">Type of the DTO input model.</typeparam>
        /// <typeparam name="TRequest">Type of the query/command (request) for MediatR.</typeparam>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="model">Input model.</param>
        /// <returns>Returns result of the action.</returns>
        protected async Task<ActionResult<TResult>> ExecuteAction<TModel, TRequest, TResult>(TModel model)
        {
            if (model != null)
            {
                return await ProcessApiCallAsync<TModel, TRequest, TResult>(model);
            }

            return BadRequest();
        }

        /// <summary>
        /// Process API call with MediatR without mapping for result.
        /// </summary>
        /// <typeparam name="TRequest">Type of the request.</typeparam>
        /// <typeparam name="TResult">Type of the DTO model with result.</typeparam>
        /// <param name="request">Query to process.</param>
        /// <returns>Returns original result after processing.</returns>
        protected async Task<TResult> ProcessApiCallWithoutMappingAsync<TRequest, TResult>(TRequest request)
        {
            var response = await Mediator.Send(request);

            return (TResult)response;
        }

        /// <summary>
        /// Process API call with MediatR.
        /// </summary>
        /// <param name="request">Model with request data.</param>
        /// <typeparam name="TRequest">Type of the request.</typeparam>
        protected async Task ProcessApiCallAsync<TRequest>(TRequest request)
        {
            await Mediator.Send(request);
        }
    }
}