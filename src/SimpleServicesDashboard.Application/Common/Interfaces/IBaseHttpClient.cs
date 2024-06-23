using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleServicesDashboard.Application.Common.Interfaces;

public interface IBaseHttpClient
{
    Task<TResponseModel?> Get<TResponseModel>(string url, IDictionary<string, string>? queryString = null, Dictionary<string, string>? headers = null);

    Task<TResponseModel?> Get<TRequest, TResponseModel>(string url, TRequest? requestModel, IDictionary<string, string>? headers = null);

    Task<TResponseModel?> Post<TRequestModel, TResponseModel>(string url, TRequestModel? body, Dictionary<string, string>? headers = null);

    Task Post<TRequestModel>(string url, TRequestModel? body, Dictionary<string, string>? headers = null);

    Task<TResponseModel?> Put<TRequestModel, TResponseModel>(string url, TRequestModel? body, Dictionary<string, string>? headers = null);

    Task Put<TRequestModel>(string url, TRequestModel? body, Dictionary<string, string>? headers = null);

    Task Delete(string url, Dictionary<string, string>? headers = null);
}