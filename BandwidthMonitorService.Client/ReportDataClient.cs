using BandwidthMonitorService.Dto.Request;
using BandwidthMonitorService.Dto.Response;
using System;
using System.Threading.Tasks;

namespace BandwidthMonitorService.Client
{
    public class ReportDataClient : IReportDataClient
    {
        public ReportDataClient()
        {

        }

        public Task<SummedGraphData> GetSummedGraphData(GetSummedGraphDataQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
