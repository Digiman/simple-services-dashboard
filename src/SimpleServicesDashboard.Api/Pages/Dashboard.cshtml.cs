using System.Linq;
using System.Threading.Tasks;
using SimpleServicesDashboard.Api.Models;
using SimpleServicesDashboard.Application.Services.Interfaces;
using SimpleServicesDashboard.Common.Configuration;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace SimpleServicesDashboard.Api.Pages
{
    /// <summary>
    /// Simple default page with details for each monitored service (very basic).
    /// </summary>
    public sealed class Dashboard : PageModel
    {
        private readonly IServicesStatusService _servicesStatusService;
        private readonly ServicesConfigurationOptions _servicesConfiguration;

        /// <summary>
        /// Model with services details to show on the page after load.
        /// </summary>
        public DashboardViewModel DashboardData { get; set; }

        public Dashboard(IServicesStatusService servicesStatusService,
            IOptions<ServicesConfigurationOptions> servicesConfiguration)
        {
            _servicesStatusService = servicesStatusService;
            _servicesConfiguration = servicesConfiguration.Value;
        }

        public async Task OnGetAsync()
        {
            DashboardData = await CollectServicesDetails();
        }

        private async Task<DashboardViewModel> CollectServicesDetails()
        {
            var details = await _servicesStatusService.GetServicesStatusAsync();

            var model = new DashboardViewModel();

            // build the model to prepare the data in the block on the page
            var servicesGroup = details.Statuses.GroupBy(x => x.Code);

            model.Services = servicesGroup.Select(x =>
                new ServiceViewModel
                {
                    Code = x.Key,
                    Name = _servicesConfiguration.Services.FirstOrDefault(conf => conf.Code == x.Key)?.Name ?? "",
                    Environments = x.ToDictionary(s => s.Environment, s => new ServiceEnvironmentViewModel
                    {
                        Name = s.Name,
                        Created = s.Created,
                        MachineName = s.MachineName,
                        EnvironmentName = s.EnvironmentName,
                        ReleaseDate = s.ReleaseDate,
                        AppStartTime = s.AppStartTime,
                        Version = s.Version,
                        BaseUrl = s.BaseUrl
                    })
                }).ToList();

            // get environments configuration
            model.Environments = _servicesConfiguration.Environments.ToDictionary(x => x.Code, x => x.Name);

            return model;
        }
    }
}