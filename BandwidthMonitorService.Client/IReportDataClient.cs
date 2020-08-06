using System.Threading.Tasks;

namespace BandwidthMonitorService.Client
{
    interface IReportDataClient
    {
        Task<Dto.Response.SummedGraphData> GetSummedGraphData(Dto.Request.GetSummedGraphDataQuery query);
    }
}
