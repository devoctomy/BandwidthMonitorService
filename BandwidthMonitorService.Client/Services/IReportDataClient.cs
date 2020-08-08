using BandwidthMonitorService.Dto.Response;
using System.Threading;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Client.Services
{
    [BandwidthMonitorServiceClient(
        UniqueName = "ReportDataClient",
        Implementation = typeof(ReportDataClient))]
    public interface IReportDataClient
    {
        Task<Response<SummedGraphData>> GetSummedGraphDataAsync(
            Dto.Request.GetSummedGraphDataQuery query,
            CancellationToken cancellationToken);
    }
}
