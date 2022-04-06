using System.Net.Http;
using SimpleServicesDashboard.Application.Common.Interfaces;

namespace SimpleServicesDashboard.Infrastructure.Clients
{
    public sealed class EmailServiceClient : BaseHttpClient, IEmailServiceClient
    {
        public EmailServiceClient(HttpClient httpClient) : base(httpClient)
        {
        }
    }
}