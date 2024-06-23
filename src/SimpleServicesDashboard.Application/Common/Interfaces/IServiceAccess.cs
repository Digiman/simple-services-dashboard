using System.Threading.Tasks;

namespace SimpleServicesDashboard.Application.Common.Interfaces;

public interface IServiceAccess
{
    Task<Infrastructure.Models.StatusResponseBase?> GetServiceStatus(string url);
}