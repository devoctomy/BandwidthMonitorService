using BandwidthMonitorService.Dto.Request;
using BandwidthMonitorService.Dto.Response;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Client.Services
{
    [BandwidthMonitorServiceClient(UniqueName = "ReportDataClient")]
    public class ReportDataClient : IReportDataClient
    {
        public string UniqueName => typeof(ReportDataClient).GetCustomAttribute<BandwidthMonitorServiceClientAttribute>().UniqueName;

        private readonly HttpClient _httpClient;

        public ReportDataClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(UniqueName);
        }

        public async Task<Response<SummedGraphData>> GetSummedGraphDataAsync(
            GetSummedGraphDataQuery query,
            CancellationToken cancellationToken)
        {
            using (var response = await _httpClient.PostAsync(
                new Uri("v1/ReportData/GetSummedGraphData", UriKind.Relative),
                new StringContent(
                    JsonConvert.SerializeObject(query),
                    System.Text.Encoding.UTF8,
                    "application/json"),
                cancellationToken))
            {
                if(response.IsSuccessStatusCode)
                {
                    var value = JsonConvert.DeserializeObject<SummedGraphData>(await response.Content.ReadAsStringAsync());
                    return new Response<SummedGraphData>()
                    {
                        HttpStatusCode = response.StatusCode,
                        Value = value
                    };
                }
                else
                {
                    return new Response<SummedGraphData>()
                    {
                        HttpStatusCode = response.StatusCode
                    };
                }
            }
        }
    }
}
