using SimpleServicesDashboard.Application.Common.Interfaces;
using System.Net.Http;

namespace SimpleServicesDashboard.Infrastructure.Clients;

public sealed class EmailServiceClient : BaseHttpClient, IEmailServiceClient
{
    public EmailServiceClient(HttpClient httpClient) : base(httpClient)
    {
    }
}