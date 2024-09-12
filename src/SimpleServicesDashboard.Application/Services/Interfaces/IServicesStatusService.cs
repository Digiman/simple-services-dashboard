using SimpleServicesDashboard.Application.Models;
using System.Threading.Tasks;

namespace SimpleServicesDashboard.Application.Services.Interfaces;

public interface IServicesStatusService
{
    Task<ServicesStatusResponse> GetServicesStatusAsync();
    Task<ServicesStatusResponse> GetServicesStatusAsync(string code);
    Task<ServiceStatusResponse> GetServiceStatusAsync(string code, string environment);
    Task<ServiceDetailsResponse> GetServiceDetailsAsync(string code, string environment);
    Task<ServicesDescriptionResponse> GetServicesDescriptionAsync();
}