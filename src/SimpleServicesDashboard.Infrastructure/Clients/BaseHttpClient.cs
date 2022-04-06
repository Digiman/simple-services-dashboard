using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SimpleServicesDashboard.Application.Common.Interfaces;
using SimpleServicesDashboard.Infrastructure.Helpers;
using Microsoft.AspNetCore.WebUtilities;

namespace SimpleServicesDashboard.Infrastructure.Clients
{
    public class BaseHttpClient : IBaseHttpClient
    {
        private readonly HttpClient _httpClient;

        protected BaseHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TResponseModel> Get<TResponseModel>(string url, IDictionary<string, string> queryString = null, Dictionary<string, string> headers = null)
        {
            return await Execute<TResponseModel>(HttpMethod.Get, url, queryString: queryString, headers: headers);
        }

        public Task<TResponseModel> Get<TRequest, TResponseModel>(string url, TRequest requestModel, IDictionary<string, string> headers = null)
        {
            IDictionary<string, string> queryString = requestModel.ToDictionary<string>(ParameterToString);
            return Execute<TResponseModel>(HttpMethod.Get, url, queryString, headers);
        }

        public Task<TResponseModel> Post<TRequestModel, TResponseModel>(string url, TRequestModel body, Dictionary<string, string> headers = null)
        {
            return Execute<TResponseModel>(HttpMethod.Post, url, headers: headers, body: body);
        }

        public async Task Post<TRequestModel>(string url, TRequestModel body, Dictionary<string, string> headers = null)
        {
            await Execute<TRequestModel>(HttpMethod.Post, url, headers: headers, body: body);
        }

        public Task<TResponseModel> Put<TRequestModel, TResponseModel>(string url, TRequestModel body, Dictionary<string, string> headers = null)
        {
            return Execute<TResponseModel>(HttpMethod.Put, url, headers: headers, body: body);
        }

        public async Task Put<TRequestModel>(string url, TRequestModel body, Dictionary<string, string> headers = null)
        {
            await Execute<TRequestModel>(HttpMethod.Put, url, headers: headers, body: body);
        }

        public async Task Delete(string url, Dictionary<string, string> headers = null)
        {
            await Execute<object>(HttpMethod.Delete, url, headers: headers);
        }

        #region Helpers.

        private async Task<TResponseModel> Execute<TResponseModel>(HttpMethod method, string url,
            IDictionary<string, string> queryString = null,
            IDictionary<string, string> headers = null,
            object body = null)
        {
            HttpRequestMessage request = BuildRequestMessage(url, method, queryString, headers, body);

            HttpResponseMessage responseMessage = await _httpClient.SendAsync(request);

            responseMessage.EnsureSuccessStatusCode();

            if (StatusCodeCheck(request, responseMessage))
            {
                return await DeserializeResponseBodyAsync<TResponseModel>(responseMessage);
            }

            return await Execute<TResponseModel>(method, url, queryString, headers, body);
        }

        private static async Task<TModel> DeserializeResponseBodyAsync<TModel>(HttpResponseMessage response)
        {
            try
            {
                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var reader = new StreamReader(stream))
                {
                    var content = reader.ReadToEnd();

                    return JsonSerializer.Deserialize<TModel>(content);
                }
            }
            catch (Exception ex)
            {
                var bytes = await response.Content.ReadAsByteArrayAsync();
                throw new InvalidOperationException(Encoding.UTF8.GetString(bytes), ex);
            }
        }

        private static bool StatusCodeCheck(HttpRequestMessage request, HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return false;
            }

            var exception = new InvalidOperationException(BuildExceptionMessage(request, response));
            exception.Data.Add("HttpStatusCode", response.StatusCode);

            throw exception;
        }

        private static string BuildExceptionMessage(HttpRequestMessage request, HttpResponseMessage response)
        {
            var message =
                $"Can't {response.RequestMessage.Method} {response.RequestMessage.RequestUri}.{Environment.NewLine}" +
                $"Status code = {response.StatusCode},{Environment.NewLine}Reason: {response.ReasonPhrase}{Environment.NewLine}ClientHeaders{Environment.NewLine}";

            return request.Headers.Aggregate(message,
                (current, header) => current + $"{header.Key}:{header.Value}{Environment.NewLine}");
        }

        private HttpRequestMessage BuildRequestMessage(string url, HttpMethod method,
            IDictionary<string, string> queryString = null, IDictionary<string, string> customHeaders = null,
            object body = null)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException(nameof(url));
            }

            string requestUri = url;
            if (queryString != null)
            {
                requestUri = QueryHelpers.AddQueryString(url, queryString);
            }

            var requestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(requestUri),
                Method = method
            };

            SetBody(requestMessage, body);
            SetHeaders(requestMessage, customHeaders);

            return requestMessage;
        }

        private static void SetBody(HttpRequestMessage requestMessage, object body)
        {
            if (body != null)
            {
                var jsonBody = JsonSerializer.Serialize(body, body.GetType());
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                requestMessage.Content = content;
            }
        }

        private static void SetHeaders(HttpRequestMessage requestMessage, IDictionary<string, string> headers)
        {
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (headers != null)
            {
                foreach (var pair in headers)
                {
                    requestMessage.Headers.Add(pair.Key, pair.Value);
                }
            }
        }

        /// <summary>
        /// If parameter is DateTime, output in a formatted string (default ISO 8601), customizable with Configuration.DateTime.
        /// If parameter is a list of string, join the list with ",".
        /// Otherwise just return the string.
        /// </summary>
        /// <param name="obj">The parameter (header, path, query, form).</param>
        /// <returns>Formatted string.</returns>
        private static string ParameterToString(object obj)
        {
            switch (obj)
            {
                // Return a formatted date string - Can be customized with Configuration.DateTimeFormat
                // Defaults to an ISO 8601, using the known as a Round-trip date/time pattern ("o")
                // https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx#Anchor_8
                // For example: 2009-06-15T13:45:30.0000000
                case DateTime dateTime:
                    return dateTime.ToString(DefaultDateTimeFormat);
                case ICollection<int> list:
                    return string.Join(",", list.ToArray());
                case ICollection<object> list:
                    return string.Join(",", list.Select(i => i.ToString()));
                default:
                    return Convert.ToString(obj);
            }
        }

        private const string DefaultDateTimeFormat = "o";

        #endregion
    }
}
