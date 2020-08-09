using BandwidthMonitorService.Dto.Response;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Client.Services
{   
    public class StatusClient : IStatusClient
    {
        public string UniqueName => typeof(IStatusClient).GetCustomAttribute<BandwidthMonitorServiceClientAttribute>().UniqueName;

        private readonly HttpClient _httpClient;

        public StatusClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(UniqueName);
        }
        public async Task<Response<ServiceStatus>> GetServiceStatusAsync(CancellationToken cancellationToken)
        {
            using (var response = await _httpClient.GetAsync(
                new Uri("/v1/Status/Service", UriKind.Relative),
                cancellationToken))
            {
                if (response.IsSuccessStatusCode)
                {
                    var value = JsonConvert.DeserializeObject<ServiceStatus>(await response.Content.ReadAsStringAsync());
                    return new Response<ServiceStatus>()
                    {
                        HttpStatusCode = response.StatusCode,
                        Value = value
                    };
                }
                else
                {
                    return new Response<ServiceStatus>()
                    {
                        HttpStatusCode = response.StatusCode
                    };
                }
            }
        }

    }
}
