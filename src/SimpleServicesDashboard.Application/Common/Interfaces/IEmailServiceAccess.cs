namespace SimpleServicesDashboard.Application.Common.Interfaces;

public interface IEmailServiceAccess
{
    Task<Infrastructure.Models.EmailService.StatusResponse?> GetServiceStatus(string url);
}