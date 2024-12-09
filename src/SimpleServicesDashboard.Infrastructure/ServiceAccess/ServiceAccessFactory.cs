using Microsoft.Extensions.DependencyInjection;
using SimpleServicesDashboard.Application.Common.Interfaces;

namespace SimpleServicesDashboard.Infrastructure.ServiceAccess;

/// <summary>
/// Simple factory to create needed instance of the service access class instance to access to the service via API.
/// </summary>
public sealed class ServiceAccessFactory : IServiceAccessFactory
{
    private readonly IServiceProvider _serviceProvider;

    private readonly Dictionary<string, Type> _servicesAccessMap = new()
    {
        { "email", typeof(EmailServiceAccess) }
    };

    public ServiceAccessFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public IServiceAccess GetServiceAccess(string serviceCode)
    {
        var type = _servicesAccessMap[serviceCode];
        return (IServiceAccess)ActivatorUtilities.CreateInstance(_serviceProvider, type);
    }
}