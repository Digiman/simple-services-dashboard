using System.Threading.Tasks;
using SimpleServicesDashboard.Api.Models;
using SimpleServicesDashboard.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SimpleServicesDashboard.Api.Pages
{
    /// <summary>
    /// Page to see the service details.
    /// </summary>
    public sealed class ServiceStatus : PageModel
    {
        private readonly IServicesStatusService _servicesStatusService;

        public ServiceStatus(IServicesStatusService servicesStatusService)
        {
            _servicesStatusService = servicesStatusService;
        }

        public ServiceStatusViewModel ServiceStatusViewModel { get; set; }

        public async Task OnGetAsync(string serviceCode, string environment)
        {
            ServiceStatusViewModel = await CollectServiceStatus(serviceCode, environment);
        }

        private async Task<ServiceStatusViewModel> CollectServiceStatus(string serviceCode, string environment)
        {
            var serviceStatus = await _servicesStatusService.GetServiceDetailsAsync(serviceCode, environment);

            var result = new ServiceStatusViewModel
            {
                Url = serviceStatus.Url,
                ServiceName = serviceStatus.ServiceName,
                Environment = serviceStatus.Environment,
                StatusData = serviceStatus.JsonData
            };

            return result;
        }
    }
}