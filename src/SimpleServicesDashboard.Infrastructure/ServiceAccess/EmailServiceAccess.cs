using Microsoft.Extensions.Logging;
using SimpleServicesDashboard.Application.Common.Interfaces;
using SimpleServicesDashboard.Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace SimpleServicesDashboard.Infrastructure.ServiceAccess;

public sealed class EmailServiceAccess : IEmailServiceAccess, IServiceAccess
{
    private readonly ILogger<EmailServiceAccess> _logger;
    private readonly IEmailServiceClient _comboServiceClient;

    public EmailServiceAccess(ILogger<EmailServiceAccess> logger, IEmailServiceClient comboServiceClient)
    {
        _logger = logger;
        _comboServiceClient = comboServiceClient;
    }

    public async Task<Models.EmailService.StatusResponse?> GetServiceStatus(string url)
    {
        try
        {
            return await _comboServiceClient.Get<Models.EmailService.StatusResponse>(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during requesting Email Service status");

            return null;
        }
    }

    async Task<StatusResponseBase?> IServiceAccess.GetServiceStatus(string url)
    {
        try
        {
            return await _comboServiceClient.Get<Models.EmailService.StatusResponse>(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during requesting Email Service status");

            return null;
        }
    }
}