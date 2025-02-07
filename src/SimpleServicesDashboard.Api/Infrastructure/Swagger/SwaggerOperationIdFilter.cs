using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SimpleServicesDashboard.Api.Infrastructure.Swagger;

/// <summary>
/// Represents the Swagger/Swashbuckle operation filter used to document the implicit API operationId to be generated.
/// </summary>
public sealed class SwaggerOperationIdFilter : IOperationFilter
{
    /// <summary>
    /// Applies the filter to the specified operation using the given context.
    /// </summary>
    /// <param name="operation">The operation to apply the filter to.</param>
    /// <param name="context">The current operation filter context.</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var controllerName = context.ApiDescription.ActionDescriptor.RouteValues["controller"];
        var actionName = context.ApiDescription.ActionDescriptor.RouteValues["action"];
        operation.OperationId = $"{controllerName}_{actionName}";
    }
}