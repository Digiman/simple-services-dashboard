using SimpleServicesDashboard.Application.Models;
using System.Threading.Tasks;

namespace SimpleServicesDashboard.Application.Services.Interfaces;

/// <summary>
/// Simple service to identify application status to get the main information like configuration and etc.
/// </summary>
public interface IApplicationStatusService
{
    /// <summary>
    /// Get application status.
    /// </summary>
    /// <returns>Returns the application status.</returns>
    Task<StatusResponse> GetApplicationStatusAsync();
}