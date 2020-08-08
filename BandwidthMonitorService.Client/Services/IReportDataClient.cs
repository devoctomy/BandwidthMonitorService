using BandwidthMonitorService.Dto.Response;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Client.Services
{
    interface IReportDataClient
    {
        Task<Response<SummedGraphData>> GetSummedGraphDataAsync(Dto.Request.GetSummedGraphDataQuery query);
    }
}
