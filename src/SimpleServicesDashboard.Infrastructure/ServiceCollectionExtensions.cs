using Microsoft.Extensions.DependencyInjection;
using SimpleServicesDashboard.Application.Common.Interfaces;
using SimpleServicesDashboard.Infrastructure.Clients;
using SimpleServicesDashboard.Infrastructure.ServiceAccess;

namespace SimpleServicesDashboard.Infrastructure;

/// <summary>
/// Dependency registrator for Application stuff.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register application level dependencies and services.
    /// </summary>
    /// <param name="services">Services collection.</param>
    /// <returns>Returns updated services collection.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        RegisterApiClients(services);

        RegisterServiceAccess(services);

        return services;
    }

    private static void RegisterApiClients(IServiceCollection services)
    {
        // register clients - API clients
        services.AddHttpClient<IEmailServiceClient, EmailServiceClient>("EmailServiceClient")
            .AddStandardResilienceHandler();
    }

    private static void RegisterServiceAccess(IServiceCollection services)
    {
        services.AddTransient<IServiceAccessFactory, ServiceAccessFactory>();

        // register service access for each individual monitored service
        services.AddTransient<IEmailServiceAccess, EmailServiceAccess>();
    }
}