using Microsoft.Extensions.Logging;
using SimpleServicesDashboard.Application.Models;
using SimpleServicesDashboard.Application.Services.Interfaces;
using System.Reflection;

namespace SimpleServicesDashboard.Application.Services;

/// <summary>
/// Simple service to identify application status to get the main information like configuration and etc.
/// </summary>
public sealed class ApplicationStatusService : IApplicationStatusService
{
    private readonly ILogger<ApplicationStatusService> _logger;

    public ApplicationStatusService(ILogger<ApplicationStatusService> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public Task<StatusResponse> GetApplicationStatusAsync()
    {
        try
        {
            var response = new StatusResponse
            {
                Created = DateTime.UtcNow,
                AppInfo = CollectApplicationInfo()
            };

            return Task.FromResult(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during identifying application status!");

            throw;
        }
    }

    #region Helpers.

    /// <summary>
    /// Collect application information.
    /// </summary>
    /// <returns>Returns application info.</returns>
    private static AppInfo CollectApplicationInfo()
    {
        var assembly = Assembly.GetExecutingAssembly();

        return new AppInfo
        {
            MachineName = Environment.MachineName,
            EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
            ReleaseDate = System.IO.File.GetLastWriteTime(assembly.Location).ToUniversalTime(),
            AppStartTime = System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime(),
            Version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString()
        };
    }

    #endregion
}